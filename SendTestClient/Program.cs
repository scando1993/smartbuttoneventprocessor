using System;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Threading.Tasks;
using RecieveEPHClient;
using Newtonsoft.Json;

namespace SendTestClient
{
    class Program
    {
        private static EventHubClient eventHubClient;
        private const string EventHubConnectionString = "Endpoint=sb://iot-button.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=5FolQBU9YxZD5p2oF96Vo623pwv+r+9jPIyn8kqbPm0=";

        //private const string EventHubConnectionString = "Endpoint=sb://ihsuprodbnres010dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=mVuc5wXe0aIEcKN9chP29GV7xxOrEhLym9Shz3aNDR0=;";

        private const string EventHubName = "eventhubdev";

        //private const string EventHubName = "iothub-ehub-iot-smartb-1102955-43eaefe60e";

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but this simple scenario
            // uses the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
            {
                EntityPath = EventHubName
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub(100);

            await eventHubClient.CloseAsync();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        // Creates an event hub client and sends 100 messages to the event hub.
        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            Random rnd = new Random();
            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    SmartButton sm = new SmartButton
                    {
                        DeviceId = "" + i,
                        Status = rnd.Next(0, 2),
                        Data = "0000",
                        Latitude =rnd.NextDouble() * 100 + (-10),
                        Longitude = rnd.NextDouble() * 100 + (-20)
                    };

                    Console.WriteLine($"Button {sm.DeviceId} pressed");
                    string sm_json = JsonConvert.SerializeObject(sm, Formatting.Indented);
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(sm_json)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10);
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}
