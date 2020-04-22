using FeatureFlags.Models;
using FeatureFlags.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureFlags.Web.Controllers
{
    public interface IServiceAPIClient
    {
        Task<Data<List<FeatureFlag>>> GetFeatureFlags();
        Task<Data<FeatureFlag>> GetFeatureFlag(string name);
        Task<Data<bool>> AddFeatureFlag(FeatureFlag featureFlag);
        Task<Data<bool>> DeleteFeatureFlag(FeatureFlag featureFlag);
    }
}
