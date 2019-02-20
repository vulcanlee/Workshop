using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOBCore.DataAccesses;
using LOBCore.DataAccesses.Entities;
using Microsoft.AspNetCore.Authorization;
using LOBCore.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionRecordsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        private readonly APIResult apiResult;
        int UserID;

        public ExceptionRecordsController(LOBDatabaseContext context, APIResult apiResult)
        {
            _context = context;
            this.apiResult = apiResult;
        }

        // GET: api/ExceptionRecords
        [HttpGet]
        public async Task<APIResult> GetExceptionRecords()
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            var fooUser = await _context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser == null)
            {
                apiResult.Status = false;
                apiResult.Message = "沒有發現指定的該使用者資料";
                return apiResult;
            }

            List<ExceptionRecordResponseDTO> ExceptionRecordResponseDTO = new List<ExceptionRecordResponseDTO>();
            var fooList = await _context.ExceptionRecords.Include(x => x.User)
                .Where(x => x.User.Id == fooUser.Id).OrderByDescending(x => x.ExceptionTime).Take(100).ToListAsync();
            List<ExceptionRecordResponseDTO> fooObject = new List<ExceptionRecordResponseDTO>();
            foreach (var item in fooList)
            {
                ExceptionRecordResponseDTO fooNode = new ExceptionRecordResponseDTO()
                {
                    Id = item.Id,
                    CallStack = item.CallStack,
                    DeviceModel = item.DeviceModel,
                    DeviceName = item.DeviceName,
                    ExceptionTime = item.ExceptionTime,
                    Message = item.Message,
                    OSType = item.OSType,
                    OSVersion = item.OSVersion,
                };
                fooObject.Add(fooNode);
            }
            apiResult.Payload = fooObject;
            return apiResult;
        }

        // POST: api/ExceptionRecords
        [HttpPost]
        public async Task<APIResult> PostExceptionRecord([FromBody] List<ExceptionRecordRequestDTO> exceptionRecords)
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                apiResult.Status = false;
                apiResult.Message = $"傳送過來的資料有問題 {ModelState}";
                return apiResult;
            }
            var fooUser = await _context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser != null)
            {
                foreach (var item in exceptionRecords)
                {
                    ExceptionRecord fooExceptionRecordObject = new ExceptionRecord()
                    {
                        User = fooUser,
                        CallStack = item.CallStack,
                        DeviceModel = item.DeviceModel,
                        DeviceName = item.DeviceName,
                        ExceptionTime = item.ExceptionTime,
                        Message = item.Message,
                        OSType = item.OSType,
                        OSVersion = item.OSVersion,
                    };
                    _context.ExceptionRecords.Add(fooExceptionRecordObject);
                }

                await _context.SaveChangesAsync();
                List<ExceptionRecordResponseDTO> fooObject = new List<ExceptionRecordResponseDTO>();
                //foreach (var item in exceptionRecords)
                //{
                //    var foo = new ExceptionRecordResponseDTO()
                //    {
                //        CallStack = item.CallStack,
                //        DeviceModel = item.DeviceModel,
                //        DeviceName = item.DeviceName,
                //        ExceptionTime = item.ExceptionTime,
                //        Message = item.Message,
                //        OSType = item.OSType,
                //        OSVersion = item.OSVersion,
                //    };
                //    fooObject.Add(foo);
                //}
                apiResult.Payload = fooObject;
            }
            else
            {
                apiResult.Status = false;
                apiResult.Message = "沒有發現指定的該使用者資料";
                return apiResult;
            }

            return apiResult;
        }
    }
}