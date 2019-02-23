using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOBCore.DataAccesses;
using LOBCore.DataAccesses.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using LOBCore.Extensions;
using LOBCore.DataTransferObject.DTOs;
using LOBCore.BusinessObjects.Factories;
using LOBCore.Helpers;
using LOBCore.Filters;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveFormsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        int UserID;
        APIResult apiResult;

        public LeaveFormsController(LOBDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/LeaveForms
        [HttpGet]
        public async Task<IActionResult> GetLeaveForms()
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

            List<LeaveFormResponseDTO> fooLeaveFormResponseDTO = new List<LeaveFormResponseDTO>();
            foreach (var item in _context.LeaveForms.Include(x => x.LeaveFormType)
                .Include(x => x.User).ThenInclude(x => x.Department)
                .Where(x => x.User.Id == UserID))
            {
                LeaveFormResponseDTO fooObject = item.ToLeaveFormResponseDTO();
                fooLeaveFormResponseDTO.Add(fooObject);
            }
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: fooLeaveFormResponseDTO);
            return Ok(apiResult);
        }

        // GET: api/LeaveForms/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveForm([FromRoute] int id)
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

            if (!ModelState.IsValid)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.傳送過來的資料有問題, exceptionMessage: $"傳送過來的資料有問題 {ModelState}");
                return BadRequest(apiResult);
            }

            var leaveForm = await _context.LeaveForms.Include(x => x.LeaveFormType)
                .Include(x => x.User).ThenInclude(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (leaveForm == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的請假單);
                return NotFound(apiResult);
            }
            else if (leaveForm.User.Id != UserID)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.權杖Token上標示的使用者與傳送過來的使用者不一致);
                return BadRequest(apiResult);
            }

            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: leaveForm.ToLeaveFormResponseDTO());
            return Ok(apiResult);
        }

        // PUT: api/LeaveForms/5
        [HttpPut]
        public async Task<IActionResult> PutLeaveForm([FromBody] LeaveFormRequestDTO leaveForm)
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

            if (!ModelState.IsValid)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.傳送過來的資料有問題, exceptionMessage: $"傳送過來的資料有問題 {ModelState}");
                return BadRequest(apiResult);
            }

            var fooLeaveFormType = await _context.LeaveFormTypes.FindAsync(leaveForm.leaveFormType.Id);
            if (fooLeaveFormType == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                    ErrorMessageEnum.沒有發現指定的請假單類別);
                return NotFound(apiResult);
            }

            var leaveFormOnDB = await _context.LeaveForms.Include(x => x.LeaveFormType)
                .Include(x => x.User).ThenInclude(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == leaveForm.id);

            if (leaveFormOnDB == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的請假單);
                return NotFound(apiResult);
            }
            else if (leaveFormOnDB.User.Id != UserID)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.權杖Token上標示的使用者與傳送過來的使用者不一致);
                return BadRequest(apiResult);
            }

            leaveFormOnDB.BeginTime = leaveForm.BeginTime;
            leaveFormOnDB.Description = leaveForm.Description;
            leaveFormOnDB.EndTime = leaveForm.EndTime;
            leaveFormOnDB.TotalHours = leaveForm.TotalHours;
            leaveFormOnDB.LeaveFormType = fooLeaveFormType;
            _context.Entry(leaveFormOnDB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LeaveFormExists(leaveForm.id))
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status409Conflict,
                        ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
                    return Conflict(apiResult);
                }
                else
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status409Conflict,
                        ErrorMessageEnum.紀錄更新時_發生同時存取衝突);
                    return Conflict(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status500InternalServerError,
                    Helpers.ErrorMessageEnum.Exception,
                    exceptionMessage: $"({ex.GetType().Name}), {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, apiResult);
            }

            apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
               ErrorMessageEnum.None, payload: leaveForm);
            return Accepted(apiResult);
        }

        // POST: api/LeaveForms
        [HttpPost]
        public async Task<IActionResult> PostLeaveForm([FromBody] LeaveFormRequestDTO leaveForm)
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

            if (!ModelState.IsValid)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.傳送過來的資料有問題, exceptionMessage: $"傳送過來的資料有問題 {ModelState}");
                return BadRequest(apiResult);
            }

            var fooLeaveFormType = await _context.LeaveFormTypes.FindAsync(leaveForm.leaveFormType.Id);
            if (fooLeaveFormType == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                    ErrorMessageEnum.沒有發現指定的請假單類別);
                return NotFound(apiResult);
            }

            LeaveForm fooLeaveForm = leaveForm.ToLeaveForm(fooUser, fooLeaveFormType);
            _context.LeaveForms.Add(fooLeaveForm);
            await _context.SaveChangesAsync();

            apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
               ErrorMessageEnum.None, payload: leaveForm);
            return Accepted(apiResult);
        }

        // DELETE: api/LeaveForms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveForm([FromRoute] int id)
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            var fooUser = await _context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的該使用者資料);
                return NotFound(apiResult);
            }

            if (!ModelState.IsValid)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.傳送過來的資料有問題, exceptionMessage: $"傳送過來的資料有問題 {ModelState}");
                return BadRequest(apiResult);
            }

            var leaveForm = await _context.LeaveForms.Include(x => x.LeaveFormType)
                .Include(x => x.User).ThenInclude(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (leaveForm == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的請假單);
                return NotFound(apiResult);
            }
            else if (leaveForm.User.Id != UserID)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.權杖Token上標示的使用者與傳送過來的使用者不一致);
                return BadRequest(apiResult);
            }

            _context.LeaveForms.Remove(leaveForm);
            await _context.SaveChangesAsync();

            apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
               ErrorMessageEnum.None, payload: leaveForm.ToLeaveFormResponseDTO());
            return Accepted(apiResult);
        }

        private async Task<bool> LeaveFormExists(int id)
        {
            return await _context.LeaveForms.AnyAsync(e => e.Id == id);
        }
    }
}