using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeatureFlags.Web.Models;
using FeatureFlags.Models;
using Microsoft.Extensions.FileProviders;

namespace FeatureFlags.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceAPIClient _ServiceApiClient;

        public HomeController(IServiceAPIClient ServiceApiClient)
        {
            _ServiceApiClient = ServiceApiClient;
        }

        public async Task<IActionResult> Index()
        {
            List<FeatureFlag> featureFlags = await _ServiceApiClient.GetFeatureFlags();

            return View(featureFlags);
        }

        public IActionResult AddFeatureFlag()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddFeatureFlagPost(string newName, string newDescription)
        {
            List<FeatureFlag> featureFlags = await _ServiceApiClient.GetFeatureFlags();

            bool foundDuplicate = false;
            foreach (FeatureFlag item in featureFlags)
            {
                if (item.Name == newName)
                {
                    foundDuplicate = true;
                }
            }

            if (foundDuplicate == false)
            {
                FeatureFlag featureFlag = new FeatureFlag(newName);
                featureFlag.Name = newName;
                featureFlag.Description = newDescription;
                featureFlag.LastUpdated = DateTime.Now;
                await _ServiceApiClient.AddFeatureFlag(featureFlag);
            }

            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
