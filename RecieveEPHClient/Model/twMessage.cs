using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RecieveEPHClient.Model
{
    public enum TwMessageType
    {
        tweet_mention,
        direct_message
    }
    public enum TwContentType
    {
        text,
        photo,
        animated_gif,
        video
    }
    public partial class TwitterMessage
    {
        [JsonProperty("for_user_id")]
        public string ForUserId { get; set; }

        [JsonProperty("tweet_create_events")] //Mención user
        public List<TweetCreateEvent> TweetCreateEvents { get; set; }

        [JsonProperty("direct_message_events")] //Mensajes Directos
        public List<DirectMessageEvent> DirectMessageEvents { get; set; }

        [JsonProperty("users")] //Mensajes Directos
        public Dictionary<string, User> Users { get; set; }

        //Manual
        public TwContentType TwContentType { get; set; }
        public TwMessageType TwMessageType { get; set; }
        public bool IsMsgTypeDefined { get; set; }

        //MANUAL  
        public void findTwMsgType()
        {
            if (DirectMessageEvents != null)
            {

                foreach (var Item in DirectMessageEvents)
                {
                    TwMessageType = TwMessageType.direct_message;
                    IsMsgTypeDefined = false;

                    if (Item.MessageCreate.MessageData.Attachment != null)
                    {
                        if (Item.MessageCreate.MessageData.Attachment.Type == "media")
                        {
                            switch (Item.MessageCreate.MessageData.Attachment.Media.Type)
                            {
                                case "photo":
                                    TwContentType = TwContentType.photo;
                                    IsMsgTypeDefined = true;
                                    break;
                                case "video":
                                    TwContentType = TwContentType.video;
                                    IsMsgTypeDefined = true;
                                    break;
                                case "animated_gif":
                                    TwContentType = TwContentType.animated_gif;
                                    IsMsgTypeDefined = true;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        TwContentType = TwContentType.text;
                        IsMsgTypeDefined = true;
                    }

                }
            }
            if (TweetCreateEvents != null)
            {

            }
        }

    }

    public partial class TweetCreateEvent
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("id")]
        public double Id { get; set; }

        [JsonProperty("id_str")]
        public string IdStr { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("truncated")]
        public bool Truncated { get; set; }

        [JsonProperty("in_reply_to_status_id")]
        public object InReplyToStatusId { get; set; }

        [JsonProperty("in_reply_to_status_id_str")]
        public object InReplyToStatusIdStr { get; set; }

        [JsonProperty("in_reply_to_user_id", NullValueHandling = NullValueHandling.Ignore)]
        public double InReplyToUserId { get; set; }

        [JsonProperty("in_reply_to_user_id_str", NullValueHandling = NullValueHandling.Ignore)]
        public string InReplyToUserIdStr { get; set; }

        [JsonProperty("in_reply_to_screen_name", NullValueHandling = NullValueHandling.Ignore)]
        public string InReplyToScreenName { get; set; }

        [JsonProperty("user")]
        public TwUser User { get; set; }

        [JsonProperty("geo")]
        public object Geo { get; set; }

        [JsonProperty("coordinates")]
        public object Coordinates { get; set; }

        [JsonProperty("place")]
        public object Place { get; set; }

        [JsonProperty("contributors")]
        public object Contributors { get; set; }

        [JsonProperty("is_quote_status")]
        public bool IsQuoteStatus { get; set; }

        [JsonProperty("quote_count")]
        public long QuoteCount { get; set; }

        [JsonProperty("reply_count")]
        public long ReplyCount { get; set; }

        [JsonProperty("retweet_count")]
        public long RetweetCount { get; set; }

        [JsonProperty("favorite_count")]
        public long FavoriteCount { get; set; }

        [JsonProperty("entities")]
        public Entities Entities { get; set; }

        [JsonProperty("favorited")]
        public bool Favorited { get; set; }

        [JsonProperty("retweeted")]
        public bool Retweeted { get; set; }

        [JsonProperty("filter_level")]
        public string FilterLevel { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("timestamp_ms")]
        public string TimestampMs { get; set; }
    }

    public partial class DirectMessageEvent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_timestamp")]
        public string CreatedTimestamp { get; set; }

        [JsonProperty("initiated_via")]
        public InitiatedVia InitiatedVia { get; set; }

        [JsonProperty("message_create")]
        public MessageCreate MessageCreate { get; set; }
    }

    public partial class InitiatedVia
    {
        [JsonProperty("tweet_id")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long TweetId { get; set; }

        [JsonProperty("welcome_message_id")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long WelcomeMessageId { get; set; }
    }

    public partial class MessageCreate
    {
        [JsonProperty("target")]
        public Target Target { get; set; }

        [JsonProperty("sender_id")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long SenderId { get; set; }

        [JsonProperty("source_app_id")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long SourceAppId { get; set; }

        [JsonProperty("message_data")]
        public MessageData MessageData { get; set; }
    }

    public partial class MessageData
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("entities")]
        public Entities Entities { get; set; }

        [JsonProperty("quick_reply_response")]
        public QuickReplyResponse QuickReplyResponse { get; set; }

        [JsonProperty("attachment")]
        public TwAttachment Attachment { get; set; }
    }

    public partial class TwAttachment
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("media")]
        public Media Media { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("id")]
        public double Id { get; set; }

        [JsonProperty("id_str")]
        public string IdStr { get; set; }

        [JsonProperty("indices")]
        public List<long> Indices { get; set; }

        [JsonProperty("media_url")]
        public Uri MediaUrl { get; set; }

        [JsonProperty("media_url_https")]
        public Uri MediaUrlHttps { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; }

        [JsonProperty("expanded_url")]
        public Uri ExpandedUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("source_status_id", NullValueHandling = NullValueHandling.Ignore)]
        public double? SourceStatusId { get; set; }

        [JsonProperty("source_status_id_str", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceStatusIdStr { get; set; }

        [JsonProperty("sizes")]
        public Sizes Sizes { get; set; }

        [JsonProperty("video_info", NullValueHandling = NullValueHandling.Ignore)]
        public VideoInfo VideoInfo { get; set; }
    }

    public partial class Sizes
    {
        [JsonProperty("thumb")]
        public Large Thumb { get; set; }

        [JsonProperty("small")]
        public Large Small { get; set; }

        [JsonProperty("large")]
        public Large Large { get; set; }

        [JsonProperty("medium")]
        public Large Medium { get; set; }
    }

    public partial class Large
    {
        [JsonProperty("w")]
        public long W { get; set; }

        [JsonProperty("h")]
        public long H { get; set; }

        [JsonProperty("resize")]
        public string Resize { get; set; }
    }

    public partial class VideoInfo
    {
        [JsonProperty("aspect_ratio")]
        public List<long> AspectRatio { get; set; }

        [JsonProperty("variants")]
        public List<Variant> Variants { get; set; }
    }

    public partial class Variant
    {
        [JsonProperty("bitrate")]
        public long Bitrate { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class Entities
    {
        [JsonProperty("hashtags")]
        public List<Hashtag> Hashtags { get; set; }

        [JsonProperty("symbols")]
        public List<Hashtag> Symbols { get; set; }

        [JsonProperty("user_mentions")]
        public List<UserMention> UserMentions { get; set; }

        [JsonProperty("media", NullValueHandling = NullValueHandling.Ignore)]
        public List<Media> Media { get; set; }

        [JsonProperty("urls")]
        public List<Url> Urls { get; set; }
    }

    public partial class Hashtag
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("indices")]
        public List<long> Indices { get; set; }
    }

    public partial class Url
    {
        [JsonProperty("url")]
        public Uri UrlUrl { get; set; }

        [JsonProperty("expanded_url")]
        public Uri ExpandedUrl { get; set; }

        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; }

        [JsonProperty("indices")]
        public List<long> Indices { get; set; }
    }

    public partial class UserMention
    {
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("id_str")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long IdStr { get; set; }

        [JsonProperty("indices")]
        public List<long> Indices { get; set; }
    }

    public partial class QuickReplyResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("metadata")]
        public string Metadata { get; set; }
    }

    public partial class Target
    {
        [JsonProperty("recipient_id")]
        public string RecipientId { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_timestamp")]
        public string CreatedTimestamp { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("protected")]
        public bool Protected { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("followers_count")]
        public long FollowersCount { get; set; }

        [JsonProperty("friends_count")]
        public long FriendsCount { get; set; }

        [JsonProperty("statuses_count")]
        public long StatusesCount { get; set; }

        [JsonProperty("profile_image_url")]
        public Uri ProfileImageUrl { get; set; }

        [JsonProperty("profile_image_url_https")]
        public Uri ProfileImageUrlHttps { get; set; }
    }

    public partial class TwUser
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("id_str")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long IdStr { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("url")]
        public object Url { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("translator_type")]
        public string TranslatorType { get; set; }

        [JsonProperty("protected")]
        public bool Protected { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("followers_count")]
        public long FollowersCount { get; set; }

        [JsonProperty("friends_count")]
        public long FriendsCount { get; set; }

        [JsonProperty("listed_count")]
        public long ListedCount { get; set; }

        [JsonProperty("favourites_count")]
        public long FavouritesCount { get; set; }

        [JsonProperty("statuses_count")]
        public long StatusesCount { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("utc_offset")]
        public object UtcOffset { get; set; }

        [JsonProperty("time_zone")]
        public object TimeZone { get; set; }

        [JsonProperty("geo_enabled")]
        public bool GeoEnabled { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("contributors_enabled")]
        public bool ContributorsEnabled { get; set; }

        [JsonProperty("is_translator")]
        public bool IsTranslator { get; set; }

        [JsonProperty("profile_background_color")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string ProfileBackgroundColor { get; set; }

        [JsonProperty("profile_background_image_url")]
        public Uri ProfileBackgroundImageUrl { get; set; }

        [JsonProperty("profile_background_image_url_https")]
        public Uri ProfileBackgroundImageUrlHttps { get; set; }

        [JsonProperty("profile_background_tile")]
        public bool ProfileBackgroundTile { get; set; }

        [JsonProperty("profile_link_color")]
        public string ProfileLinkColor { get; set; }

        [JsonProperty("profile_sidebar_border_color")]
        public string ProfileSidebarBorderColor { get; set; }

        [JsonProperty("profile_sidebar_fill_color")]
        public string ProfileSidebarFillColor { get; set; }

        [JsonProperty("profile_text_color")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long ProfileTextColor { get; set; }

        [JsonProperty("profile_use_background_image")]
        public bool ProfileUseBackgroundImage { get; set; }

        [JsonProperty("profile_image_url")]
        public Uri ProfileImageUrl { get; set; }

        [JsonProperty("profile_image_url_https")]
        public Uri ProfileImageUrlHttps { get; set; }

        [JsonProperty("profile_banner_url")]
        public Uri ProfileBannerUrl { get; set; }

        [JsonProperty("default_profile")]
        public bool DefaultProfile { get; set; }

        [JsonProperty("default_profile_image")]
        public bool DefaultProfileImage { get; set; }

        [JsonProperty("following")]
        public object Following { get; set; }

        [JsonProperty("follow_request_sent")]
        public object FollowRequestSent { get; set; }

        [JsonProperty("notifications")]
        public object Notifications { get; set; }
    }

    public partial class TwitterMessage
    {
        public static TwitterMessage FromJson(string json) => JsonConvert.DeserializeObject<TwitterMessage>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this TwitterMessage self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    //internal class ParseStringConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        long l;
    //        if (Int64.TryParse(value, out l))
    //        {
    //            return l;
    //        }
    //        throw new Exception("Cannot unmarshal type long");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (long)untypedValue;
    //        serializer.Serialize(writer, value.ToString());
    //        return;
    //    }

    //    public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    //}

    /// <summary>
    /// Respuesta de Twitter
    /// </summary>

    public partial class RespMessage
    {
        [JsonProperty("event")]
        public RespEvent Event { get; set; }

        public RespMessage(RespEvent Event)
        {
            this.Event = Event;
        }
    }

    public partial class RespEvent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("message_create")]
        public RespMessageCreate MessageCreate { get; set; }

        public RespEvent(string Type, RespMessageCreate MessageCreate)
        {
            this.Type = "message_create";
            this.MessageCreate = MessageCreate;
        }
    }

    public partial class RespMessageCreate
    {
        [JsonProperty("target")]
        public RespTarget Target { get; set; }

        [JsonProperty("message_data")]
        public RespMessageData MessageData { get; set; }

        public RespMessageCreate(RespTarget Target, RespMessageData RespMessageData)
        {
            this.Target = Target;
            this.MessageData = RespMessageData;

        }
    }

    public partial class RespMessageData
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("quick_reply", NullValueHandling = NullValueHandling.Ignore)]
        public RespQuickReply QuickReply { get; set; }

        [JsonProperty("ctas", NullValueHandling = NullValueHandling.Ignore)]
        public List<Cta> Ctas { get; set; }

        [JsonProperty("attachment", NullValueHandling = NullValueHandling.Ignore)]
        public RespAttachment Attachment { get; set; }

        public RespMessageData(string Text)
        {
            this.Text = Text;
        }
        public RespMessageData(string Text, RespQuickReply QuickReply)
        {
            this.Text = Text;
            this.QuickReply = QuickReply;
        }
        public RespMessageData(string Text, List<Cta> Ctas)
        {
            this.Text = Text;
            this.Ctas = Ctas;
        }
        public RespMessageData(string Text, RespQuickReply QuickReply, List<Cta> Ctas)
        {
            this.Text = Text;
            this.QuickReply = QuickReply;
            this.Ctas = Ctas;
        }
        public RespMessageData(string Text, RespAttachment Attachment)
        {
            this.Text = Text;
            this.Attachment = Attachment;
        }
        public RespMessageData(RespAttachment Attachment)
        {
            this.Attachment = Attachment;
        }
    }

    public partial class Cta
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("tco_url")]
        public Uri TcoUrl { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        public Cta(string Type, string Label, Uri Url)
        {
            this.Type = Type;
            this.Label = Label;
            this.Url = Url;
        }
    }

    public partial class RespQuickReply
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("options")]
        public List<RespOption> Options { get; set; }

        public RespQuickReply(string Type, List<RespOption> Options)
        {
            this.Type = Type;
            this.Options = Options;
        }
    }

    public partial class RespOption
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("metadata")]
        public string Metadata { get; set; }

        public RespOption(string Label, string Description, string Metadata)
        {
            this.Label = Label;
            this.Description = Description;
            this.Metadata = Metadata;
        }
    }

    public partial class RespTarget
    {
        [JsonProperty("recipient_id")]
        public string RecipientId { get; set; }

        public RespTarget(string RecipientId)
        {
            this.RecipientId = RecipientId;
        }
    }

    public partial class RespAttachment
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("media")]
        public RespMedia Media { get; set; }
    }
    public partial class RespMedia
    {
        [JsonProperty("id")]
        public double Id { get; set; }
    }
    /// <summary>
    /// Clase para serializar el Json que se recibe en el Content del psMessage.
    /// </summary>

    public class Content_psMessage
    {
        [JsonProperty("message")]
        public ContentMessage Message { get; set; }
    }

    public class ContentMessage
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("quick_replies", NullValueHandling = NullValueHandling.Ignore)]
        public List<ContentQuickReply> QuickReplies { get; set; }

        [JsonProperty("attachment", NullValueHandling = NullValueHandling.Ignore)]
        public ContentAttachment Attachment { get; set; }
    }

    public class ContentQuickReply
    {
        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }
    }

    public class ContentAttachment
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("payload")]
        public AttachmentPayload Payload { get; set; }
    }

    public class AttachmentPayload
    {
        [JsonProperty("template_type")]
        public string TemplateType { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("buttons")]
        public List<TemplateButton> Buttons { get; set; }

        [JsonProperty("elements")]
        public List<Element> Elements { get; set; }

    }
    public partial class Element
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("buttons")]
        public List<TemplateButton> Buttons { get; set; }
    }

    public partial class TemplateButton
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
        public string Payload { get; set; }
    }

    /// <summary>
    /// Upload Media Init
    /// </summary>
    public partial class UploadInitResp
    {
        [JsonProperty("media_id")]
        public double MediaId { get; set; }

        [JsonProperty("media_id_string")]
        public string MediaIdString { get; set; }

        [JsonProperty("expires_after_secs")]
        public long ExpiresAfterSecs { get; set; }

        [JsonProperty("media_key")]
        public string MediaKey { get; set; }
    }

    /// <summary>
    /// Check Friendship
    /// </summary>
    public class CheckFriendship
    {
        [JsonProperty("relationship")]
        public Relationship Relationship { get; set; }
    }

    public partial class Relationship
    {
        [JsonProperty("source")]
        public FriendshipSource Source { get; set; }

        [JsonProperty("target")]
        public FriendshipTarget Target { get; set; }
    }

    public class FriendshipSource
    {
        [JsonProperty("id")]
        public double Id { get; set; }

        [JsonProperty("id_str")]
        public string IdStr { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("following")]
        public bool Following { get; set; }

        [JsonProperty("followed_by")]
        public bool FollowedBy { get; set; }

        [JsonProperty("live_following")]
        public bool LiveFollowing { get; set; }

        [JsonProperty("following_received")]
        public bool FollowingReceived { get; set; }

        [JsonProperty("following_requested")]
        public bool FollowingRequested { get; set; }

        [JsonProperty("notifications_enabled")]
        public bool NotificationsEnabled { get; set; }

        [JsonProperty("can_dm")]
        public bool CanDm { get; set; }

        [JsonProperty("blocking")]
        public bool Blocking { get; set; }

        [JsonProperty("blocked_by")]
        public bool BlockedBy { get; set; }

        [JsonProperty("muting")]
        public bool Muting { get; set; }

        [JsonProperty("want_retweets")]
        public bool WantRetweets { get; set; }

        [JsonProperty("all_replies")]
        public bool AllReplies { get; set; }

        [JsonProperty("marked_spam")]
        public bool MarkedSpam { get; set; }
    }

    public class FriendshipTarget
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("id_str")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long IdStr { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("following")]
        public bool Following { get; set; }

        [JsonProperty("followed_by")]
        public bool FollowedBy { get; set; }

        [JsonProperty("following_received")]
        public bool FollowingReceived { get; set; }

        [JsonProperty("following_requested")]
        public bool FollowingRequested { get; set; }
    }
}
