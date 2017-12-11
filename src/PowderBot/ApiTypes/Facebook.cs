using System;
using System.Collections.Generic;

namespace PowderBot.ApiTypes.Facebook
{
    public class User
    {
        public string Id { get; set; }
    }

    public class MessageBase
    {
        public User Sender { get; set; }
        public User Recipient { get; set; }
    }

    public class AdditionalData
    {
        public string Payload { get; set; }
    }

    public class TextData
{
        public string Text { get; set; }
    }

    public class ListButton
    {
        public string Type => "postback";
        public string Title { get; set; }
        public string Payload { get; set; }
    }

    public class ListPayload : TextData
    {
        public string TemplateType => "button";
        public ListButton[] Buttons { get; set; }
    }

    public class ListAttachment
    {
        public string Type => "template";
        public ListPayload Payload { get; set; }
    }

    public class ListData
    {
        public ListAttachment Attachment { get; set; }
    }

    public class MessageBody : TextData
    {
        public string Mid { get; set; }

        public AdditionalData QuickReply { get; set; }
        public long? Seq { get; set; }
        public List<object> Attachments { get; set; }
    }

    public class MessagePostback
    {
        public string Title { get; set; }
        public string Payload { get; set; }
        public object Referal { get; set; }
    }

    public class MessageData : MessageBase
    {
        public long Timestamp { get; set; }
        public MessageBody Message { get; set; }
        public MessagePostback Postback { get; set; }
    }

    public class Entry
    {
        public string Id { get; set; }
        public long Time { get; set; }
        public List<MessageData> Messaging { get; set; }
    }

    public class Event
    {
        public string Object { get; set; }
        public List<Entry> Entry { get; set; }
    }

    public class MessageResponse<T>
    {
        public User Recipient { get; set; }
        public T Message { get; set; }
    }
}
