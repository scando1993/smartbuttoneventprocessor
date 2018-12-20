using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RecieveEPHClient
{
    class Program
    {

        //var s = createToken("https://iot-button.servicebus.windows.net/eventhubdev/messages", "RootManageSharedAccessKey", "5FolQBU9YxZD5p2oF96Vo623pwv+r+9jPIyn8kqbPm0=");

        private const string EventHubConnectionString = "Endpoint=sb://iot-button.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=5FolQBU9YxZD5p2oF96Vo623pwv+r+9jPIyn8kqbPm0=";

        //Endpoint=sb://ihsuprodbnres010dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=mVuc5wXe0aIEcKN9chP29GV7xxOrEhLym9Shz3aNDR0=;EntityPath=iothub-ehub-iot-smartb-1102955-43eaefe60e

        //private const string EventHubConnectionString = "Endpoint=sb://ihsuprodbnres010dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=mVuc5wXe0aIEcKN9chP29GV7xxOrEhLym9Shz3aNDR0=;";


        private const string EventHubName = "eventhubdev";
        //private const string EventHubName = "iothub-ehub-iot-smartb-1102955-43eaefe60e";

        //private const string StorageContainerName = "smartbutton";
        private const string StorageContainerName = "smartbutton2";
        private const string StorageAccountName = "pacificsoft";
        private const string StorageAccountKey = "WLCQMzj06pQH7+30PbcuGz6GIHVoMiZB0LGrVHQ7eFxv23cf8btApz7ZBI/eHt1k00x28slVWe17suJPtU9wFg==";

        private static readonly string StorageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Registering EventProcessor...");

            var eventProcessorHost = new EventProcessorHost(
                EventHubName,
                PartitionReceiver.DefaultConsumerGroupName,
                EventHubConnectionString,
                StorageConnectionString,
                StorageContainerName);

            // Registers the Event Processor Host and starts receiving messages
            await eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>();

            Console.WriteLine("Receiving. Press ENTER to stop worker.");
            Console.ReadLine();

            // Disposes of the Event Processor Host
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }

        //private static string createToken(string resourceUri, string keyName, string key)
        //{
        //    TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
        //    var week = 60 * 60 * 24 * 7;
        //    var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + week);
        //    string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
        //    HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        //    var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
        //    var sasToken = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}", HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);
        //    return sasToken;
        //}
    }
}
