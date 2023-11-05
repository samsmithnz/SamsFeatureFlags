using FeatureFlags.Models;
using FeatureFlags.Service.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
        public IEnumerable<FeatureFlag> GetFeatureFlags()
        {
            return _featureFlagsDA.GetFeatureFlags();
        }

        [HttpGet("GetFeatureFlag")]
        public FeatureFlag GetFeatureFlag(string name)
        {
            return _featureFlagsDA.GetFeatureFlag(name);
        }

        [HttpPost("SaveFeatureFlag")]
        public bool SaveFeatureFlag(FeatureFlag featureFlag)
        {
            return _featureFlagsDA.SaveFeatureFlag(featureFlag);
        }

        [HttpGet("SaveFeatureFlagState")]
        public bool SaveFeatureFlagState(string name, string environment, bool isEnabled)
        {
            FeatureFlag featureFlag = _featureFlagsDA.GetFeatureFlag(name);
            if (featureFlag != null)
            {
                switch (environment.ToLower())
                {
                    case "pr":
                        featureFlag.PRIsEnabled = isEnabled;
                        break;
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
                _featureFlagsDA.SaveFeatureFlag(featureFlag);
                _featureFlagsDA.CheckFeatureFlag(name, environment);
            }

            return true;
        }

        [HttpGet("CheckFeatureFlag")]
        public bool CheckFeatureFlag(string name, string environment)
        {
            return _featureFlagsDA.CheckFeatureFlag(name, environment);
        }

        [HttpPost("DeleteFeatureFlag")]
        public bool DeleteFeatureFlag(string name)
        {
            return _featureFlagsDA.DeleteFeatureFlag(name);
        }

    }
}