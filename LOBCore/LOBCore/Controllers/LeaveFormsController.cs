using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOBCore.DataAccesses;
using LOBCore.DataAccesses.Entities;
using LOBCore.DTOs;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveFormsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        private readonly APIResult apiResult;
        int UserID;

        public LeaveFormsController(LOBDatabaseContext context, APIResult apiResult)
        {
            _context = context;
            this.apiResult = apiResult;
        }

        // GET: api/LeaveForms
        [HttpGet]
        public IEnumerable<LeaveFormResponseDTO> GetLeaveForms()
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            List<LeaveFormResponseDTO> fooLeaveFormResponseDTO = new List<LeaveFormResponseDTO>();
            foreach (var item in _context.LeaveForms.Include(x => x.LeaveFormType)
                .Include(x => x.User).ThenInclude(x => x.Department)
                .Where(x => x.User.Id == UserID))
            {
                LeaveFormResponseDTO fooObject = new LeaveFormResponseDTO()
                {
                    BeginTime = item.BeginTime,
                    EndTime = item.EndTime,
                    Description = item.Description,
                    Id = item.Id,
                    TotalHours = item.TotalHours,
                    user = new UserDTO()
                    {
                        Id = item.User.Id,
                        Department = new DepartmentDTO()
                        {
                            Id = item.User.Department.Id
                        }
                    },
                    leaveFormType = new LeaveFormTypeDTO()
                    {
                        Id = item.LeaveFormType.Id
                    }
                };
                fooLeaveFormResponseDTO.Add(fooObject);
            }
            return fooLeaveFormResponseDTO;
        }

        // GET: api/LeaveForms/5
        [HttpGet("{id}")]
        public async Task<APIResult> GetLeaveForm([FromRoute] int id)
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            if (!ModelState.IsValid)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"傳送過來的資料有問題 {ModelState}";
                return apiResult;
            }

            var leaveForm = await _context.LeaveForms.Include(x => x.LeaveFormType)
                .Include(x => x.User).ThenInclude(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (leaveForm == null)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"沒有發現指定的請假單";
                return apiResult;
            }
            else if (leaveForm.User.Id != UserID)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"你沒有權限刪除其他人的請假單";
                return apiResult;
            }

            apiResult.Payload = new LeaveFormResponseDTO()
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
            return apiResult;
        }

        // PUT: api/LeaveForms/5
        [HttpPut]
        public async Task<APIResult> PutLeaveForm([FromBody] LeaveFormRequestDTO leaveForm)
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            if (!ModelState.IsValid)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"傳送過來的資料有問題 {ModelState}";
                return apiResult;
            }

            var leaveFormOnDB = await _context.LeaveForms.Include(x => x.LeaveFormType)
                .Include(x => x.User).ThenInclude(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == leaveForm.id);

            if (leaveFormOnDB == null)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"沒有發現指定的請假單";
                return apiResult;
            }
            else if (leaveFormOnDB.User.Id != UserID)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"你沒有權限修改其他人的請假單";
                return apiResult;
            }

            var fooLeaveFormType = await _context.LeaveFormTypes.FindAsync(leaveForm.leaveFormType.Id);
            if (fooLeaveFormType == null)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"沒有發現指定的請假單類別";
                return apiResult;
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
                    apiResult.Status = APIResultStatus.Failure;
                    apiResult.Message = $"要更新的請假單紀錄，發生同時存取衝突，已經不存在資料庫上";
                    return apiResult;
                }
                else
                {
                    apiResult.Status = APIResultStatus.Failure;
                    apiResult.Message = $"紀錄更新時，發生同時存取衝突";
                    return apiResult;
                }
            } catch(Exception ex)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"紀錄更新時，發生例外異常 {ex.Message}";
                return apiResult;
            }

            apiResult.Payload = leaveForm;
            return apiResult;
        }

        // POST: api/LeaveForms
        [HttpPost]
        public async Task<APIResult> PostLeaveForm([FromBody] LeaveFormRequestDTO leaveForm)
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"傳送過來的資料有問題 {ModelState}";
                return apiResult;
            }
            var fooUser = await _context.LobUsers.FirstOrDefaultAsync(x => x.Id == UserID);
            if (fooUser != null)
            {
                var fooLeaveFormType = await _context.LeaveFormTypes.FirstOrDefaultAsync(x => x.Id == leaveForm.leaveFormType.Id);
                if (fooLeaveFormType != null)
                {
                    LeaveForm fooLeaveForm = new LeaveForm()
                    {
                        BeginTime = leaveForm.BeginTime,
                        EndTime = leaveForm.EndTime,
                        Description = leaveForm.Description,
                        TotalHours = leaveForm.TotalHours,
                        User = fooUser,
                        LeaveFormType = fooLeaveFormType,
                    };
                    _context.LeaveForms.Add(fooLeaveForm);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    apiResult.Status = APIResultStatus.Failure;
                    apiResult.Message = "沒有發現指定的請假類別";
                }
            }
            else
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = "沒有發現指定的使用者";
            }


            return apiResult;
        }

        // DELETE: api/LeaveForms/5
        [HttpDelete("{id}")]
        public async Task<APIResult> DeleteLeaveForm([FromRoute] int id)
        {
            UserID = Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value);
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"傳送過來的資料有問題 {ModelState}";
                return apiResult;
            }

            var leaveForm = await _context.LeaveForms.Include(x=>x.LeaveFormType)
                .Include(x => x.User).ThenInclude(x=>x.Department)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (leaveForm == null)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"沒有發現指定的請假單";
                return apiResult;
            }
            else if (leaveForm.User.Id != UserID)
            {
                apiResult.Status = APIResultStatus.Failure;
                apiResult.Message = $"你沒有權限刪除其他人的請假單";
                return apiResult;
            }

            _context.LeaveForms.Remove(leaveForm);
            await _context.SaveChangesAsync();

            apiResult.Payload = new LeaveFormResponseDTO()
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
            return apiResult;
        }

        private async Task<bool> LeaveFormExists(int id)
        {
            return await _context.LeaveForms.AnyAsync(e => e.Id == id);
        }
    }
}