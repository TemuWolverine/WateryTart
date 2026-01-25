using WateryTart.MassClient.Messages;
using WateryTart.MassClient.Models.Auth;

namespace WateryTart.MassClient;

public interface IMassWsClient
{
    public Task<MassCredentials> Login(string username, string password, string baseurl);

    public Task Connect(IMassCredentials credentials);

    public void Send<T>(MessageBase message, Action<string> ResponseHandler);

    bool IsConnected { get; }
}