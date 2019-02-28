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
using System.IdentityModel.Tokens.Jwt;
using LOBCore.DataTransferObject.DTOs;
using LOBCore.BusinessObjects.Factories;
using LOBCore.Helpers;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionRecordsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        APIResult apiResult;
        int UserID;

        public ExceptionRecordsController(LOBDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ExceptionRecords
        [HttpGet]
        public async Task<IActionResult> GetExceptionRecords()
        {
            var claimSID = User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;
            if (claimSID == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.權杖中沒有發現指定使用者ID);
                return BadRequest(apiResult);
            }
            UserID = Convert.ToInt32(claimSID);
            var fooUser = await _context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的該使用者資料);
                return NotFound(apiResult);
            }

            var fooList = await _context.ExceptionRecords.Include(x => x.User)
                .Where(x => x.User.Id == fooUser.Id).OrderByDescending(x => x.ExceptionTime).Take(100).ToListAsync();
            List<ExceptionRecordResponseDTO> ExceptionRecordResponseDTOs = new List<ExceptionRecordResponseDTO>();
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
                    OSType = (OSTypeDTO)Enum.Parse(typeof(OSTypeDTO), item.OSType.ToString()),
                    OSVersion = item.OSVersion,
                };
                ExceptionRecordResponseDTOs.Add(fooNode);
            }
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: ExceptionRecordResponseDTOs);
            return Ok(apiResult);
        }

        // POST: api/ExceptionRecords
        [HttpPost]
        public async Task<IActionResult> PostExceptionRecord([FromBody] List<ExceptionRecordRequestDTO> exceptionRecords)
        {
            var claimSID = User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;
            if (claimSID == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.權杖中沒有發現指定使用者ID);
                return BadRequest(apiResult);
            }
            UserID = Convert.ToInt32(claimSID);
            var fooUser = await _context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的該使用者資料);
                return NotFound(apiResult);
            }

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
                    OSType = (OSType)Enum.Parse(typeof(OSType), item.OSType.ToString()),
                    OSVersion = item.OSVersion,
                };
                _context.ExceptionRecords.Add(fooExceptionRecordObject);
            }

            await _context.SaveChangesAsync();
            List<ExceptionRecordResponseDTO> ExceptionRecordResponseDTOs = new List<ExceptionRecordResponseDTO>();
            foreach (var item in exceptionRecords)
            {
                var foo = new ExceptionRecordResponseDTO()
                {
                    CallStack = item.CallStack,
                    DeviceModel = item.DeviceModel,
                    DeviceName = item.DeviceName,
                    ExceptionTime = item.ExceptionTime,
                    Message = item.Message,
                    OSType = item.OSType,
                    OSVersion = item.OSVersion,
                };
                ExceptionRecordResponseDTOs.Add(foo);
            }
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: ExceptionRecordResponseDTOs);
            return Ok(apiResult);
        }
    }
}