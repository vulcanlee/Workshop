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
        public static LoginResponseDTO ToLoginResponseDTO(this LobUser lobUser, string token, string refreshToken)
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
                RefreshToken = refreshToken,
            };
            return LoginResponseDTO;
        }
    }
}
