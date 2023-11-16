using FeatureFlags.Models;
using FeatureFlags.Web2.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureFlags.Web2.Data
{
    public interface IServiceAPIClient
    {
        Task<Payload<List<FeatureFlag>>> GetFeatureFlags();
        Task<Payload<FeatureFlag>> GetFeatureFlag(string name);
        Task<Payload<bool>> AddFeatureFlag(FeatureFlag featureFlag);
        Task<Payload<bool>> DeleteFeatureFlag(FeatureFlag featureFlag);
    }
}
