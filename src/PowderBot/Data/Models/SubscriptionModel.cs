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
            PartitionKey = DefaultPartition;
            RowKey = userId +
                uri.Split(new Char[]{'/'})
                   .SkipWhile(str => str != "resorts")
                   .Skip(1)
                   .First();

            Uri = uri;
            UserId = userId;
        }

        public SubscriptionModel()
        {
        }

        public string UserId { get; set; }
        public string Uri { get; set; }
        public int Snowfall { get; set; }
    }
}
