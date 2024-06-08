using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService_Data
{
    public class HealthCheck : TableEntity
    {
        public string ServiceName { get; set; }
        public string isHealthy { get; set; }
        public DateTime TimeOfCheck { get; set; }

        public HealthCheck()
        {
            PartitionKey = "HealthCheck";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
