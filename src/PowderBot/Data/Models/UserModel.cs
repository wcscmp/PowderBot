using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Data.Models
{
    public class UserModel : TableEntity
    {
        private const string DefaultPartition = "";

        public UserModel(string id)
        {
            PartitionKey = DefaultPartition;
            RowKey = id;

            Id = id;
        }

        public UserModel()
        {
        }

        public bool CanBeNotified(DateTimeOffset now)
        {
            const int notifyAfter = 9;
            const int notifyBefore = 22;
            var userTime = now.AddHours(Gmt).Hour;
            return notifyAfter <= userTime && userTime < notifyBefore;
        }

        public string Id { get; set; }
        public int NotifyAfter { get; set; }
        public int NotifyBefore { get; set; }
        public double Gmt { get; set; }
        public string LastCommand { get; set; }
    }
}
