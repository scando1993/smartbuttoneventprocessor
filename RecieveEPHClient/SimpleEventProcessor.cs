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

namespace RecieveEPHClient
{
    class SimpleEventProcessor : IEventProcessor
    {
        private static HttpClient client = new HttpClient();
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
                if(sm != null) {
                    if(checkID(sm.DeviceId)) {
                        //Verificar que configuracion tiene
                        //Enviar mensaje
                        sendMessage(sm);
                        //Llamara webhook
                        //callWebhook("www.google.com", sm);
                    }
                }
                Console.WriteLine($"Processing. Partition: {context.PartitionId} {sm.Data} with status {sm.Status}, Id {sm.DeviceId}, lat: {sm.Latitude}, longitude: {sm.Longitude}");
            }
            return context.CheckpointAsync();
        }
        private bool checkID(string DeviceId) {
            if (DeviceId == "42110D") return true;
            return false;
        }
        private void callWebhook(string url, SmartButton sm) {
            
            var stringContent = new StringContent(JsonConvert.SerializeObject(sm), Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, stringContent);

        }

        private void sendMessage(SmartButton sm) {

            // Find your Account Sid and Token at twilio.com/console
            const string accountSid = "ACf7f52d59e73761f55180118ff12194e8";
            const string authToken = "a9ac53163435a413070eaefdd318e55b";

            TwilioClient.Init(accountSid, authToken);
            string message = "Alerta boton presionado : " + sm.DeviceId;
            var msg1 = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber("+13138256642"),
                to: new Twilio.Types.PhoneNumber("+593981893287") // kevin
            );

            var msg2 = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber("+13138256642"),
                to: new Twilio.Types.PhoneNumber("+593939379562") // axel
            );

            Console.WriteLine("Message send to: +593981893287" + msg1.Sid);
            Console.WriteLine("Message send to: +593939379562" + msg2.Sid);

        }
    }
}
