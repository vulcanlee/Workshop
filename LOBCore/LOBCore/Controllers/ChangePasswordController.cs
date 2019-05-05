using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOBCore.DataAccesses;
using LOBCore.DataAccesses.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using LOBCore.Extensions;
using LOBCore.DataTransferObject.DTOs;
using LOBCore.BusinessObjects.Factories;
using LOBCore.Helpers;
using LOBCore.Filters;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        int UserID;
        APIResult apiResult;

        public ChangePasswordController(LOBDatabaseContext context)
        {
            _context = context;
        }

        [HttpPut]
        public async Task<IActionResult> PutLeaveForm([FromBody] ChangePasswordRequestDTO changePasswordRequestDTO)
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

            if (!ModelState.IsValid)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.傳送過來的資料有問題, exceptionMessage: $"傳送過來的資料有問題 {ModelState}");
                return BadRequest(apiResult);
            }

            if (string.IsNullOrEmpty(changePasswordRequestDTO.NewPassword))
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.新密碼不能為空白);
                return BadRequest(apiResult);
            }

            if (fooUser.Password != changePasswordRequestDTO.OldPassword)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.原有密碼不正確);
                return BadRequest(apiResult);
            }

            fooUser.Password = changePasswordRequestDTO.NewPassword;
            fooUser.TokenVersion = fooUser.TokenVersion + 1;
            _context.Entry(fooUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status500InternalServerError,
                    Helpers.ErrorMessageEnum.Exception,
                    exceptionMessage: $"({ex.GetType().Name}), {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, apiResult);
            }

            apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
               ErrorMessageEnum.None, payload: new ChangePasswordResponseDTO() { Success = true });
            return Accepted(apiResult);
        }
    }
}