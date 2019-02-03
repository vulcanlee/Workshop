using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOBCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteSourceController : ControllerBase
    {
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
    }
}