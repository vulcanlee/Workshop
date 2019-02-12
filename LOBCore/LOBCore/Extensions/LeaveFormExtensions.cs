using LOBCore.DataAccesses.Entities;
using LOBCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.Extensions
{
    public static class LeaveFormExtensions
    {
        public static LeaveForm ToLeaveForm(this LeaveFormRequestDTO leaveFormRequestDTO, LobUser lobUser, LeaveFormType leaveFormType)
        {
            LeaveForm fooLeaveForm = new LeaveForm()
            {
                BeginTime = leaveFormRequestDTO.BeginTime,
                EndTime = leaveFormRequestDTO.EndTime,
                Description = leaveFormRequestDTO.Description,
                TotalHours = leaveFormRequestDTO.TotalHours,
                User = lobUser,
                LeaveFormType = leaveFormType,
            };
            return fooLeaveForm;
        }

        public static LeaveFormResponseDTO ToLeaveFormResponseDTO(this LeaveForm leaveForm)
        {
            LeaveFormResponseDTO LeaveFormResponseDTO = new LeaveFormResponseDTO()
            {
                BeginTime = leaveForm.BeginTime,
                EndTime = leaveForm.EndTime,
                Description = leaveForm.Description,
                Id = leaveForm.Id,
                TotalHours = leaveForm.TotalHours,
                leaveFormType = new LeaveFormTypeDTO()
                {
                    Id = leaveForm.LeaveFormType.Id
                },
                user = new UserDTO()
                {
                    Id = leaveForm.User.Id,
                    Department = new DepartmentDTO()
                    {
                        Id = leaveForm.User.Department.Id
                    }
                }
            };
            return LeaveFormResponseDTO;
        }
    }
}
