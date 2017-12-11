using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Data.Models
{
    public class RequestModel : TableEntity
    {
        private const string DefaultPartition = "";

        public RequestModel(string id)
        {
            PartitionKey = DefaultPartition;
            RowKey = id;

            Id = id;
        }

        public RequestModel()
        {
        }

        public string Id { get; set; }
        public string Request { get; set; }
    }
}
