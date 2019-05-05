using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LOBCore.DataAccesses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using LOBCore.Extensions;
using LOBCore.DataAccesses.Entities;
using LOBCore.DataTransferObject.DTOs;
using LOBCore.Helpers;
using LOBCore.BusinessObjects.Factories;

namespace LOBCore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly LOBDatabaseContext context;
        int UserID;
        int TokenVersion;

        public LoginController(IConfiguration configuration, LOBDatabaseContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(LoginRequestDTO loginRequestDTO)
        {
            var fooUser = await context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Account == loginRequestDTO.Account && x.Password == loginRequestDTO.Password);
            if (fooUser == null)
            {
                APIResult apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                    ErrorMessageEnum.帳號或密碼不正確);
                return BadRequest(apiResult);
            }
            else
            {
                string token = GenerateToken(fooUser);
                string refreshToken = GenerateRefreshToken(fooUser);

                LoginResponseDTO LoginResponseDTO = fooUser.ToLoginResponseDTO(
                    token, refreshToken,
                    configuration["Tokens:JwtExpireMinutes"], configuration["Tokens:JwtRefreshExpireDays"]);
                APIResult apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                    ErrorMessageEnum.None, payload: LoginResponseDTO);
                return Ok(apiResult);
            }

        }

        [Authorize(Roles = "RefreshToken")]
        [Route("RefreshToken")]
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            TokenVersion = Convert.ToInt32(User.FindFirst(ClaimTypes.Version)?.Value);
            var fooUser = await context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser == null)
            {
                APIResult apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                    ErrorMessageEnum.沒有發現指定的該使用者資料);
                return NotFound(apiResult);
            }
            else
            {
                APIResult apiResult;
                if (fooUser.TokenVersion > TokenVersion)
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                     ErrorMessageEnum.使用者需要強制登出並重新登入以便進行身分驗證);
                    return BadRequest(apiResult);
                }

                string token = GenerateToken(fooUser);
                string refreshToken = GenerateRefreshToken(fooUser);

                LoginResponseDTO LoginResponseDTO = fooUser.ToLoginResponseDTO(
                    token, refreshToken,
                    configuration["Tokens:JwtExpireMinutes"], configuration["Tokens:JwtRefreshExpireDays"]);
                apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                   ErrorMessageEnum.None, payload: LoginResponseDTO);
                return Ok(apiResult);
            }

        }

        public string GenerateToken(LobUser fooUser)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, fooUser.Id.ToString()),
                new Claim(ClaimTypes.Name, fooUser.Account),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Role, $"Dept{fooUser.Department.Id}"),
                new Claim(ClaimTypes.Version, $"{fooUser.TokenVersion}"),
            };

            var token = new JwtSecurityToken
            (
                issuer: configuration["Tokens:ValidIssuer"],
                audience: configuration["Tokens:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Tokens:JwtExpireMinutes"])),
                //notBefore: DateTime.Now.AddMinutes(-5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(configuration["Tokens:IssuerSigningKey"])),
                        SecurityAlgorithms.HmacSha512)
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;

        }

        public string GenerateRefreshToken(LobUser fooUser)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, fooUser.Id.ToString()),
                new Claim(ClaimTypes.Name, fooUser.Account),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Role, $"RefreshToken"),
                new Claim(ClaimTypes.Version, $"{fooUser.TokenVersion}"),
            };

            var token = new JwtSecurityToken
            (
                issuer: configuration["Tokens:ValidIssuer"],
                audience: configuration["Tokens:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(configuration["Tokens:JwtRefreshExpireDays"])),
                //notBefore: DateTime.Now.AddMinutes(-5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(configuration["Tokens:IssuerSigningKey"])),
                        SecurityAlgorithms.HmacSha512)
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;

        }
    }
}