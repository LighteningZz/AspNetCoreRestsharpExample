using Newtonsoft.Json;

namespace AspNetCoreRestsharpExample.Models
{
    public  class EmployeeResponseModel
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public EmployeeDataResponseModel Data { get; set; }
      
    }
}
