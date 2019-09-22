using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeatureFlags.Models;
using FeatureFlags.Service.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;

namespace FeatureFlags.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureFlagsController : ControllerBase
    {
        private readonly IFeatureFlagsStorageTable _featureFlagsDA;

        public FeatureFlagsController(IFeatureFlagsStorageTable featureFlagsDA)
        {
            _featureFlagsDA = featureFlagsDA;
        }

        [HttpGet("GetFeatureFlags")]
        public async Task<IEnumerable<FeatureFlag>> GetFeatureFlags()
        {
            return await _featureFlagsDA.GetFeatureFlags();
        }

        [HttpGet("GetFeatureFlag")]
        public async Task<FeatureFlag> GetFeatureFlag(string name)
        {
            return await _featureFlagsDA.GetFeatureFlag(name);
        }

        [HttpPost("SaveFeatureFlag")]
        public async Task<bool> SaveFeatureFlag(FeatureFlag featureFlag)
        {
            return await _featureFlagsDA.SaveFeatureFlag(featureFlag);
        }

        [HttpGet("SaveFeatureFlagState")]
        public async Task<bool> SaveFeatureFlagState(string name, string environment, bool isEnabled)
        {
            FeatureFlag featureFlag = await _featureFlagsDA.GetFeatureFlag(name);
            if (featureFlag != null)
            {
                switch (environment.ToLower())
                {
                    case "dev":
                        featureFlag.DevIsEnabled = isEnabled;
                        break;
                    case "qa":
                        featureFlag.QAIsEnabled = isEnabled;
                        break;
                    case "prod":
                        featureFlag.ProdIsEnabled = isEnabled;
                        break;
                    default:
                        throw new Exception("Unknown environment: " + environment + " for feature flag " + name);
                }
                await _featureFlagsDA.SaveFeatureFlag(featureFlag);
                await _featureFlagsDA.CheckFeatureFlag(name, environment);
            }

            return true;
        }


        [HttpGet("CheckFeatureFlag")]
        public async Task<bool> CheckFeatureFlag(string name, string environment)
        {
            return await _featureFlagsDA.CheckFeatureFlag(name, environment);
        }


        [HttpPost("DeleteFeatureFlag")]
        public async Task<bool> DeleteFeatureFlag(string name)
        {
            return await _featureFlagsDA.DeleteFeatureFlag(name);
        }

    }
}