using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOBCore.Models;
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
        private readonly DatabaseContext databaseContext;

        public InitController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        [HttpGet]
        public async Task<APIResult> Get()
        {
            var foo = databaseContext.LobUsers.First();
            var foo1 = databaseContext.LobUsers.ToList();
            var bar = JsonConvert.SerializeObject(foo);

            await CleanDB();
            await CommUserGroupReset();
            await CommUserGroupItemReset();
            await DepartmentReset();
            await LobUserReset();


            var fooReslut = new APIResult()
            {
                Status = APIResultStatus.Success,
                Message = "",
                Token = "",
                Payload = databaseContext.LobUsers
            };
            return fooReslut;
        }

        private async Task CleanDB()
        {
            databaseContext.CommUserGroupItems.RemoveRange(databaseContext.CommUserGroupItems);
            databaseContext.CommUserGroups.RemoveRange(databaseContext.CommUserGroups);
            databaseContext.LobUsers.RemoveRange(databaseContext.LobUsers);
            databaseContext.Departments.RemoveRange(databaseContext.Departments);
            await databaseContext.SaveChangesAsync();
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
                databaseContext.CommUserGroups.Add(fooCommUserGroup);
            }
            await databaseContext.SaveChangesAsync();
        }

        private async Task CommUserGroupItemReset()
        {
            for (int i = 0; i <= 30; i++)
            {
                var fooIdx = i % 3;
                var fooCommUserGroup = databaseContext.CommUserGroups.FirstOrDefault(x => x.Name == $"Department{i}");
                CommUserGroupItem fooUser = new CommUserGroupItem()
                {
                    Name = $"Name{i}",
                    CommUserGroup = fooCommUserGroup,
                    Email = $"email{i:d2}@vulcanlab.net",
                    Mobile = $"0900567{i:d2}",
                    Phone = $"02123456{i:d2}",
                };
                databaseContext.CommUserGroupItems.Add(fooUser);
            }
            await databaseContext.SaveChangesAsync();
        }

        private async Task DepartmentReset()
        {
            for (int i = 0; i < 7; i++)
            {
                Department fooDepartment = new Department()
                {
                    Name = $"Department{i}",
                };
                databaseContext.Departments.Add(fooDepartment);
            }
            await databaseContext.SaveChangesAsync();
        }

        private async Task LobUserReset()
        {
            for (int i = 1; i <= 50; i++)
            {
                var fooIdx = (i - 1) % 7;
                var fooDepartment = databaseContext.Departments.FirstOrDefault(x => x.Name == $"Department{fooIdx}");
                LobUser fooUser = new LobUser()
                {
                    Account = $"user{i}",
                    Password = $"password{i}",
                    Image = $"",
                    Name = $"Account{i}",
                    Department = fooDepartment,
                };
                databaseContext.LobUsers.Add(fooUser);
            }
            await databaseContext.SaveChangesAsync();
        }
    }
}