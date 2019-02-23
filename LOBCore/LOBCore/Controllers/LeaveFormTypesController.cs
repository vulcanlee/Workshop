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
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveFormTypesController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;

        public LeaveFormTypesController(LOBDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetLeaveFormTypes()
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
            APIResult apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: LeaveFormTypeResponseDTO);
            return Ok(apiResult);
        }
    }
}