using FeatureFlagsDemo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureFlagsDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IFeatureFlagsServiceApiClient _featureFlagsServiceApiClient;

        public HomeController(IConfiguration configuration, IFeatureFlagsServiceApiClient featureFlagsServiceApiClient)
        {
            _configuration = configuration;
            _featureFlagsServiceApiClient = featureFlagsServiceApiClient;
        }

        public async Task<IActionResult> IndexAsync()
        {
            IndexViewModel indexPageData = new()
            {
                DivideByZeroFeatureFlag = false,
                VerticalProductsFeatureFlag = false
            };


            //Divide by zero feature flag
            if (_featureFlagsServiceApiClient != null)
            {
                indexPageData.DivideByZeroFeatureFlag = await _featureFlagsServiceApiClient.CheckFeatureFlag("DivideByZero", _configuration["AppSettings:Environment"].ToString());
                indexPageData.VerticalProductsFeatureFlag = await _featureFlagsServiceApiClient.CheckFeatureFlag("VerticalProducts", _configuration["AppSettings:Environment"].ToString());
            }
            if (indexPageData.DivideByZeroFeatureFlag == true)
            {
                int i = 1;
                int j = 0;
                Console.WriteLine(i / j);
            }

            return View(indexPageData);
        }

        public IActionResult Privacy()
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
