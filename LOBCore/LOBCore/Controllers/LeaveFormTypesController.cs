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
using LOBCore.DTOs;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveFormTypesController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        private readonly APIResult apiResult;

        public LeaveFormTypesController(LOBDatabaseContext context, APIResult apiResult)
        {
            _context = context;
            this.apiResult = apiResult;
        }

        [HttpGet]
        public APIResult GetLeaveFormTypes()
        {
            List<LeaveFormTypeResponseDTO> LeaveFormTypeResponseDTO = new List<LeaveFormTypeResponseDTO>();
            foreach (var item in _context.LeaveFormTypes)
            {
                LeaveFormTypeResponseDTO fooObject = new LeaveFormTypeResponseDTO()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                LeaveFormTypeResponseDTO.Add(fooObject);
            }
            apiResult.Payload = LeaveFormTypeResponseDTO;
            return apiResult;
        }
    }
}