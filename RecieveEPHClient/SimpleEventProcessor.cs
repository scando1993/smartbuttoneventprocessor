﻿using System;
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
using System.Net.Http.Headers;

namespace RecieveEPHClient
{
    public class SimpleEventProcessor : IEventProcessor
    {
        private HttpClient client;
        private SmartButtonContext db;
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

                Console.WriteLine(data);


                SendNotification();

                //@JoseFlo07943435

                //SmartButton sm = JsonConvert.DeserializeObject<SmartButton>(data);
                //if (sm != null)
                //{
                //    var device = getDevice(sm.DeviceId);
                //    if (device != null)
                //    {
                //        //Verificar que configuracion tiene
                //        device.Message = (string.IsNullOrWhiteSpace(device.Message)) ? "Boton presionado" : device.Message;
                //        device.Alias = (string.IsNullOrWhiteSpace(device.Alias)) ? "" : device.Alias;

                //        //Enviar mensaje
                //        sendMessage(sm, device);

                //        //Llamara webhook
                //        if (!string.IsNullOrWhiteSpace(device.Webhook))
                //        {
                //            MyButton btn = new MyButton
                //            {
                //                Alias = device.Alias,
                //                Message = device.Message,
                //                Id = device.Id,
                //                State = "pressed",
                //                Dtm = DateTime.Now.ToUniversalTime()
                //            };
                //            callWebhook(device.Webhook, btn);
                //            Console.WriteLine($"Url {device.Webhook}");
                //        }
                //    }
                //}
                //EventProcessor.events += 1;
                //Console.WriteLine($"Procesando. Particion: {context.PartitionId} {sm.Data} .Estado: {sm.Status}, Id {sm.DeviceId}, lat: {sm.Latitude}, longitude: {sm.Longitude}");
            }
            return context.CheckpointAsync();
        }

        private void SendNotification()
        {
            //string username = "@JoseFlo07943435";
            client = new HttpClient();
            string userIdRC = "17747093";
            string userIdKS = "3392698503";
            string userIdJF = "1084864679757926401";

            string direct_messages_uri = "https://api.twitter.com/1.1/direct_messages/events/new.json";

            string AuthHeader = ProjectComponent.GenerateTwitterAuthHeader(direct_messages_uri); client.DefaultRequestHeaders.Add("Authorization", AuthHeader); client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string json = ProjectComponent.ParseJson(userIdRC, "hola eventhub");
            var response = client.PostAsync(direct_messages_uri, new StringContent(json, Encoding.UTF8, "application/json"));

            Console.WriteLine(response.Status);
        }


        public async Task<string> SendReplyResponse(string Text, string ReplyToStatusId, string ReplyToUserId)
        {
            OAuthInfo OAI = new OAuthInfo()
            {
                ConsumerKey = Properties.Resources.TwConsumerKey,
                ConsumerSecret = Properties.Resources.TwConsumerSecret,
                AccessToken = Properties.Resources.TwToken,
                AccessSecret = Properties.Resources.TwTokenSecret
            };

            TinyTwitter TT = new TinyTwitter(OAI);
            TT.ReplyStatus(Text, ReplyToStatusId, ReplyToUserId);
            return "OK";
        }

        public async Task<bool> CheckFriendship(string BotScreenName, string RecipientUsername)
        {
            bool IsFriend = false;
            OAuthInfo OAI = new OAuthInfo()
            {
                ConsumerKey = Properties.Resources.TwConsumerKey,
                ConsumerSecret = Properties.Resources.TwConsumerSecret,
                AccessToken = Properties.Resources.TwToken,
                AccessSecret = Properties.Resources.TwTokenSecret
            };

            TinyTwitter TT = new TinyTwitter(OAI);

            string CheckFriendJson = TT.CheckFriendship(BotScreenName, RecipientUsername);

            CheckFriendship CF = (CheckFriendship)Newtonsoft.Json.JsonConvert.DeserializeObject(CheckFriendJson, typeof(CheckFriendship));

            if (CF != null)

            {
                IsFriend = CF.Relationship.Source.Following;

            }
            return IsFriend;
        }

        private async Task<string> FollowUserId(string UserId)
        {
            bool IsFriend = false;
            OAuthInfo OAI = new OAuthInfo()
            {
                ConsumerKey = Properties.Resources.TwConsumerKey,
                ConsumerSecret = Properties.Resources.TwConsumerSecret,
                AccessToken = Properties.Resources.TwToken,
                AccessSecret = Properties.Resources.TwTokenSecret
            };
            TinyTwitter TT = new TinyTwitter(OAI);
            TT.FollowUserId(UserId);
            return "OK";

        }

        private UserDevices getDevice(string Id)
        {
            db = new SmartButtonContext();
            var button = db.UserDevices.Where(device => device.DeviceId == Id && device.Status == "CONFIGURED").FirstOrDefault();
            db.Dispose();
            return button;
        }

        private void callWebhook(string url, MyButton obj)
        {
            client = new HttpClient();
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
                    body: $"{device.Alias}:  {device.Message}",
                    from: new Twilio.Types.PhoneNumber("+13138256642"),
                    to: new Twilio.Types.PhoneNumber(n)
                );
                Console.WriteLine($"Mensaje {device.Message} para: {n}. Cod: {msg.Sid}. Dispositivo {device.DeviceId} con alias: {device.Alias}");
            }

        }
    }
}
