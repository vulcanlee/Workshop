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

namespace LOBCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        private readonly APIResult apiResult;
        int UserID;

        public SuggestionsController(LOBDatabaseContext context, APIResult apiResult)
        {
            _context = context;
            this.apiResult = apiResult;
        }

        // GET: api/Suggestions
        [HttpGet]
        public async Task<APIResult> GetSuggestions()
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            var fooUser = await _context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser == null)
            {
                apiResult.Status = false;
                apiResult.Message = "沒有發現指定的該使用者資料";
                return apiResult;
            }
            List<SuggestionResponseDTO> SuggestionResponseDTO = new List<SuggestionResponseDTO>();
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
                SuggestionResponseDTO.Add(fooObject);
            }
            apiResult.Payload = SuggestionResponseDTO;
            return apiResult;
        }

        // POST: api/Suggestions
        [HttpPost]
        public async Task<APIResult> PostSuggestion([FromBody] SuggestionRequestDTO suggestionRequestDTO)
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            if (!ModelState.IsValid)
            {
                apiResult.Status = false;
                apiResult.Message = $"傳送過來的資料有問題 {ModelState}";
                return apiResult;
            }
            var fooUser = await _context.LobUsers.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser == null)
            {
                apiResult.Status = false;
                apiResult.Message = "沒有發現指定的該使用者資料";
                return apiResult;
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
            apiResult.Payload = SuggestionResponseDTO;
            return apiResult;
        }
    }
}