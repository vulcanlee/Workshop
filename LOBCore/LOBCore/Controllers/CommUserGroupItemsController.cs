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
    public class CommUserGroupItemsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        private readonly APIResult apiResult;

        public CommUserGroupItemsController(LOBDatabaseContext context, APIResult apiResult)
        {
            _context = context;
            this.apiResult = apiResult;
        }

        // GET: api/CommUserGroupItems
        [HttpGet]
        //public IEnumerable<CommUserGroupItem> GetCommUserGroupItems()
        //{
        //    return _context.CommUserGroupItems;
        //}

        public async Task<APIResult> GetCommUserGroupItems(CommUserGroupItemRequestDTO CommUserGroupItemRequestDTO)
        {
            List<CommUserGroupItemResponseDTO> CommUserGroupItemResponseDTO = new List<CommUserGroupItemResponseDTO>();
            foreach (var item in await _context.CommUserGroupItems.Include(x => x.CommUserGroup).Where(x => x.CommUserGroup.Id == CommUserGroupItemRequestDTO.Id).ToListAsync())
            {
                CommUserGroupItemResponseDTO fooObject = new CommUserGroupItemResponseDTO()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    Mobile = item.Mobile,
                    Phone = item.Phone,
                };
                CommUserGroupItemResponseDTO.Add(fooObject);
            }
            apiResult.Payload = CommUserGroupItemResponseDTO;
            return apiResult;
        }
    }
}