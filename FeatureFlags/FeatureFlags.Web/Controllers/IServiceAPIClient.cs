using FeatureFlags.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureFlags.Web.Controllers
{
    public interface IServiceAPIClient
    {
        Task<List<FeatureFlag>> GetFeatureFlags();

        Task<FeatureFlag> GetFeatureFlag(string name);

        Task<bool> AddFeatureFlag(FeatureFlag featureFlag);

        Task<bool> DeleteFeatureFlag(FeatureFlag featureFlag);
    }
}
