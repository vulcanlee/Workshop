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
    public class CommUserGroupsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        private readonly APIResult apiResult;

        public CommUserGroupsController(LOBDatabaseContext context, APIResult apiResult)
        {
            _context = context;
            this.apiResult = apiResult;
        }

        public async Task<APIResult> GetCommUserGroups()
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
            apiResult.Payload = CommUserGroupResponseDTO;
            return apiResult;
        }
    }
}