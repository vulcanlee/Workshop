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
using Newtonsoft.Json;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        int UserID;
        APIResult apiResult;

        public InvoicesController(LOBDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<IActionResult> Get()
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

            List<InvoiceResponseDTO> fooInvoiceResponseDTO = new List<InvoiceResponseDTO>();
            foreach (var item in _context.Invoices
                .Include(x => x.User).ThenInclude(x => x.Department)
                .Where(x => x.User.Id == UserID))
            {
                InvoiceResponseDTO fooObject = new InvoiceResponseDTO()
                {
                    Id = item.Id,
                    InvoiceNo = item.InvoiceNo,
                    Date = item.Date,
                    Memo = item.Memo,
                    user = new UserDTO() { Id = item.User.Id }
                };
                fooInvoiceResponseDTO.Add(fooObject);
            }
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: fooInvoiceResponseDTO);
            return Ok(apiResult);
        }

        // PUT: api/Invoices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] InvoiceRequestDTO invoiceRequestDTO)
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

            if (invoiceRequestDTO.Id != id)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.紀錄更新所指定ID不一致);
                return BadRequest(apiResult);
            }

            var invoiceOnDB = await _context.Invoices
                .Include(x => x.User).ThenInclude(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == invoiceRequestDTO.Id);

            if (invoiceOnDB == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的發票);
                return NotFound(apiResult);
            }
            else if (invoiceOnDB.User.Id != UserID)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.權杖Token上標示的使用者與傳送過來的使用者不一致);
                return BadRequest(apiResult);
            }

            invoiceOnDB.InvoiceNo = invoiceRequestDTO.InvoiceNo;
            invoiceOnDB.Date = invoiceRequestDTO.Date;
            invoiceOnDB.Memo = invoiceRequestDTO.Memo;
            _context.Entry(invoiceOnDB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await InvoiceExists(invoiceRequestDTO.Id))
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

            var fooItem = await _context.Invoices.FirstOrDefaultAsync(x => x.Id == invoiceRequestDTO.Id);
            InvoiceResponseDTO invoiceResponseDTO = new InvoiceResponseDTO()
            {
                Id = fooItem.Id,
                InvoiceNo = fooItem.InvoiceNo,
                Date = fooItem.Date,
                Memo = fooItem.Memo,
                user = new UserDTO()
                {
                    Id = fooUser.Id
                }
            };
            apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
               ErrorMessageEnum.None, payload: invoiceResponseDTO);
            return Accepted(apiResult);
        }

        // POST: api/Invoices
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InvoiceRequestDTO invoiceRequestDTO)
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

            Invoice fooInvoice = new Invoice()
            {
                InvoiceNo = invoiceRequestDTO.InvoiceNo,
                Date = invoiceRequestDTO.Date,
                Memo = invoiceRequestDTO.Memo,
                User = fooUser
            };
            _context.Invoices.Add(fooInvoice);
            await _context.SaveChangesAsync();
            var fooItem = await _context.Invoices.FirstOrDefaultAsync(x => x.InvoiceNo == invoiceRequestDTO.InvoiceNo);
            InvoiceResponseDTO invoiceResponseDTO = new InvoiceResponseDTO()
            {
                Id = fooItem.Id,
                InvoiceNo = fooItem.InvoiceNo,
                Date = fooItem.Date,
                Memo = fooItem.Memo,
                user = new UserDTO()
                {
                    Id = fooUser.Id
                }
            };

            apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
               ErrorMessageEnum.None, payload: invoiceResponseDTO);
            return Accepted(apiResult);
        }

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
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

            var invoiceResponseDTO = await _context.Invoices
                .Include(x => x.User).ThenInclude(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (invoiceResponseDTO == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的發票);
                return NotFound(apiResult);
            }
            else if (invoiceResponseDTO.User.Id != UserID)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.權杖Token上標示的使用者與傳送過來的使用者不一致);
                return BadRequest(apiResult);
            }

            _context.Invoices.Remove(invoiceResponseDTO);
            await _context.SaveChangesAsync();

            InvoiceResponseDTO fooInvoiceResponseDTO = new InvoiceResponseDTO()
            {
                Id = invoiceResponseDTO.Id,
                Date = invoiceResponseDTO.Date,
                InvoiceNo = invoiceResponseDTO.InvoiceNo,
                Memo = invoiceResponseDTO.Memo,
                user = new UserDTO() { Id = fooUser.Id }
            };
            apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
               ErrorMessageEnum.None, payload: fooInvoiceResponseDTO);
            return Accepted(apiResult);
        }

        private async Task<bool> InvoiceExists(int id)
        {
            return await _context.Invoices.AnyAsync(e => e.Id == id);
        }
    }
}