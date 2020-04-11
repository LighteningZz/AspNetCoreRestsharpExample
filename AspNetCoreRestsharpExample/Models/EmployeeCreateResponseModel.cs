using Newtonsoft.Json;

namespace AspNetCoreRestsharpExample.Models
{
    public partial class EmployeeCreateResponseModel
    {

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public Employee Data { get; set; }
        public partial class Employee
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("salary")]
            public int Salary { get; set; }

            [JsonProperty("age")]
            public int Age { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }
        }
    }
}
