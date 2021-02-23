using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FeatureFlags.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Get an array of strings "value1" and "value2"
        /// </summary>
        /// <returns>IEnumerable string array</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Get a single string "value"
        /// </summary>
        /// <param name="id">An integer number, e.g. 1</param>
        /// <returns>A single string</returns>
        [HttpGet("{id}")]
        public string Get(int id)
        { 
            return "value";
        }

        /// <summary>
        /// Post a string value 
        /// </summary>
        /// <param name="value">a string</param>
        /// <returns>bool/ true if successful</returns>
        [HttpPost]
        public bool Post([FromBody] string value)
        {
            return true;
        }

        /// <summary>
        /// Put a string value 
        /// </summary>
        /// <param name="id">An integer number, e.g. 1</param>
        /// <param name="value">a string</param>
        /// <returns>bool/ true if successful</returns>
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody] string value)
        {
            return true;
        }

        /// <summary>
        /// Delete a value
        /// </summary>
        /// <param name="id">An integer number, e.g. 1</param>
        /// <returns>bool/ true if successful</returns>
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return true;
        }
    }
}
