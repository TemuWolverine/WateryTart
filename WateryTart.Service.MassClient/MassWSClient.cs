using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Text.Json.Serialization;
using WateryTart.Service.MassClient.Events;
using WateryTart.Service.MassClient.Messages;
using WateryTart.Service.MassClient.Models.Auth;
using WateryTart.Service.MassClient.Models.Enums;
using WateryTart.Service.MassClient.Responses;
using Websocket.Client;

namespace WateryTart.Service.MassClient
{
    public class MassWsClient : IMassWsClient
    {
        private string _baseUrl;
        internal WebsocketClient _client;
        internal ConcurrentDictionary<string, Action<string>> _routing = new();  // Changed from Dictionary
        private IMassCredentials creds;

        private CancellationTokenSource _connectionCts = new CancellationTokenSource();
        private readonly Subject<BaseEventResponse> subject = new Subject<BaseEventResponse>();
        private IDisposable _reconnectionSubscription;
        private IDisposable _messageSubscription;

        // Track if we're already attempting to connect
        private Task _currentConnectTask = Task.CompletedTask;
        private readonly object _connectLock = new object();

        private bool _isAuthenticated = false;
        private readonly Queue<string> _pendingMessages = new();
        private readonly object _authLock = new object();

        /// <summary>
        /// Shared JSON serializer options for all MassClient operations. AOT-compatible with snake_case naming.
        /// </summary>
        internal static readonly JsonSerializerOptions SerializerOptions = new()
        {
            TypeInfoResolver = MassClientJsonContext.Default,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false

        };

        public async Task<LoginResults> Login(string username, string password, string baseurl)
        {
            MassCredentials mc = new MassCredentials();

            var factory = new Func<ClientWebSocket>(() => new ClientWebSocket
            {
                Options = { KeepAliveInterval = TimeSpan.FromSeconds(1) }
            });

            // Music Assistant uses port 8095 for HTTP and 8097 for WebSocket API
            var wsUrl = GetWebSocketUrl(baseurl);

            var tcs = new TaskCompletionSource<LoginResults>();
            using (_client = new WebsocketClient(new Uri(wsUrl), factory))
            {
                Console.WriteLine($"Connecting to WebSocket: {wsUrl}");
                _client.MessageReceived.Subscribe(OnNext);
                await _client.Start();
                Console.WriteLine("WebSocket connected, sending auth request");

                this.GetAuthToken(username, password, (response) =>
                {
                    Console.WriteLine($"Auth response received: success={response?.Result?.success}");
                    if (response?.Result == null)
                    {
                        tcs.TrySetResult(new LoginResults { Success = false, Error = "No response from server" });
                        return;
                    }
                    if (!response.Result.success)
                    {
                        var r = new LoginResults
                        {
                            Success = false,
                            Error = response.Result.error ?? "Authentication failed"

                        };
                        tcs.TrySetResult(r);
                        return;
                    }
                    var success = new LoginResults
                    {
                        Credentials = new MassCredentials
                        {
                            Token = response.Result.access_token,
                            BaseUrl = baseurl
                        },
                        Success = true
                    };
                    tcs.TrySetResult(success);
                });

                // Add timeout to prevent hanging forever
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(10));
                var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    return new LoginResults { Success = false, Error = "Connection timed out" };
                }

