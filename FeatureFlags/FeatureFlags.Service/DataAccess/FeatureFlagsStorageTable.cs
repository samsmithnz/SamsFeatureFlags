using FeatureFlags.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureFlags.Service.DataAccess
{
    public class FeatureFlagsStorageTable : IFeatureFlagsStorageTable
    {
        private readonly IConfiguration? _configuration;

        public FeatureFlagsStorageTable(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        private CloudTable CreateConnection()
        {
            string name = "samsappdataeustorage";
            string accessKey = "";
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                    new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(name, accessKey), true);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Get a reference to a table named "FeatureFlags"
            CloudTable featureFlagsTable = tableClient.GetTableReference("FeatureFlags");

            return featureFlagsTable;
        }

        public async Task<IEnumerable<FeatureFlag>> GetFeatureFlags()
        {
            CloudTable featureFlagsTable = CreateConnection();

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<FeatureFlag> query = new TableQuery<FeatureFlag>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "FeatureFlag"));

            // Print the fields for each customer.
            TableQuerySegment<FeatureFlag> resultSegment = await featureFlagsTable.ExecuteQuerySegmentedAsync(query, null);

            //Convert the array into a list and sort by Name
            List<FeatureFlag> results = resultSegment.Results.ToList<FeatureFlag>();
            results.Sort((x, y) => x.Name.CompareTo(y.Name));

            return results;
        }

        public async Task<FeatureFlag> GetFeatureFlag(string name)
        {
            CloudTable featureFlagsTable = CreateConnection();

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<FeatureFlag>("FeatureFlag", name);

            // Execute the retrieve operation.
            TableResult retrievedResult = await featureFlagsTable.ExecuteAsync(retrieveOperation);

            return (FeatureFlag)retrievedResult.Result;
        }

        public async Task<bool> CheckFeatureFlag(string name, string environment)
        {
            CloudTable featureFlagsTable = CreateConnection();

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<FeatureFlag>("FeatureFlag", name);

            // Execute the retrieve operation.
            TableResult retrievedResult = await featureFlagsTable.ExecuteAsync(retrieveOperation);

            FeatureFlag featureFlag = (FeatureFlag)retrievedResult.Result;

            bool result;
            switch (environment.ToLower())
            {
                case "pr":
                    result = featureFlag.PRIsEnabled;
                    featureFlag.PRViewCount++;
                    break;
                case "dev":
                    result = featureFlag.DevIsEnabled;
                    featureFlag.DevViewCount++;
                    break;
                case "qa":
                    result = featureFlag.QAIsEnabled;
                    featureFlag.QAViewCount++;
                    break;
                case "prod":
                    result = featureFlag.ProdIsEnabled;
                    featureFlag.ProdViewCount++;
                    break;
                default:
                    throw new Exception("Unknown environment: " + environment + " for feature flag " + name);
            }

            await SaveFeatureFlag(featureFlag);

            return result;
        }

        public async Task<bool> SaveFeatureFlag(FeatureFlag featureFlag)
        {
            CloudTable featureFlagsTable = CreateConnection();

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.InsertOrMerge(featureFlag);

            // Execute the insert operation.
            await featureFlagsTable.ExecuteAsync(insertOperation);
            return true;
        }

        public async Task<bool> DeleteFeatureFlag(string name)
        {
            CloudTable featureFlagsTable = CreateConnection();

            // Create a retrieve operation that expects a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<FeatureFlag>("FeatureFlag", name);

            // Execute the operation.
            TableResult retrievedResult = await featureFlagsTable.ExecuteAsync(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            FeatureFlag deleteEntity = (FeatureFlag)retrievedResult.Result;

            if (deleteEntity != null)
            {
                // Create the TableOperation that inserts the customer entity.
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                // Execute the delete operation.
                await featureFlagsTable.ExecuteAsync(deleteOperation);
            }
            return true;
        }


    }
}
