using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreRestsharpExample.Models
{
    public class EmployeeListResponseModel
    {

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public List<EmployeeDataResponseModel> Data { get; set; }

    
    }
}
