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
    public class CommUserGroupItemsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;

        public CommUserGroupItemsController(LOBDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetCommUserGroupItems(CommUserGroupItemRequestDTO CommUserGroupItemRequestDTO)
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
            APIResult apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: CommUserGroupItemResponseDTO);
            return Ok(apiResult);
        }
    }
}