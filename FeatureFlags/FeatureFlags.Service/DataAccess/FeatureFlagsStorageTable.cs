using FeatureFlags.Models;
using Microsoft.Extensions.Configuration;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;

namespace FeatureFlags.Service.DataAccess
{
    public class FeatureFlagsStorageTable : IFeatureFlagsStorageTable
    {
        private readonly IConfiguration? _configuration;

        public FeatureFlagsStorageTable(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        private TableClient CreateConnection()
        {
            TableClient tableClient;
            if (_configuration == null)
            {
                throw new Exception("Configuration details missing");
            }
            string? url = _configuration["AppSettings:TableStorageURL"];
            string? name = _configuration["FeatureFlagsStorageName"];
            string? accessKey = _configuration["FeatureFlagsStorageAccessKey"];
            string? tableName = _configuration["AppSettings:TableName"];
            if (url != null && name != null && accessKey != null && tableName != null)
            {
                TableServiceClient storageAccount = new(
                      new Uri(url),
                      new TableSharedKeyCredential(name, accessKey));

                tableClient = storageAccount.GetTableClient(tableName);
            }
            else
            {
                throw new Exception("Table storage connection details missing");
            }

            return tableClient;
        }

        public IEnumerable<FeatureFlag> GetFeatureFlags()
        {
            TableClient featureFlagsTable = CreateConnection();

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            Pageable<FeatureFlag> queryResults = featureFlagsTable.Query<FeatureFlag>(e => e.PartitionKey == "FeatureFlag");

            //Convert the array into a list and sort by Name
            List<FeatureFlag> results = queryResults.ToList<FeatureFlag>();
            results.Sort((x, y) => x.Name.CompareTo(y.Name));

            return results;
        }

        public FeatureFlag GetFeatureFlag(string name)
        {
            TableClient featureFlagsTable = CreateConnection();

            // Create a retrieve operation that takes a customer entity.
            FeatureFlag queryResult = featureFlagsTable.GetEntity<FeatureFlag>("FeatureFlag", name);

            return queryResult;
        }

        public bool CheckFeatureFlag(string name, string environment)
        {
            FeatureFlag featureFlag = GetFeatureFlag(name);

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

            SaveFeatureFlag(featureFlag);

            return result;
        }

        public bool SaveFeatureFlag(FeatureFlag featureFlag)
        {
            TableClient featureFlagsTable = CreateConnection();
            featureFlagsTable.UpsertEntity(featureFlag);

            return true;
        }

        public bool DeleteFeatureFlag(string name)
        {
            TableClient featureFlagsTable = CreateConnection();
            featureFlagsTable.DeleteEntity("FeatureFlag", name);

            return true;
        }


    }
}
