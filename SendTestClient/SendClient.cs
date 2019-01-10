using System;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Threading.Tasks;
using RecieveEPHClient;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SendTestClient
{
    public class SendClient
    {
        private EventHubClient eventHubClient;
        private const string EventHubConnectionString = "Endpoint=sb://iot-button.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=5FolQBU9YxZD5p2oF96Vo623pwv+r+9jPIyn8kqbPm0=";

        private const string EventHubName = "eventhubdev";

        public int mensajesEnviados = 0;

        public async Task MainAsync(List<string> DummyData)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but this simple scenario
            // uses the connection string from the namespace.

            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
            {
                EntityPath = EventHubName
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub(DummyData);
            await eventHubClient.CloseAsync();
            Console.WriteLine("Mensajes enviados.");
        }
        
        // Simulacion envio de mensajes
        private async Task SendMessagesToEventHub(List<string> DummyData)
        {
            Random rnd = new Random();
            //Cargar DummyData
            foreach (var item in DummyData)
            {
                try
                {
                    SmartButton sm = new SmartButton
                    {
                        DeviceId = item,
                        Status = rnd.Next(0, 2),
                        Data = "0000",
                        Latitude = rnd.NextDouble() * 100 + (-10),
                        Longitude = rnd.NextDouble() * 100 + (-20)
                    };

                    Console.WriteLine($"Boton {sm.DeviceId} presionado");
                    string sm_json = JsonConvert.SerializeObject(sm, Formatting.Indented);
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(sm_json)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }
                mensajesEnviados++;
            }
            Console.WriteLine($"{mensajesEnviados} mensajes enviados.");
        }
    }
}
