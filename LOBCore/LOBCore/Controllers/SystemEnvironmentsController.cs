﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOBCore.DataAccesses;
using LOBCore.DataAccesses.Entities;
using Microsoft.AspNetCore.Authorization;
using LOBCore.DTOs;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemEnvironmentsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        private readonly APIResult apiResult;

        public SystemEnvironmentsController(LOBDatabaseContext context, APIResult apiResult)
        {
            _context = context;
            this.apiResult = apiResult;
        }

        // GET: api/SystemEnvironments
        [HttpGet]
        public async Task<APIResult> GetSystemEnvironment()
        {
            SystemEnvironmentResponseDTO SystemEnvironmentResponseDTO = new SystemEnvironmentResponseDTO();
            var fooObject = await _context.SystemEnvironment.FirstAsync();
            if(fooObject!=null)
            {
                SystemEnvironmentResponseDTO.AndroidVersion = fooObject.AndroidVersion;
                SystemEnvironmentResponseDTO.iOSVersion = fooObject.iOSVersion;
                apiResult.Payload = SystemEnvironmentResponseDTO;
            }
            else
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"系統環境資料表內沒有任何紀錄存在";
            }

            return apiResult;
        }
    }
}