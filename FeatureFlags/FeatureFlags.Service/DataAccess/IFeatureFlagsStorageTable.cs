using FeatureFlags.Models;
using System.Collections.Generic;

namespace FeatureFlags.Service.DataAccess
{
    public interface IFeatureFlagsStorageTable
    {
        IEnumerable<FeatureFlag> GetFeatureFlags();
        FeatureFlag GetFeatureFlag(string name);
        bool CheckFeatureFlag(string name, string environment);
        bool SaveFeatureFlag(FeatureFlag featureFlag);
        bool DeleteFeatureFlag(string name);
    }
}