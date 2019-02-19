using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RecieveEPHClient.Model
{
    class ProjectComponent
    {
        public static string GenerateTwitterAuthHeader(string ResourceUrl, string TweetType = "TEXT")
        {

            var timeStamp = Math.Truncate((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();

            //string TimeInSecondsSince1970 = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();

            string nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));

            //***

            string resource_url = ResourceUrl;

            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}"; //&status={6}

            string oauth_consumer_key = Properties.Resources.TwConsumerKey;
            string oauth_consumer_secret = Properties.Resources.TwConsumerSecret;
            string oauth_nonce = nonce;
            string oauth_signature_method = "HMAC-SHA1";
            string oauth_timestamp = timeStamp;
            string oauth_token = Properties.Resources.TwToken;
            string oauth_token_secret = Properties.Resources.TwTokenSecret;
            string oauth_version = "1.0";
            //string status = "";

            //IMAGE Because the method uses multipart POST, OAuth is handled differently.

            //POST or query string parameters are not used when calculating an OAuth signature basestring or signature. Only the oauth_* parameters are used.

            var baseString = string.Format(baseFormat,
             oauth_consumer_key,
             oauth_nonce,
             oauth_signature_method,
             oauth_timestamp,
             oauth_token,
             oauth_version
            );
            switch (TweetType)
            {
                case "MEDIA":

                    baseString = string.Concat(baseString); //"POST&", Uri.EscapeDataString(resource_url));

                    break;

                default:

                    baseString = string.Concat("POST&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));
                    break;
            }

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret), "&", Uri.EscapeDataString(oauth_token_secret));

            //Generar Signature

            string oauth_signature;

            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))

            {
                oauth_signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            var headerFormat = "OAuth oauth_nonce=\"{0}\",oauth_signature_method=\"{1}\"," +

             "oauth_timestamp=\"{2}\",oauth_consumer_key=\"{3}\"," +

             "oauth_token=\"{4}\",oauth_signature=\"{5}\"," +

             "oauth_version=\"{6}\"";



            var authHeader = string.Format(headerFormat,

             Uri.EscapeDataString(oauth_nonce),

             Uri.EscapeDataString(oauth_signature_method),

             Uri.EscapeDataString(oauth_timestamp),

             Uri.EscapeDataString(oauth_consumer_key),

             Uri.EscapeDataString(oauth_token),

             Uri.EscapeDataString(oauth_signature),

             Uri.EscapeDataString(oauth_version)

            );
            return authHeader;

        }

        public static string ParseJson(string SocialId, string MsgText) {

            RespTarget RespTarget = new RespTarget(SocialId);
            RespMessageData RespMessageData = new RespMessageData(MsgText);
            RespMessageCreate MessageCreate = new RespMessageCreate(RespTarget, RespMessageData);
            RespEvent RespEvent = new RespEvent("message_create", MessageCreate);
            RespMessage ResponseMessage = new RespMessage(RespEvent);
            var MessageContent = JsonConvert.SerializeObject(ResponseMessage);
            return MessageContent;
            //Answers.logger.Debug("MessageContent for Twitter : " + MessageContent);

        }
    }
}
