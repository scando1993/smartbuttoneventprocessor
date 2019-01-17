using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RecieveEPHClient
{
    public class EventProcessor
    {
        public static int events = 0;
        private static string EventHubConnectionString = "Endpoint=sb://iot-button.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=5FolQBU9YxZD5p2oF96Vo623pwv+r+9jPIyn8kqbPm0=";

        private string EventHubName = "eventhubdev";

        private string StorageContainerName = "smartbutton2";
        private static string StorageAccountName = "pacificsoft";
        private static string StorageAccountKey = "WLCQMzj06pQH7+30PbcuGz6GIHVoMiZB0LGrVHQ7eFxv23cf8btApz7ZBI/eHt1k00x28slVWe17suJPtU9wFg==";

        private readonly string StorageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);

        private ManualResetEvent Wait = new ManualResetEvent(false);

        public async Task MainAsync(int time = 0)
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

            Console.WriteLine("Receiving data.");
            //if (time == 0) {
            Wait.WaitOne();
            //}
            System.Threading.Thread.Sleep(time);
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
