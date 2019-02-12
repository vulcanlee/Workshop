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
using System.IdentityModel.Tokens.Jwt;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationTokensController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        private readonly APIResult apiResult;
        int UserID;

        public NotificationTokensController(LOBDatabaseContext context, APIResult apiResult)
        {
            _context = context;
            this.apiResult = apiResult;
        }

        // POST: api/NotificationTokens
        [HttpPost]
        public async Task<APIResult> PostNotificationToken([FromBody] NotificationTokenRequestDTO notificationToken)
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"傳送過來的資料有問題 {ModelState}";
                return apiResult;
            }

            var fooUser = await _context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser != null)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = "沒有發現指定的該使用者資料";
                return apiResult;
            }

            NotificationToken NotificationToken = new NotificationToken()
            {
                OSType = notificationToken.OSType,
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
            apiResult.Payload = NotificationTokenResponseDTO;
            return apiResult;
        }
    }
}