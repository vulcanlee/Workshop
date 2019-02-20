using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOBCore.DataAccesses;
using LOBCore.DataAccesses.Entities;
using LOBCore.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LOBCore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class InitController : ControllerBase
    {
        private readonly LOBDatabaseContext lobDatabaseContext;

        public InitController(LOBDatabaseContext lobDatabaseContext)
        {
            this.lobDatabaseContext = lobDatabaseContext;
        }
        [HttpGet]
        public async Task<APIResult> Get()
        {
            //var foo = lobDatabaseContext.LobUsers.First();
            //var foo1 = lobDatabaseContext.LobUsers.ToList();
            //var bar = JsonConvert.SerializeObject(foo);

            await CleanDB();
            await CommUserGroupReset();
            await CommUserGroupItemReset();
            await DepartmentReset();
            await LobUserReset();
            await LeaveFormTypeReset();
            await SystemEnvironmentReset();

            var fooReslut = new APIResult()
            {
                Status = true,
                Message = "",
                Payload = lobDatabaseContext.LobUsers
            };
            return fooReslut;
        }

        private async Task CleanDB()
        {
            lobDatabaseContext.SystemEnvironment.RemoveRange(lobDatabaseContext.SystemEnvironment);
            lobDatabaseContext.Suggestions.RemoveRange(lobDatabaseContext.Suggestions);
            lobDatabaseContext.ExceptionRecords.RemoveRange(lobDatabaseContext.ExceptionRecords);
            lobDatabaseContext.NotificationTokens.RemoveRange(lobDatabaseContext.NotificationTokens);
            lobDatabaseContext.LeaveForms.RemoveRange(lobDatabaseContext.LeaveForms);
            lobDatabaseContext.LeaveFormTypes.RemoveRange(lobDatabaseContext.LeaveFormTypes);
            lobDatabaseContext.CommUserGroupItems.RemoveRange(lobDatabaseContext.CommUserGroupItems);
            lobDatabaseContext.CommUserGroups.RemoveRange(lobDatabaseContext.CommUserGroups);
            lobDatabaseContext.LobUsers.RemoveRange(lobDatabaseContext.LobUsers);
            lobDatabaseContext.Departments.RemoveRange(lobDatabaseContext.Departments);
            await lobDatabaseContext.SaveChangesAsync();
        }

        private async Task SystemEnvironmentReset()
        {
            var fooSystemEnvironment = new SystemEnvironment()
            {
                AppName = $"LOB 應用練習專案",
                AndroidVersion = "1.0.0.0",
                AndroidUrl = "",
                iOSVersion = "1.0.0.0",
                iOSUrl = "",
            };
            lobDatabaseContext.SystemEnvironment.Add(fooSystemEnvironment);
            await lobDatabaseContext.SaveChangesAsync();
        }

        private async Task LeaveFormTypeReset()
        {
            for (int i = 0; i < 3; i++)
            {
                LeaveFormType fooLeaveFormType = new LeaveFormType()
                {
                    Name = $"LeaveFormType{i}",
                };
                lobDatabaseContext.LeaveFormTypes.Add(fooLeaveFormType);
            }
            await lobDatabaseContext.SaveChangesAsync();
        }

        private async Task CommUserGroupReset()
        {
            for (int i = 0; i < 3; i++)
            {
                CommUserGroup fooCommUserGroup = new CommUserGroup()
                {
                    Name = $"Department{i}",
                    Description = $"Department{i}"
                };
                lobDatabaseContext.CommUserGroups.Add(fooCommUserGroup);
            }
            await lobDatabaseContext.SaveChangesAsync();
        }

        private async Task CommUserGroupItemReset()
        {
            for (int i = 0; i <= 30; i++)
            {
                var fooIdx = i % 3;
                var fooCommUserGroup = lobDatabaseContext.CommUserGroups.FirstOrDefault(x => x.Name == $"Department{fooIdx}");
                CommUserGroupItem fooUser = new CommUserGroupItem()
                {
                    Name = $"Name{i}",
                    CommUserGroup = fooCommUserGroup,
                    Email = $"email{i:d2}@vulcanlab.net",
                    Mobile = $"0900567{i:d2}",
                    Phone = $"02123456{i:d2}",
                };
                lobDatabaseContext.CommUserGroupItems.Add(fooUser);
            }
            await lobDatabaseContext.SaveChangesAsync();
        }

        private async Task DepartmentReset()
        {
            for (int i = 0; i < 7; i++)
            {
                Department fooDepartment = new Department()
                {
                    Name = $"Department{i}",
                };
                lobDatabaseContext.Departments.Add(fooDepartment);
            }
            await lobDatabaseContext.SaveChangesAsync();
        }

        private async Task LobUserReset()
        {
            for (int i = 1; i <= 50; i++)
            {
                var fooIdx = (i - 1) % 7;
                var fooDepartment = lobDatabaseContext.Departments.FirstOrDefault(x => x.Name == $"Department{fooIdx}");
                LobUser fooUser = new LobUser()
                {
                    Account = $"user{i}",
                    Password = $"password{i}",
                    Image = $"",
                    Name = $"Account{i}",
                    Department = fooDepartment,
                };
                lobDatabaseContext.LobUsers.Add(fooUser);
            }
            await lobDatabaseContext.SaveChangesAsync();
        }
    }
}