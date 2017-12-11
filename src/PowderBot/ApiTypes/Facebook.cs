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

    public class ListButton : TextData
    {
        public string Type => "postback";
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

    public class TextMessageBody : TextData
    {
        public string Mid { get; set; }

        public AdditionalData QuickReply { get; set; }
        public long? Seq { get; set; }
        public List<object> Attachments { get; set; }
    }

    public class TextMessage : MessageBase
    {
        public long Timestamp { get; set; }
        public TextMessageBody Message { get; set; }
    }

    public class Entry<T>
    {
        public string Id { get; set; }
        public long Time { get; set; }
        public List<T> Messaging { get; set; }
    }

    public class Event<T>
    {
        public string Object { get; set; }
        public List<Entry<T>> Entry { get; set; }
    }

    public class MessageResponse<T>
    {
        public User Recipient { get; set; }
        public T Message { get; set; }
    }
}
