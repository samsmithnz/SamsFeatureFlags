using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureFlags.Web.Models
{
    public class Data<T>
    {
        public Data()
        {
            Message = "";
            Error = "";
        }

        public T Payload { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
    }
}
