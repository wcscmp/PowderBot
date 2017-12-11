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
            NotifyAfter = 0;
            NotifyBefore = 24;
        }

        public UserModel()
        {
        }

        public bool CanBeNotified(DateTimeOffset now)
        {
            var userTime = now.AddHours(Gmt).Hour;
            return NotifyAfter <= userTime && userTime < NotifyBefore;
        }

        public string Id { get; set; }
        public int NotifyAfter { get; set; }
        public int NotifyBefore { get; set; }
        public int Gmt { get; set; }
    }
}
