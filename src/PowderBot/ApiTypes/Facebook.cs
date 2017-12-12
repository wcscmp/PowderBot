using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PowderBot.ApiTypes.Facebook
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string Id { get; set; }
    }

    [DataContract]
    public class MessageBase
    {
        [DataMember]
        public User Sender { get; set; }
        [DataMember]
        public User Recipient { get; set; }
    }

    [DataContract]
    public class AdditionalData
    {
        [DataMember]
        public string Payload { get; set; }
    }

    [DataContract]
    public class TextData
    {
        [DataMember]
        public string Text { get; set; }
    }

    [DataContract]
    public class QuickReplyBody
    {
        [DataMember]
        public string ContentType => "text";
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Payload { get; set; }
    }

    [DataContract]
    public class QuickReply : TextData
    {
        [DataMember]
        public QuickReplyBody[] QuickReplies { get; set; }
    }

    [DataContract]
    public class MessageBody : TextData
    {
        [DataMember]
        public string Mid { get; set; }
        [DataMember(Name = "quick_reply")]
        public AdditionalData QuickReply { get; set; }
        [DataMember]
        public long? Seq { get; set; }
        [DataMember]
        public object[] Attachments { get; set; }
    }

    [DataContract]
    public class MessagePostback
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Payload { get; set; }
        [DataMember]
        public object Referal { get; set; }
    }

    [DataContract]
    public class MessageData : MessageBase
    {
        [DataMember]
        public long Timestamp { get; set; }
        [DataMember]
        public MessageBody Message { get; set; }
        [DataMember]
        public MessagePostback Postback { get; set; }
    }

    [DataContract]
    public class Entry
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public long Time { get; set; }
        [DataMember]
        public MessageData[] Messaging { get; set; }
    }

    [DataContract]
    public class Event
    {
        [DataMember]
        public string Object { get; set; }
        [DataMember]
        public Entry[] Entry { get; set; }
    }

    [DataContract]
    public class MessageResponse<T>
    {
        [DataMember]
        public User Recipient { get; set; }
        [DataMember]
        public T Message { get; set; }
    }
}
