using System.Collections;
using WateryTart.MassClient.Messages;
using WateryTart.MassClient.Responses;

namespace WateryTart.MassClient;
public static partial class MassClientExtensions
{


    extension(IMassWsClient c)
    {
        public void GetAuthToken(string username, string password, Action<AuthResponse> responseHandler)
        {
            var m = new Message(Commands.AuthLogin)
            {
                args = new Hashtable
                {
                    { "username", username },
                    { "password", password }
                }
            };

            c.Send<AuthResponse>(m, Deserialise<AuthResponse>(responseHandler), true);
        }
    }
}