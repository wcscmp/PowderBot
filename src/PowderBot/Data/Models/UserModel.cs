using Microsoft.WindowsAzure.Storage.Table;

namespace Data.Models
{
    public class UserModel : TableEntity
    {
        private const string DefaultPartition = "";

        public UserModel()
        {
        }

        public UserModel(string id)
        {
            PartitionKey = DefaultPartition;
            RowKey = id;
        }

        public bool CanBeNotified(DateTimeOffset now)
        {
            const int notifyAfter = 10;
            const int notifyBefore = 22;
            var userTime = now.AddHours(Gmt).Hour;
            return notifyAfter <= userTime && userTime < notifyBefore;
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public double Gmt { get; set; }
        public string LastCommand { get; set; }

        public string Id => RowKey;
    }
}
