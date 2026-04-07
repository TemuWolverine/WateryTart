using Avalonia;
using Microsoft.Extensions.Logging;
using Sendspin.SDK.Audio;
using Sendspin.SDK.Client;
using Sendspin.SDK.Connection;
using Sendspin.SDK.Models;
using Sendspin.SDK.Synchronization;
using SendspinClient.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;
using WateryTart.Core.Services;

namespace WateryTart.Core.Playback;

public class SendSpinClient : IDisposable, IReaper
{
    private readonly SendspinClientService _sendspinClient;
    private readonly AudioPipeline _audioPipeline;
    private readonly ILogger<SendSpinClient> _logger;
    private AudioPlayerState _state = AudioPlayerState.Uninitialized;
    private bool _isConnected;
    private bool _disposed;

    public event EventHandler<PlaybackChangedEventArgs> OnPlaybackChanged;
    public event EventHandler<ErrorEventArgs> OnError;
    public event EventHandler<EventArgs> OnConnected;
    public event EventHandler<EventArgs> OnDisconnected;
    public event EventHandler<AudioPlayerState>? StateChanged;
    public event EventHandler<AudioPlayerError>? ErrorOccurred;

    public AudioPlayerState State
    {
        get => _state;
        private set
        {
            if (_state != value)
            {
                _state = value;
                StateChanged?.Invoke(this, value);
            }
        }
    }

    public SendSpinClient(IPlayerFactory? player = null, ILoggerFactory? loggerFactory = null)
    {
        if (OperatingSystem.IsAndroid())
            return;

        try
        {
            loggerFactory ??= LoggerFactory.Create(b =>
            {
                b.AddConsole();
                b.SetMinimumLevel(LogLevel.Debug);
            });

            _logger = loggerFactory.CreateLogger<SendSpinClient>();

            var connection = new SendspinConnection(loggerFactory.CreateLogger<SendspinConnection>());

            var forgetFactor = 1.001;
            var adaptiveCutoff = 0.75;
            var minSamplesForForgetting = 100;

            var clockSync = new KalmanClockSynchronizer(
                loggerFactory.CreateLogger<KalmanClockSynchronizer>(),
                forgetFactor: forgetFactor,
                adaptiveCutoff: adaptiveCutoff,
                minSamplesForForgetting: minSamplesForForgetting);

            clockSync.StaticDelayMs = 0;

            var decoderFactory = new AudioDecoderFactory();

            _audioPipeline = new AudioPipeline(
                loggerFactory.CreateLogger<AudioPipeline>(),
                decoderFactory,
                clockSync,
                bufferFactory: (format, sync) =>
                {
                    var buffer = new TimedAudioBuffer(format, sync, 120000, syncOptions: null, loggerFactory.CreateLogger<TimedAudioBuffer>());
                    buffer.TargetBufferMilliseconds = 250;
                    return buffer;
                },
                playerFactory: player.CreatePlayer,
                sourceFactory: (buffer, timeFunc) => new BufferedAudioSampleSource(buffer, timeFunc),
                precisionTimer: null,
                waitForConvergence: true,
                convergenceTimeoutMs: 5000);

            var capabilities = new ClientCapabilities
            {
                ClientName = $"{Environment.MachineName} (WateryTart)",
                ProductName = "WateryTart",
                Manufacturer = "TemuWolverine",
                SoftwareVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(), 
                InitialVolume = 100,
                InitialMuted = false,
                AudioFormats = AudioFormatBuilder.BuildFormats(
                    new AudioDeviceCapabilities
                    {
                        NativeSampleRate = 48000,
                        NativeBitDepth = 32,
                    },
                    preferredCodec: "flac")
            };

            _sendspinClient = new SendspinClientService(
                loggerFactory.CreateLogger<SendspinClientService>(),
                connection,
                clockSync,
                capabilities,
                _audioPipeline
            );

            _sendspinClient.GroupStateChanged += HandleGroupStateChanged;
            _sendspinClient.PlayerStateChanged += HandlePlayerStateChanged;

            State = AudioPlayerState.Stopped;
            _logger.LogInformation("SendSpinNAudioClient initialized.");
        }
        catch (Exception ex)
        {
            State = AudioPlayerState.Error;
            _logger.LogError(ex, "Error initializing SendSpinNAudioClient");
            OnError?.Invoke(this, new ErrorEventArgs(ex));
            ErrorOccurred?.Invoke(this, new AudioPlayerError(ex.Message, ex));
            throw;
        }
    }

    public async Task ConnectAsync(string serverUri)
    {
        //for now, assume the url 
        var temp = new Uri("ws://" + serverUri);
        
        //and also assuming the port, default/recommended port used
        serverUri = $"ws://{temp.Host}:8927/sendspin";

        if (_isConnected)
        {
            _logger.LogInformation("Already connected to Sendspin server.");
            return;
        }

        try
        {
            await _sendspinClient.ConnectAsync(new Uri(serverUri));
            _isConnected = true;
            State = AudioPlayerState.Stopped;
            _logger.LogInformation("Connected to Sendspin server: {ServerUri}", serverUri);
            OnConnected?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            _isConnected = false;
            State = AudioPlayerState.Error;
            _logger.LogError(ex, "Error connecting to Sendspin server");
            OnError?.Invoke(this, new ErrorEventArgs(ex));
            ErrorOccurred?.Invoke(this, new AudioPlayerError($"Connection failed: {ex.Message}", ex));
            throw;
        }
    }

    public async Task DisconnectAsync()
    {
        if (!_isConnected)
            return;

        try
        {
            await _sendspinClient.DisconnectAsync();
            _isConnected = false;
            State = AudioPlayerState.Stopped;
            _logger.LogInformation("Disconnected from Sendspin server.");
            OnDisconnected?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            State = AudioPlayerState.Error;
            _logger.LogError(ex, "Error disconnecting from Sendspin server");
            OnError?.Invoke(this, new ErrorEventArgs(ex));
            ErrorOccurred?.Invoke(this, new AudioPlayerError($"Disconnection failed: {ex.Message}", ex));
        }
    }

    /// <summary>
    /// Stop audio playback.
    /// </summary>
    public void Stop()
    {
        try
        {
            if (State == AudioPlayerState.Stopped || State == AudioPlayerState.Uninitialized)
                return;

            State = AudioPlayerState.Stopped;
            _logger.LogInformation("Audio playback stopped");
        }
        catch (Exception ex)
        {
            State = AudioPlayerState.Error;
            _logger.LogError(ex, "Error stopping playback");
            ErrorOccurred?.Invoke(this, new AudioPlayerError($"Playback stop failed: {ex.Message}", ex));
            throw;
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        GC.SuppressFinalize(this);
        try
        {
            Stop();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during cleanup");
        }

        _disposed = true;
    }

    private void HandleGroupStateChanged(object sender, GroupState group)
    {
        OnPlaybackChanged?.Invoke(this, new PlaybackChangedEventArgs { GroupState = group });
    }

    private void HandlePlayerStateChanged(object sender, PlayerState playerState)
    {
        _logger.LogInformation(
            "Player state changed: Volume={Volume}%, Muted={Muted}",
            playerState?.Volume ?? 0,
            playerState?.Muted ?? false
        );
    }

    public void Reap()
    {
        _ =  _audioPipeline.DisposeAsync();
        _ =  DisconnectAsync();
        Dispose();
    }
}