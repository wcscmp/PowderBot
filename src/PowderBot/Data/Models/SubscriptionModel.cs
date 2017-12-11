using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;

namespace Data.Models
{
    public class SubscriptionModel : TableEntity
    {
        private const string DefaultPartition = "";

        public SubscriptionModel(string userId, string uri)
        {
            Uri = uri;
            UserId = userId;
            PartitionKey = DefaultPartition;
            RowKey = userId + GetResortName();
        }

        public SubscriptionModel()
        {
        }

        public string GetResortName()
        {
            return Uri.Split(new Char[]{'/'})
                   .SkipWhile(str => str != "resorts")
                   .Skip(1)
                   .First();
        }

        public string UserId { get; set; }
        public string Uri { get; set; }
        public int Snowfall { get; set; }
    }
}
