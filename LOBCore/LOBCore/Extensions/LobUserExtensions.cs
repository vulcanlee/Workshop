using LOBCore.DataAccesses.Entities;
using LOBCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.Extensions
{
    public static class LobUserExtensions
    {
        public static LoginResponseDTO ToLoginResponseDTO(this LobUser lobUser, string token, string refreshToken,
            string tokenExpireMinutes, string refreshTokenExpireDays)
        {
            LoginResponseDTO LoginResponseDTO = new LoginResponseDTO()
            {
                Account = lobUser.Account,
                Id = lobUser.Id,
                Name = lobUser.Name,
                Image = lobUser.Image,
                Department = new DepartmentDTO()
                {
                    Id = lobUser.Department.Id,
                },
                Token = token,
                TokenExpireMinutes = Convert.ToInt32(tokenExpireMinutes),
                RefreshToken = refreshToken,
                RefreshTokenExpireDays = Convert.ToInt32(refreshTokenExpireDays),
            };
            return LoginResponseDTO;
        }
    }
}
