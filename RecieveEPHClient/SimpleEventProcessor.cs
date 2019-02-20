using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        public string direct_messages_uri = "https://api.twitter.com/1.1/direct_messages/events/new.json";

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

                        //Enviar Notificacion por Twitter

                        //[{"Id":"1084864679757926400","Username":"JoseFlo07943435"},{"Id":"17747093","Username":"rt10runner"}, {"Id":"3392698503", "Username":"ksantacr_"}]
                        try
                        {
                            List<TwitterAccount> accounts = JsonConvert.DeserializeObject<List<TwitterAccount>>(device.TwitterAccount);

                            foreach (TwitterAccount account in accounts)
                            {
                                SendNotification(account.Id, $"{device.Alias}: {device.Message} \n@{account.Username}");
                            }
                        }
                        catch {
                            Console.WriteLine($"No se han definido usuarios en el device: {device.DeviceId}");
                        }

                    }
                }

            }
            return context.CheckpointAsync();
        }

        /// <summary>
        /// Send notification twitter's via
        /// </summary>
        /// <param name="userId">Recipient's id</param>
        /// <param name="Message">Message body</param>
        /// <returns></returns>
        private async Task<bool> SendNotification(string userId, string Message)
        {
            client = new HttpClient();

            string AuthHeader = ProjectComponent.GenerateTwitterAuthHeader(direct_messages_uri); client.DefaultRequestHeaders.Add("Authorization", AuthHeader); client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string json = ProjectComponent.ParseJson(userId, Message);
            var response = client.PostAsync(direct_messages_uri, new StringContent(json, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
            
            Console.WriteLine($"It was send: {response.IsSuccessStatusCode}, userId: {userId}, message: {Message}");

            return response.IsSuccessStatusCode;
        }

        private UserDevices getDevice(string Id)
        {
            db = new SmartButtonContext();
            var button = db.UserDevices.Where(device => device.DeviceId == Id && device.Status == "CONFIGURED").FirstOrDefault();
            db.Dispose();
            return button;
        }

        private async Task<string> SendReplyResponse(string Text, string ReplyToStatusId, string ReplyToUserId)
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

        private async Task<bool> CheckFriendship(string BotScreenName, string RecipientUsername)
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

    }
}
