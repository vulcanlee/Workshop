using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LOBCore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, "vulcan.lee@vulcan.net"),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var token = new JwtSecurityToken
            (
                issuer: configuration["Tokens:ValidIssuer"],
                audience: configuration["Tokens:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(configuration["JwtExpireDays"])),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Tokens:IssuerSigningKey"])),
                SecurityAlgorithms.HmacSha512)
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        // POST: api/Login
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var claims = new[]
      {
                new Claim(JwtRegisteredClaimNames.NameId, "vulcan.lee@vulcan.net"),
                new Claim(ClaimTypes.Role, "Admini"),
            };

            var token = new JwtSecurityToken
            (
                issuer: configuration["Tokens:ValidIssuer"],
                audience: configuration["Tokens:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(configuration["Tokens:IssuerSigningKey"])),
                        SecurityAlgorithms.HmacSha512)
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}