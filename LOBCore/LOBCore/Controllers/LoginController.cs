using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LOBCore.DataAccesses;
using LOBCore.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using LOBCore.Extensions;
using LOBCore.DataAccesses.Entities;

namespace LOBCore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly LOBDatabaseContext context;
        private readonly APIResult apiResult;
        int UserID;

        public LoginController(IConfiguration configuration, LOBDatabaseContext context, APIResult APIResult)
        {
            this.configuration = configuration;
            this.context = context;
            apiResult = APIResult;
        }
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.NameId, "vulcan.lee@vulcan.net"),
        //        new Claim(ClaimTypes.Role, "Admin"),
        //    };

        //    var token = new JwtSecurityToken
        //    (
        //        issuer: configuration["Tokens:ValidIssuer"],
        //        audience: configuration["Tokens:ValidAudience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(Convert.ToDouble(configuration["JwtExpireDays"])),
        //        signingCredentials: new SigningCredentials(new SymmetricSecurityKey
        //        (Encoding.UTF8.GetBytes(configuration["Tokens:IssuerSigningKey"])),
        //        SecurityAlgorithms.HmacSha512)
        //    );

        //    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        //}
        [AllowAnonymous]
        [HttpPost]
        public async Task<APIResult> Post(LoginRequestDTO loginRequestDTO)
        {
            var fooUser = await context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Account == loginRequestDTO.Account && x.Password == loginRequestDTO.Password);
            if (fooUser != null)
            {
                apiResult.Status = true;
                string token = GenerateToken(fooUser);
                string refreshToken = GenerateRefreshToken(fooUser);

                LoginResponseDTO LoginResponseDTO = fooUser.ToLoginResponseDTO(
                    token, refreshToken,
                    configuration["Tokens:JwtExpireMinutes"], configuration["Tokens:JwtRefreshExpireDays"]);
                apiResult.Payload = LoginResponseDTO;
            }
            else
            {
                apiResult.Status = false;
                apiResult.Message = "帳號或密碼不正確";
            }

            return apiResult;
        }

        [Authorize(Roles = "RefreshToken")]
        [Route("RefreshToken")]
        [HttpGet]
        public async Task<APIResult> RefreshToken()
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            var fooUser = await context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser == null)
            {
                apiResult.Status = false;
                apiResult.Message = "沒有發現指定的該使用者資料";
                return apiResult;
            }
            else
            {
                apiResult.Status = true;
                string token = GenerateToken(fooUser);
                string refreshToken = GenerateRefreshToken(fooUser);

                LoginResponseDTO LoginResponseDTO = fooUser.ToLoginResponseDTO(
                    token, refreshToken,
                    configuration["Tokens:JwtExpireMinutes"], configuration["Tokens:JwtRefreshExpireDays"]);
                apiResult.Payload = LoginResponseDTO;
            }

            return apiResult;
        }

        public string GenerateToken(LobUser fooUser)
        {
            var claims = new[]
{
                    new Claim(JwtRegisteredClaimNames.Sid, fooUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, fooUser.Account),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.Role, $"Dept{fooUser.Department.Id}"),
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