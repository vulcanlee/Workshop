using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            return value1 + value2;
        }
        [HttpGet("AddAsync/{value1}/{value2}/{delay}")]
        public async Task<string> AddAsync(int value1, int value2, int delay)
        {
            DateTime Begin = DateTime.Now;
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            int workerThreadsMax;
            int completionPortThreadsMax;
            int workerThreadsMin;
            int completionPortThreadsMin;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            ThreadPool.GetMaxThreads(out workerThreadsMax, out completionPortThreadsMax);
            ThreadPool.GetMinThreads(out workerThreadsMin, out completionPortThreadsMin);
            await Task.Delay(delay);
            var fooUrl = Url.Action("ResponAndAwait2");
            DateTime Complete = DateTime.Now;
            return $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
                $" MaxW:{workerThreadsMax} MaxC:{completionPortThreadsMax}" +
                $" MinW:{workerThreadsMin} MinC:{completionPortThreadsMin} ({Begin.TimeOfDay} - {Complete.TimeOfDay})";
        }
        [HttpGet("AddSync/{value1}/{value2}/{delay}")]
        public string AddSync(int value1, int value2, int delay)
        {
            DateTime Begin = DateTime.Now;
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            int workerThreadsMax;
            int completionPortThreadsMax;
            int workerThreadsMin;
            int completionPortThreadsMin;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            ThreadPool.GetMaxThreads(out workerThreadsMax, out completionPortThreadsMax);
            ThreadPool.GetMinThreads(out workerThreadsMin, out completionPortThreadsMin);
            Thread.Sleep(delay);
            var fooUrl = Url.Action("ResponAndAwait2");
            DateTime Complete = DateTime.Now;
            return $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
                $" MaxW:{workerThreadsMax} MaxC:{completionPortThreadsMax}" +
                $" MinW:{workerThreadsMin} MinC:{completionPortThreadsMin} ({Begin.TimeOfDay} - {Complete.TimeOfDay})";
        }

        [HttpGet("SetThreadPool/{value1}/{value2}")]
        public string SetThreadPool(int value1, int value2)
        {
            ThreadPool.SetMinThreads(value1, value2);

            string result = "OK";
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            int workerThreadsMax;
            int completionPortThreadsMax;
            int workerThreadsMin;
            int completionPortThreadsMin;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            ThreadPool.GetMaxThreads(out workerThreadsMax, out completionPortThreadsMax);
            ThreadPool.GetMinThreads(out workerThreadsMin, out completionPortThreadsMin);

            DateTime Complete = DateTime.Now;
            result = "OK " + $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
                $" MaxW:{workerThreadsMax} MaxC:{completionPortThreadsMax}" +
                $" MinW:{workerThreadsMin} MinC:{completionPortThreadsMin} ";

            return result;
        }

        [HttpGet("SetMaxThreadPool/{value1}/{value2}")]
        public string SetMaxThreadPool(int value1, int value2)
        {
            ThreadPool.SetMaxThreads(value1, value2);

            string result = "OK";
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            int workerThreadsMax;
            int completionPortThreadsMax;
            int workerThreadsMin;
            int completionPortThreadsMin;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            ThreadPool.GetMaxThreads(out workerThreadsMax, out completionPortThreadsMax);
            ThreadPool.GetMinThreads(out workerThreadsMin, out completionPortThreadsMin);

            DateTime Complete = DateTime.Now;
            result = "OK " + $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
                $" MaxW:{workerThreadsMax} MaxC:{completionPortThreadsMax}" +
                $" MinW:{workerThreadsMin} MinC:{completionPortThreadsMin} ";

            return result;
        }


        [HttpGet("GetThreadPool")]
        public string GetThreadPool()
        {
            string result = "";
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            int workerThreadsMax;
            int completionPortThreadsMax;
            int workerThreadsMin;
            int completionPortThreadsMin;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            ThreadPool.GetMaxThreads(out workerThreadsMax, out completionPortThreadsMax);
            ThreadPool.GetMinThreads(out workerThreadsMin, out completionPortThreadsMin);

            DateTime Complete = DateTime.Now;
            result = " " + $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
                $" MaxW:{workerThreadsMax} MaxC:{completionPortThreadsMax}" +
                $" MinW:{workerThreadsMin} MinC:{completionPortThreadsMin} ";

            return result;
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