using Microsoft.WindowsAzure.Storage.Table;

namespace Data.Models
{
    public class SubscriptionModel : TableEntity
    {
        private const string DefaultPartition = "";

        public SubscriptionModel()
        {
        }

        public SubscriptionModel(string chatId, string uri, string userId)
        {
            Uri = uri;
            ChatId = chatId;
            PartitionKey = DefaultPartition;
            RowKey = chatId + GetResortName();
            UserId = userId;
        }

        public static bool IsValidUri(string uri)
        {
            return uri.ToLower().Contains("snow-forecast.com/resorts/");
        }

        public string GetResortName()
        {
            return Uri.Split(new Char[]{'/'})
                   .SkipWhile(str => str != "resorts")
                   .Skip(1)
                   .First()
                   .Replace("-", string.Empty);
        }

        public bool UpdatedToday(UserModel user, DateTimeOffset now)
        {
            var updateTimeInUsersTimeZone = LastMessageSent.AddHours(user.Gmt);
            return updateTimeInUsersTimeZone.Date == now.Date;
        }

        public string ChatId { get; set; }
        public string Uri { get; set; }
        public int Snowfall { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset LastMessageSent { get; set; }
    }
}
