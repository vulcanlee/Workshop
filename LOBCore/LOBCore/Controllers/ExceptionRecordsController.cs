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
            if (fooUser != null)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = "沒有發現指定的該使用者資料";
                return apiResult;
            }

            List<ExceptionRecordResponseDTO> ExceptionRecordResponseDTO = new List<ExceptionRecordResponseDTO>();
            var fooList = await _context.ExceptionRecords.Include(x=>x.User)
                .Where(x => x.User.Id == fooUser.Id).OrderByDescending(x=>x.ExceptionTime).Take(100).ToListAsync();
            foreach (var item in fooList)
            {
                ExceptionRecordResponseDTO fooObject = new ExceptionRecordResponseDTO()
                {
                    Id = item.Id,
                    User = new UserDTO()
                    {
                        Id = item.User.Id
                    },
                    CallStack = item.CallStack,
                    DeviceModel = item.DeviceModel,
                    DeviceName = item.DeviceName,
                    ExceptionTime = item.ExceptionTime,
                    Message = item.Message,
                    OSType = item.OSType,
                    OSVersion = item.OSVersion,
                };
                ExceptionRecordResponseDTO.Add(fooObject);
            }
            apiResult.Payload = ExceptionRecordResponseDTO;
            return apiResult;
        }

        // GET: api/ExceptionRecords/5
        [HttpGet("{id}")]
        public async Task<APIResult> GetExceptionRecord([FromRoute] int id)
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"傳送過來的資料有問題 {ModelState}";
                return apiResult;
            }

            var exceptionRecord = await _context.ExceptionRecords.Include(x=>x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (exceptionRecord == null)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"沒有發現指定的例外異常紀錄";
                return apiResult;
            }
            else if (exceptionRecord.User.Id != UserID)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"你沒有權限查看其他使用者的的例外異常紀錄";
                return apiResult;
            }

            ExceptionRecordResponseDTO fooObject = new ExceptionRecordResponseDTO()
            {
                Id = exceptionRecord.Id,
                User = new UserDTO()
                {
                    Id = exceptionRecord.User.Id
                },
                CallStack = exceptionRecord.CallStack,
                DeviceModel = exceptionRecord.DeviceModel,
                DeviceName = exceptionRecord.DeviceName,
                ExceptionTime = exceptionRecord.ExceptionTime,
                Message = exceptionRecord.Message,
                OSType = exceptionRecord.OSType,
                OSVersion = exceptionRecord.OSVersion,
            };
            apiResult.Payload = fooObject;

            return apiResult;
        }

        // POST: api/ExceptionRecords
        [HttpPost]
        public async Task<APIResult> PostExceptionRecord([FromBody] ExceptionRecordRequestDTO exceptionRecord)
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"傳送過來的資料有問題 {ModelState}";
                return apiResult;
            }
            var fooUser = await _context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser != null)
            {
                ExceptionRecord fooExceptionRecordObject = new ExceptionRecord()
                {
                    User = fooUser,
                    CallStack = exceptionRecord.CallStack,
                    DeviceModel = exceptionRecord.DeviceModel,
                    DeviceName = exceptionRecord.DeviceName,
                    ExceptionTime = exceptionRecord.ExceptionTime,
                    Message = exceptionRecord.Message,
                    OSType = exceptionRecord.OSType,
                    OSVersion = exceptionRecord.OSVersion,
                };
                _context.ExceptionRecords.Add(fooExceptionRecordObject);
                await _context.SaveChangesAsync();
                ExceptionRecordResponseDTO fooObject = new ExceptionRecordResponseDTO()
                {
                    User = new UserDTO()
                    {
                        Id = fooUser.Id
                    },
                    CallStack = exceptionRecord.CallStack,
                    DeviceModel = exceptionRecord.DeviceModel,
                    DeviceName = exceptionRecord.DeviceName,
                    ExceptionTime = exceptionRecord.ExceptionTime,
                    Message = exceptionRecord.Message,
                    OSType = exceptionRecord.OSType,
                    OSVersion = exceptionRecord.OSVersion,
                };
                apiResult.Payload = fooObject;
            }
            else
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = "沒有發現指定的該使用者資料";
                return apiResult;
            }

            return apiResult;
        }
    }
}