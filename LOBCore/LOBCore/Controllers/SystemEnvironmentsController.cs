using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOBCore.DataAccesses;
using LOBCore.DataAccesses.Entities;
using Microsoft.AspNetCore.Authorization;
using LOBCore.DataTransferObject.DTOs;
using LOBCore.BusinessObjects.Factories;
using LOBCore.Helpers;

namespace LOBCore.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemEnvironmentsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;

        public SystemEnvironmentsController(LOBDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/SystemEnvironments
        [HttpGet]
        public async Task<IActionResult> GetSystemEnvironment()
        {
            SystemEnvironmentResponseDTO SystemEnvironmentResponseDTO = new SystemEnvironmentResponseDTO();
            var fooObject = await _context.SystemEnvironment.FirstAsync();
            if(fooObject!=null)
            {
                SystemEnvironmentResponseDTO.Id = fooObject.Id;
                SystemEnvironmentResponseDTO.AppName = fooObject.AppName;
                SystemEnvironmentResponseDTO.AndroidVersion = fooObject.AndroidVersion;
                SystemEnvironmentResponseDTO.AndroidUrl = fooObject.AndroidUrl;
                SystemEnvironmentResponseDTO.iOSVersion = fooObject.iOSVersion;
                SystemEnvironmentResponseDTO.iOSUrl = fooObject.iOSUrl;
                APIResult apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                    ErrorMessageEnum.None, payload: SystemEnvironmentResponseDTO);
                return Ok(apiResult);
            }
            else
            {
                APIResult apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有任何符合資料存在);
                return NotFound(apiResult);
            }

        }
    }
}