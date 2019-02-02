using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOBCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOBCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoExceptionController : ControllerBase
    {
        [HttpGet]
        public async Task<APIResult> Get()
        {
            throw new StackOverflowException();
            return new APIResult();
        }
    }
}