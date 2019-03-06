using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOBCore.DataTransferObject.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOBCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteSourceController : ControllerBase
    {
        [Route("Sample")]
        [HttpGet]
        public string Sample()
        {
            return $"來自遠端 ASP.NET Core Web API 服務的資料";
        }
        [Route("DemoBadRequest")]
        [HttpGet]
        public IActionResult DemoBadRequest()
        {
            APIResult fooResult = new APIResult();
            fooResult.Payload = new NotificationTokenResponseDTO();
            return BadRequest(fooResult);
        }
        [Route("DemoUnauthorized")]
        [HttpGet]
        public IActionResult DemoUnauthorized()
        {
            APIResult fooResult = new APIResult();
            fooResult.Payload = new NotificationTokenResponseDTO();
            return StatusCode(StatusCodes.Status401Unauthorized, fooResult);
        }
        [Route("Source1")]
        [HttpGet]
        public async Task<string> Source1()
        {
            await Task.Delay(7000);
            return $"Come from Source1 : 23.7 (延遲 7000 ms)";
        }
        [Route("Source2")]
        [HttpGet]
        public async Task<string> Source2()
        {
            await Task.Delay(5000);
            return $"Come from Source2 : 23.7 (延遲 5000 ms)";
        }
        [Route("Source3")]
        [HttpGet]
        public async Task<string> Source3()
        {
            await Task.Delay(2000);
            return $"Come from Source3 : 23.7 (延遲 3000 ms)";
        }
        [Route("Source4")]
        [HttpGet]
        public async Task<string> Source4()
        {
            await Task.Delay(10000);
            return $"Come from Source4 : 23.7 (延遲 10000 ms)";
        }

        [HttpGet("Add/{value1}/{value2}/{delay}")]
        public async Task<int> Add(int value1, int value2, int delay)
        {
            await Task.Delay(delay * 1000);
            var fooUrl = Url.Action("ResponAndAwait2");
            return value1+value2;
        }

        [HttpGet("ResponAndAwait1/{id}")]
        public async Task<string> ResponAndAwait1(int id)
        {
            await Task.Delay(id * 1000);
            var fooUrl = Url.Action("ResponAndAwait2");
            return fooUrl;
        }

        [HttpGet("ResponAndAwait2")]
        public string ResponAndAwait2()
        {
            return "最後結果內容";
        }
    }
}