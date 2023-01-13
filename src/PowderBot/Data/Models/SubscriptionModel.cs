using Microsoft.WindowsAzure.Storage.Table;

namespace Data.Models
{
    public class SubscriptionModel : TableEntity
    {
        private const string DefaultPartition = "";

        public SubscriptionModel()
        {
        }

        public SubscriptionModel(string userId, string uri)
        {
            Uri = uri;
            UserId = userId;
            PartitionKey = DefaultPartition;
            RowKey = userId + GetResortName();
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
                   .First();
        }

        public bool UpdatedToday(UserModel user, DateTimeOffset now)
        {
            var updateTimeInUsersTimeZone = Timestamp.AddHours(user.Gmt);
            return updateTimeInUsersTimeZone.Date == now.Date;
        }

        public string UserId { get; set; }
        public string Uri { get; set; }
        public int Snowfall { get; set; }
    }
}
