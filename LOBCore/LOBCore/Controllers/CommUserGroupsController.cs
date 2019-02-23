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
    public class CommUserGroupsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;

        public CommUserGroupsController(LOBDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetCommUserGroups()
        {
            List<CommUserGroupResponseDTO> CommUserGroupResponseDTO = new List<CommUserGroupResponseDTO>();
            foreach (var item in await _context.CommUserGroups.ToListAsync())
            {
                CommUserGroupResponseDTO fooObject = new CommUserGroupResponseDTO()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                CommUserGroupResponseDTO.Add(fooObject);
            }
            APIResult apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: CommUserGroupResponseDTO);
            return Ok(apiResult);
        }
    }
}