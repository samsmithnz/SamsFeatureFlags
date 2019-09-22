using FeatureFlags.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureFlags.Service.DataAccess
{
    public interface IFeatureFlagsStorageTable
    {
        Task<IEnumerable<FeatureFlag>> GetFeatureFlags();
        Task<FeatureFlag> GetFeatureFlag(string name);
        Task<bool> CheckFeatureFlag(string name, string environment);
        Task<bool> SaveFeatureFlag(FeatureFlag featureFlag);
        Task<bool> DeleteFeatureFlag(string name);

    }
}