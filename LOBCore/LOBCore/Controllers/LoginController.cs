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
        [HttpPost]
        public async Task<APIResult> Post(LoginRequestDTO loginRequestDTO)
        {
            var fooUser = await context.LobUsers.Include(x=>x.Department).FirstOrDefaultAsync(x => x.Account == loginRequestDTO.Account && x.Password == loginRequestDTO.Password);
            if (fooUser != null)
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
                    //expires: DateTime.Now.AddDays(Convert.ToDouble(configuration["JwtExpireDays"])),
                    expires: DateTime.Now.AddDays(10),
                    notBefore:DateTime.Now.AddSeconds(-30),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                                (Encoding.UTF8.GetBytes(configuration["Tokens:IssuerSigningKey"])),
                            SecurityAlgorithms.HmacSha512)
                );
                apiResult.Status = APIResultStatus.Success;
                apiResult.Token = new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = "帳號或密碼不正確";
            }

            return apiResult;
        }
    }
}