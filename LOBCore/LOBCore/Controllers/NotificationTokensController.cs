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
using System.IdentityModel.Tokens.Jwt;
using LOBCore.DataTransferObject.DTOs;
using LOBCore.BusinessObjects.Factories;
using LOBCore.Helpers;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationTokensController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        APIResult apiResult;
        int UserID;

        public NotificationTokensController(LOBDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Suggestions
        [HttpGet]

        public async Task<IActionResult> GetSuggestions()
        {
            var claimSID = User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;
            if (claimSID == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.權杖中沒有發現指定使用者ID);
                return BadRequest(apiResult);
            }
            UserID = Convert.ToInt32(claimSID);
            var fooUser = await _context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的該使用者資料);
                return NotFound(apiResult);
            }

            List<NotificationTokenResponseDTO> NotificationTokenResponseDTOs = new List<NotificationTokenResponseDTO>();
            var fooList = await _context.NotificationTokens.Include(x => x.User)
                .Where(x => x.User.Id == fooUser.Id).OrderByDescending(x => x.RegistrationTime).Take(100).ToListAsync();
            foreach (var item in fooList)
            {
                NotificationTokenResponseDTO fooObject = new NotificationTokenResponseDTO()
                {
                    Id = item.Id,
                    User = new UserDTO()
                    {
                        Id = item.User.Id
                    },
                    Invalid = item.Invalid,
                    OSType = (OSTypeDTO)Enum.Parse(typeof(OSTypeDTO), item.OSType.ToString()),
                    RegistrationTime = item.RegistrationTime,
                    Token = item.Token,
                };
                NotificationTokenResponseDTOs.Add(fooObject);
            }
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: NotificationTokenResponseDTOs);
            return Ok(apiResult);
        }

        // POST: api/NotificationTokens
        [HttpPost]
        public async Task<IActionResult> PostNotificationToken([FromBody] NotificationTokenRequestDTO notificationToken)
        {
            var claimSID = User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;
            if (claimSID == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.權杖中沒有發現指定使用者ID);
                return BadRequest(apiResult);
            }
            UserID = Convert.ToInt32(claimSID);
            var fooUser = await _context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的該使用者資料);
                return NotFound(apiResult);
            }

            NotificationToken NotificationToken = new NotificationToken()
            {
                OSType = (OSType)Enum.Parse(typeof(OSType), notificationToken.OSType.ToString()),
                RegistrationTime = notificationToken.RegistrationTime,
                Token = notificationToken.Token,
                User = fooUser,
            };
            _context.NotificationTokens.Add(NotificationToken);
            await _context.SaveChangesAsync();

            NotificationTokenResponseDTO NotificationTokenResponseDTO = new NotificationTokenResponseDTO()
            {
                OSType = notificationToken.OSType,
                RegistrationTime = notificationToken.RegistrationTime,
                Token = notificationToken.Token,
                User = new UserDTO() { Id = fooUser.Id },
            };
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: NotificationTokenResponseDTO);
            return Ok(apiResult);
        }
    }
}