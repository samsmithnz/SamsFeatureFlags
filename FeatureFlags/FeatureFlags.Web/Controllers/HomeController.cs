using FeatureFlags.Models;
using FeatureFlags.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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
            Payload<List<FeatureFlag>> featureFlags = await _ServiceApiClient.GetFeatureFlags();

            return View(featureFlags);
        }

        public IActionResult AddFeatureFlag()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddFeatureFlagPost(string newName, string newDescription)
        {
            Payload<List<FeatureFlag>> featureFlags = await _ServiceApiClient.GetFeatureFlags();

            bool foundDuplicate = false;
            if (featureFlags.Data != null)
            {
                foreach (FeatureFlag item in featureFlags.Data)
                {
                    if (item.Name == newName)
                    {
                        foundDuplicate = true;
                    }
                }

                if (foundDuplicate == false)
                {
                    FeatureFlag featureFlag = new FeatureFlag(newName)
                    {
                        Name = newName,
                        Description = newDescription,
                        LastUpdated = DateTime.Now
                    };
                    await _ServiceApiClient.AddFeatureFlag(featureFlag);
                }
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
            return View(new ErrorViewModel(requestId: Activity.Current?.Id ?? HttpContext.TraceIdentifier));
        }
    }
}