                return await tcs.Task;
            }
        }

        public async Task<bool> Connect(IMassCredentials credentials)
        {
            var x = new MassClientJsonContext();
            Console.WriteLine($"WS Connecting");

            lock (_connectLock)
            {
                if (!_currentConnectTask.IsCompleted)
                {
                    return false;
                }
            }

            creds = credentials;
            _isAuthenticated = false;

            _reconnectionSubscription?.Dispose();
            _messageSubscription?.Dispose();
            _client?.Dispose();
            _connectionCts = new CancellationTokenSource();

            var factory = new Func<ClientWebSocket>(() => new ClientWebSocket
            {
                Options = { KeepAliveInterval = TimeSpan.FromSeconds(1) }
            });

            var wsUrl = GetWebSocketUrl(credentials.BaseUrl);
            _client = new WebsocketClient(new Uri(wsUrl), factory);

            _reconnectionSubscription = _client.ReconnectionHappened.Subscribe(info =>
            {
                if (!_connectionCts.Token.IsCancellationRequested)
                {
                    SendLogin(credentials);
                }
            });

            _messageSubscription = _client.MessageReceived.Subscribe(OnNext);

            await _client.Start();
            SendLogin(credentials);

            var authTimeout = Task.Delay(TimeSpan.FromSeconds(10), _connectionCts.Token);
            var authCompleted = WaitForAuthenticationAsync();

            var completedTask = await Task.WhenAny(authCompleted, authTimeout);

            if (completedTask == authTimeout)
            {
                Console.WriteLine("Authentication timeout");
                return false;
            }

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(Timeout.Infinite, _connectionCts.Token);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Background connection task cancelled");
                }
            });

            return _isAuthenticated;
        }

        /// <summary>
        /// Converts a base URL (e.g., "192.168.1.63:8095") to the WebSocket URL.
        /// Music Assistant WebSocket is on the same port as HTTP.
        /// </summary>
        private static string GetWebSocketUrl(string baseUrl)
        {
            // WebSocket is on the same port as HTTP, just different protocol
            return $"ws://{baseUrl}/ws";
        }

        private void SendLogin(IMassCredentials credentials)
        {
            Console.WriteLine("Sending authentication...");
            var argsx = new Dictionary<string, object>() { { "token", credentials.Token } };
            var auth = new Auth()
            {
                message_id = "auth-" + Guid.NewGuid(),
                args = argsx
            };

            _routing.TryAdd(auth.message_id, (response) =>
            {
                Console.WriteLine($"Auth response: {response}");

                if (!response.Contains("error"))
                {
                    lock (_authLock)
                    {
                        _isAuthenticated = true;

                        // Send all pending messages
                        while (_pendingMessages.Count > 0)
                        {
                            var pending = _pendingMessages.Dequeue();
                            _client?.Send(pending);
                        }
                    }
                }
            });

            var json = JsonSerializer.Serialize(auth, SerializerOptions);
            Console.WriteLine($"Sending auth: {json}");
            _client?.Send(json);
        }

        public void Send<T>(MessageBase message, Action<string> responseHandler, bool ignoreConnection = false)
        {

            var json = message.ToJson();
            _routing.TryAdd(message.message_id, responseHandler);  // Changed from Add

            if (!ignoreConnection && (_client == null || !_client.IsRunning))
            {
                lock (_connectLock)
                {
                    if (_currentConnectTask.IsCompleted)
                    {
                        _currentConnectTask = ConnectSafely();
                    }
                }
            }

            _client?.Send(json);
        }

        private async Task ConnectSafely()
        {
            Debug.WriteLine($"WS Connecting");
            try
            {
                await Connect(creds);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Connect error: {ex}");
            }
        }

        public bool IsConnected => (_client != null && _client.IsRunning);

        private void OnNext(ResponseMessage response)
        {
            if (string.IsNullOrEmpty(response.Text))
                return;

            if (response.Text.Contains("\"server_id\"") && !response.Text.Contains("\"message_id\""))
            {
                return;
            }

            TempResponse y = JsonSerializer.Deserialize<TempResponse>(response.Text, SerializerOptions);

            // Use TryRemove instead of ContainsKey + indexer
            if (y?.message_id != null && _routing.TryRemove(y.message_id, out var handler))
            {
                handler?.Invoke(response.Text);
                return;
            }

            try
            {
                var e = JsonSerializer.Deserialize<BaseEventResponse>(response.Text, SerializerOptions);
                switch (e.EventName)
                {
                    case EventType.MediaItemPlayed:
                        subject.OnNext(JsonSerializer.Deserialize<MediaItemEventResponse>(response.Text, SerializerOptions));
                        break;
                    case EventType.PlayerAdded:
                    case EventType.PlayerUpdated:
                    case EventType.PlayerRemoved:
                    case EventType.PlayerConfigUpdated:
                        try
                        {
                            var x = JsonSerializer.Deserialize<PlayerEventResponse>(response.Text, SerializerOptions);
                            subject.OnNext(x);
                        }
                        catch (Exception ex)
                        {

                        }

                        break;

                    case EventType.QueueAdded:
                    case EventType.QueueUpdated:
                        subject.OnNext(JsonSerializer.Deserialize<PlayerQueueEventResponse>(response.Text, SerializerOptions));
                        break;
                    case EventType.QueueItemsUpdated:
                        break;
                    case EventType.QueueTimeUpdated:
                        subject.OnNext(JsonSerializer.Deserialize<PlayerQueueTimeUpdatedEventResponse>(response.Text, SerializerOptions));
                        break;

                    default:
                        subject.OnNext(e);
                        break;
                }
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"JSON Deserialization Error: {ex.Message}");
                Debug.WriteLine($"Path: {ex.Path}");
            }
        }

        public IObservable<BaseEventResponse> Events => subject;

        public async Task DisconnectAsync()
        {
            try
            {
                // Cancel the connection immediately
                _connectionCts?.Cancel();
            }
            catch { }

            try
            {
                if (_client != null)
                {
                    // Disable reconnection first
                    _client.IsReconnectionEnabled = false;

                    if (_client.IsRunning)
                    {
                        try
                        {
                            await _client.Stop(WebSocketCloseStatus.NormalClosure, "Shutdown");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error calling Stop: {ex}");
                        }

                        // Force abort if still running
                        if (_client.IsRunning)
                        {
                            Debug.WriteLine("WebSocket still running, attempting abort...");
                            _client.NativeClient?.Abort();
                        }

                        await Task.Delay(500);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error stopping WebSocket: {ex}");
            }

            try
            {
                _reconnectionSubscription?.Dispose();
                _messageSubscription?.Dispose();
                _client?.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error disposing WebSocket: {ex}");
            }

            try
            {
                _connectionCts?.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error disposing CTS: {ex}");
            }

            try
            {
                subject?.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error disposing subject: {ex}");
            }
        }
        private async Task WaitForAuthenticationAsync()
        {
            while (!_isAuthenticated && !_connectionCts.Token.IsCancellationRequested)
            {
                await Task.Delay(100);
            }
        }
    }
}