using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace AspNetCoreRestsharpExample.Models
{
    public class EmployeeRequestModel
    {
        /* Json Property จะใช้กรณีที่ Property ไม่ตรงกัน
         * เช่น Standard ที่เราใช้เป็น ProductId
         * แต่ API ที่เราเรียก product_id
         * ทั้ง Project เขียน Property Model มาเขียนเป็น SometingProperty หมด
         * อยู่จะมาเขียน public string someting_property { get; set; } ก็ดูแปลก ๆ ใช่ไหม
        */
        [JsonProperty("name"), Required]
        public string Name { get; set; }

        [JsonProperty("salary"), Required]
        public int Salary { get; set; }

        [JsonProperty("age"), Required]
        public int Age { get; set; }

    }
}
