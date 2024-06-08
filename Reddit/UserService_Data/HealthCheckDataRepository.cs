using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService_Data
{
    public class HealthCheckDataRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;


        public HealthCheckDataRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("healthCheckTable"); _table.CreateIfNotExists();
        }

        public bool Exists(string Id)
        {
            return RetrieveAllChecks().Where(s => s.RowKey == Id).FirstOrDefault() != null;
        }

        public IEnumerable<HealthCheck> RetrieveAllChecks()
        {
            var results = from g in _table.CreateQuery<HealthCheck>()
                          where g.PartitionKey == "HealthCheck"
                          select g;
            return results;
        }

        public void AddCheck(HealthCheck healthCheck)
        {
            TableOperation insertOperation = TableOperation.Insert(healthCheck);
            _table.Execute(insertOperation);
        }


        public async Task<List<HealthCheck>> GetHealthChecksAsync(DateTime since)
        {
            var query = new TableQuery<HealthCheck>().Where(
                TableQuery.GenerateFilterConditionForDate("TimeOfCheck", QueryComparisons.GreaterThanOrEqual, since));

            var segment = await _table.ExecuteQuerySegmentedAsync(query, null);
            return segment.Results;
        }
    }
}
