using Newtonsoft.Json;
using System.Collections;
using WateryTart.MassClient.Messages;
using WateryTart.MassClient.Models;
using WateryTart.MassClient.Responses;

namespace WateryTart.MassClient;


public static partial class MassClientExtensions
{
    private static MessageBase JustCommand(string command)
    {
        return new Message(command);
    }
    private static MessageBase JustId(string command, string id, string id_label = "item_id")
    {
        var m = new Message(command)
        {
            args = new Hashtable
            {
                { id_label, id },
            }
        };

        return m;
    }
    private static MessageBase IdAndProvider(string command, string id, string provider)
    {
        var m = new Message(command)
        {
            args = new Hashtable
            {
                { "item_id", id },
                { "provider_instance_id_or_domain", provider }
            }
        };

        return m;
    }


    private static Action<string> Deserialise<T>(Action<T> responseHandler)
    {

        Action<string> d = (r) =>
        {
            if (r == null)
                return;

            var y = JsonConvert.DeserializeObject<T>(r);
            responseHandler(y);
        };

        return d;
    }
}