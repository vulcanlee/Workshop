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
    public class InvoiceDetailsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        int UserID;
        APIResult apiResult;

        public InvoiceDetailsController(LOBDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/InvoiceDetails/5
        [HttpGet("{invoiceid}")]
        public async Task<IActionResult> Get([FromRoute] int invoiceid)
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

            var invoiceOnDB = await _context.Invoices
                .Include(x=>x.Details)
                .FirstOrDefaultAsync(x => x.Id == invoiceid);

            if (invoiceOnDB == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的發票);
                return NotFound(apiResult);
            }

            List<InvoiceDetailResponseDTO> fooInvoiceDetailResponseDTO = new List<InvoiceDetailResponseDTO>();
            foreach (var item in invoiceOnDB.Details)
            {
                InvoiceDetailResponseDTO fooObject = new InvoiceDetailResponseDTO()
                {
                    Id = item.Id,
                    Invoice = new InvoiceDTO() { Id = item.Invoice.Id },
                    Cnt = item.Cnt,
                    Flag = item.Flag,
                    Memo = item.Memo,
                    PictureName = item.PictureName,
                    Price = item.Price,
                    Qty = item.Qty,
                    SubTotal = item.SubTotal,
                    TDate = item.TDate,
                };
                fooInvoiceDetailResponseDTO.Add(fooObject);
            }
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: fooInvoiceDetailResponseDTO);
            return Ok(apiResult);
        }

        // PUT: api/InvoiceDetails/5/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] InvoiceDetailRequestDTO invoiceDetailRequestDTO)
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

            if (invoiceDetailRequestDTO.Id != id)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                 ErrorMessageEnum.紀錄更新所指定ID不一致);
                return BadRequest(apiResult);
            }

            var invoiceOnDB = await _context.Invoices
                .FirstOrDefaultAsync(x => x.Id == invoiceDetailRequestDTO.Invoice.Id);

            if (invoiceOnDB == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的發票);
                return NotFound(apiResult);
            }

            var invoiceDetailOnDB = await _context.InvoiceDetails
                .Include(x=>x.Invoice)
                .FirstOrDefaultAsync(x => x.Id == invoiceDetailRequestDTO.Id);

            if (invoiceDetailOnDB == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的發票明細項目);
                return NotFound(apiResult);
            }

            invoiceDetailOnDB.Cnt = invoiceDetailRequestDTO.Cnt;
            invoiceDetailOnDB.Flag = invoiceDetailRequestDTO.Flag;
            invoiceDetailOnDB.Memo = invoiceDetailRequestDTO.Memo;
            invoiceDetailOnDB.PictureName = invoiceDetailRequestDTO.PictureName;
            invoiceDetailOnDB.Price = invoiceDetailRequestDTO.Price;
            invoiceDetailOnDB.Qty = invoiceDetailRequestDTO.Qty;
            invoiceDetailOnDB.SubTotal = invoiceDetailRequestDTO.SubTotal;
            invoiceDetailOnDB.TDate = invoiceDetailRequestDTO.TDate;
            _context.Entry(invoiceOnDB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await InvoiceDetailExists(invoiceDetailRequestDTO.Id))
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
               ErrorMessageEnum.None, payload: invoiceDetailRequestDTO);
            return Accepted(apiResult);
        }

        // POST: api/InvoiceDetails
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InvoiceDetailRequestDTO invoiceDetailRequestDTO)
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

            var invoiceOnDB = await _context.Invoices
                .FirstOrDefaultAsync(x => x.Id == invoiceDetailRequestDTO.Invoice.Id);

            if (invoiceOnDB == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的發票);
                return NotFound(apiResult);
            }

            InvoiceDetail fooInvoiceDetail = new InvoiceDetail()
            {
                Cnt = invoiceDetailRequestDTO.Cnt,
                Flag = invoiceDetailRequestDTO.Flag,
                Memo = invoiceDetailRequestDTO.Memo,
                PictureName = invoiceDetailRequestDTO.PictureName,
                Price = invoiceDetailRequestDTO.Price,
                Qty = invoiceDetailRequestDTO.Qty,
                SubTotal = invoiceDetailRequestDTO.SubTotal,
                TDate = invoiceDetailRequestDTO.TDate,
                Invoice = invoiceOnDB
            };
            _context.InvoiceDetails.Add(fooInvoiceDetail);
            await _context.SaveChangesAsync();

            apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
               ErrorMessageEnum.None, payload: invoiceDetailRequestDTO);
            return Accepted(apiResult);
        }

        // DELETE: api/InvoiceDetails/5
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

            var fooInvoiceDetail = await _context.InvoiceDetails
                .Include(x => x.Invoice)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (fooInvoiceDetail == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status404NotFound,
                 ErrorMessageEnum.沒有發現指定的發票明細項目);
                return NotFound(apiResult);
            }

            _context.InvoiceDetails.Remove(fooInvoiceDetail);
            await _context.SaveChangesAsync();

            InvoiceDetailResponseDTO fooInvoiceDetailResponseDTO = new InvoiceDetailResponseDTO()
            {
                Id = fooInvoiceDetail.Id,
                Cnt = fooInvoiceDetail.Cnt,
                Flag = fooInvoiceDetail.Flag,
                PictureName = fooInvoiceDetail.PictureName,
                Price = fooInvoiceDetail.Price,
                Qty = fooInvoiceDetail.Qty,
                SubTotal = fooInvoiceDetail.SubTotal,
                TDate = fooInvoiceDetail.TDate,
                Memo = fooInvoiceDetail.Memo,
                Invoice = new InvoiceDTO() { Id = fooUser.Id }
            };
            apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
               ErrorMessageEnum.None, payload: fooInvoiceDetailResponseDTO);
            return Accepted(apiResult);
        }

        private async Task<bool> InvoiceDetailExists(int id)
        {
            return await _context.InvoiceDetails.AnyAsync(e => e.Id == id);
        }
    }
}