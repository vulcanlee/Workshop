using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOBCore.DataAccesses;
using LOBCore.DataAccesses.Entities;
using System.IdentityModel.Tokens.Jwt;
using LOBCore.DataTransferObject.DTOs;
using LOBCore.BusinessObjects.Factories;
using LOBCore.Helpers;

namespace LOBCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        int UserID;
        APIResult apiResult;

        public SuggestionsController(LOBDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Suggestions
        [HttpGet]
        public async Task<IActionResult> GetSuggestions()
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

            List<SuggestionResponseDTO> SuggestionResponseDTOs = new List<SuggestionResponseDTO>();
            var fooList = await _context.Suggestions.Include(x => x.User)
                .Where(x => x.User.Id == fooUser.Id).OrderByDescending(x => x.SubmitTime).Take(100).ToListAsync();
            foreach (var item in fooList)
            {
                SuggestionResponseDTO fooObject = new SuggestionResponseDTO()
                {
                    Id = item.Id,
                    User = new UserDTO()
                    {
                        Id = item.User.Id
                    },
                    Subject = item.Subject,
                    SubmitTime = item.SubmitTime,
                    Message = item.Message,
                };
                SuggestionResponseDTOs.Add(fooObject);
            }
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: SuggestionResponseDTOs);
            return Ok(apiResult);
        }

        // POST: api/Suggestions
        [HttpPost]
        public async Task<IActionResult> PostSuggestion([FromBody] SuggestionRequestDTO suggestionRequestDTO)
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

            Suggestion fooObject = new Suggestion()
            {
                Subject = suggestionRequestDTO.Subject,
                Message = suggestionRequestDTO.Message,
                SubmitTime = suggestionRequestDTO.SubmitTime,
                User = fooUser,
            };
            _context.Suggestions.Add(fooObject);
            await _context.SaveChangesAsync();

            SuggestionResponseDTO SuggestionResponseDTO = new SuggestionResponseDTO()
            {
                Subject = fooObject.Subject,
                Message = fooObject.Message,
                SubmitTime = fooObject.SubmitTime,
                User = new UserDTO() { Id = fooUser.Id },
            };
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: SuggestionResponseDTO);
            return Ok(apiResult);
        }
    }
}