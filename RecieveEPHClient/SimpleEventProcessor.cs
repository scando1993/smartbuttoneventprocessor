using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Converters;
using System.Net.Http;
using RecieveEPHClient.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RecieveEPHClient
{
    public class SimpleEventProcessor : IEventProcessor
    {
        private static HttpClient client = new HttpClient();
        private static SmartButtonContext db = new SmartButtonContext();
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"SimpleEventProcessor initialized. Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                //Logica de negocio
                string data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                SmartButton sm = JsonConvert.DeserializeObject<SmartButton>(data);
                if (sm != null)
                {
                    var device = getDevice(sm.DeviceId);
                    if (device != null)
                    {
                        //Verificar que configuracion tiene
                        device.Message = (string.IsNullOrWhiteSpace(device.Message)) ? "Boton presionado" : device.Message;
                        device.Alias = (string.IsNullOrWhiteSpace(device.Alias)) ? "" : device.Alias;

                        //Enviar mensaje
                        sendMessage(sm, device);

                        //Llamara webhook
                        if (!string.IsNullOrWhiteSpace(device.Webhook))
                        {
                            MyButton btn = new MyButton
                            {
                                Alias = device.Alias,
                                Message = device.Message,
                                Id = device.Id,
                                State = "pressed",
                                Dtm = DateTime.Now.ToUniversalTime()
                            };
                            callWebhook(device.Webhook, btn);
                            Console.WriteLine($"Url {device.Webhook}");
                        }
                    }
                }
                EventProcessor.events += 1;
                Console.WriteLine($"Procesando. Particion: {context.PartitionId} {sm.Data} .Estado: {sm.Status}, Id {sm.DeviceId}, lat: {sm.Latitude}, longitude: {sm.Longitude}");
            }
            return context.CheckpointAsync();
        }
        private UserDevices getDevice(string Id)
        {
            var button = db.UserDevices.Where(device => device.DeviceId == Id && device.Status == "AVAILABLE").FirstOrDefault();
            return button;
        }
        private void callWebhook(string url, MyButton obj)
        {

            var stringContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, stringContent);

        }

        private void sendMessage(SmartButton sm, UserDevices device)
        {

            // Find your Account Sid and Token at twilio.com/console
            const string accountSid = "ACf7f52d59e73761f55180118ff12194e8";
            const string authToken = "a9ac53163435a413070eaefdd318e55b";

            TwilioClient.Init(accountSid, authToken);
            //string message = "Alerta boton presionado : " + sm.DeviceId;
            var numbers = new List<string>();
            try
            {
                numbers = JsonConvert.DeserializeObject<List<string>>(device.PhoneNumber);
            }
            catch (Exception e)
            {
                Console.WriteLine($"No se han definido numeros de contacto para {device.DeviceId}");
            }

            foreach (var n in numbers)
            {
                var msg = MessageResource.Create(
                body: device.Message,
                from: new Twilio.Types.PhoneNumber("+13138256642"),
                to: new Twilio.Types.PhoneNumber(n)
                );
                Console.WriteLine($"Mensaje enviado a: {n}. Cod: {msg.Sid}. Dispositivo {device.DeviceId}");
            }

        }
    }
}
