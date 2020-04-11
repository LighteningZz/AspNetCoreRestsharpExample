using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRestsharpExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Newtonsoft.Json;
using AspNetCoreRestsharpExample.Utilities;

namespace AspNetCoreRestsharpExample.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IConfiguration _conf;
        private readonly string baseUrl;
        public EmployeeController(IConfiguration conf)
        {
            // .NET Core จะทำการ Dependency Injection IConfiguration มาให้เรา Auto อยู่แล้ว
            _conf = conf;
            // Base Url ที่อยู่ใน Config เจ้า Config ตัวเนี้ยจะอ่านจาก Environment Variables และ Appsetting 
            baseUrl = _conf.GetSection("DummyAPI:BaseUrl").Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // เรียก API PATH จาก Config ของเรา
            string apiPath = _conf.GetSection("DummyAPI:GetAll").Value;
            RestClient client = new RestClient(baseUrl);
            RestRequest request = new RestRequest(apiPath, Method.GET);
            var response = await client.ExecuteAsync<EmployeeListResponseModel>(request);
            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Content);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int Id)
        {
            // เรียก API PATH จาก Config ของเรา
            string apiPath = _conf.GetSection("DummyAPI:GetById").Value;

            // CookieContainer เปิดใช้ Cookies กรณีทำ Web Scraping ได้ใช้แน่นอน
            RestClient client = new RestClient(baseUrl) { CookieContainer = new System.Net.CookieContainer() };

            RestRequest request = new RestRequest($"{apiPath}".Replace("{Id}", $"{Id}"), Method.GET);
            /*
            //กรณีมี Query String 
            request.AddParameter(new Parameter("something_query_string", "value_data", ParameterType.GetOrPost));
            */

# if DEBUG
            /* 
             * เคยเขียน API ดึง Live Score ตอน Development 
             * เครื่องเรามันดันจำ Cache ไว้ ก็ว่าทำไมมันไม่อัพเดท ฮ่า ๆ
             * แต่ขึ้น Server แล้วดันไม่เป็นนะ
             * ใส่ตัวอย่างไว้เผื่อเจอปัญหานี้กัน
            */
            request.AddHeader("RequestId", Guid.NewGuid().ToString());
# endif
            // พอดี API ตัวนี้ดันต้องใช้ Cookie ก็เลยต้อง Request ไป 1 ทีเพื่อเอา Cookie
            await client.ExecuteGetAsync(request);

            //จะเห็นว่าบรรทัดนี้ผมได้เขียน Class มาลอง Response ที่ API Return กลับมาหาเรา
            var response = await client.ExecuteGetAsync<EmployeeResponseModel>(request);
            if (response.IsSuccessful)
            {
                //Response ที่แปลงจาก JSON เป็น Object ให้เราจะมาอยู่ใน response.Data
                return Ok(response.Data);
            }
            return BadRequest(response.Content);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EmployeeRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model.");
            }
            string apiPath = _conf.GetSection("DummyAPI:Create").Value;
            RestClient client = new RestClient(baseUrl);
            RestRequest request = new RestRequest($"{apiPath}", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            //กรณี Propery ตรงกับ Json Property
            //request.AddJsonBody(model);

            //วิธีแรก หากกรณี Propery กับ Json Propery ไม่ตรงกัน
            string json = JsonConvert.SerializeObject(model, Formatting.Indented);
            request.AddParameter(new Parameter("application/json", json, ParameterType.RequestBody));

            /*
            //วิธีที่สอง หากกรณี Propery กับ Json Propery ไม่ตรงกัน
            request.JsonSerializer = new JsonNetSerializer();
            request.AddJsonBody(model);
            */

            /*
            //กรณี POST FORM แบบ application/x-www-form-urlencoded
            //request.AddParameter(new Parameter("name", model.Name, ParameterType.GetOrPost));
            //request.AddParameter(new Parameter("salary", model.Salary, ParameterType.GetOrPost));
            //request.AddParameter(new Parameter("age", model.Age, ParameterType.GetOrPost));
            */
            var response = await client.ExecutePostAsync<EmployeeCreateResponseModel>(request);
            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Content);
        }


    }
}