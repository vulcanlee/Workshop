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

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionRecordsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;

        public ExceptionRecordsController(LOBDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ExceptionRecords
        [HttpGet]
        public IEnumerable<ExceptionRecord> GetExceptionRecords()
        {
            return _context.ExceptionRecords;
        }

        // GET: api/ExceptionRecords/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExceptionRecord([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exceptionRecord = await _context.ExceptionRecords.FindAsync(id);

            if (exceptionRecord == null)
            {
                return NotFound();
            }

            return Ok(exceptionRecord);
        }

        // PUT: api/ExceptionRecords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExceptionRecord([FromRoute] int id, [FromBody] ExceptionRecord exceptionRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exceptionRecord.Id)
            {
                return BadRequest();
            }

            _context.Entry(exceptionRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExceptionRecordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ExceptionRecords
        [HttpPost]
        public async Task<IActionResult> PostExceptionRecord([FromBody] ExceptionRecord exceptionRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ExceptionRecords.Add(exceptionRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExceptionRecord", new { id = exceptionRecord.Id }, exceptionRecord);
        }

        // DELETE: api/ExceptionRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExceptionRecord([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exceptionRecord = await _context.ExceptionRecords.FindAsync(id);
            if (exceptionRecord == null)
            {
                return NotFound();
            }

            _context.ExceptionRecords.Remove(exceptionRecord);
            await _context.SaveChangesAsync();

            return Ok(exceptionRecord);
        }

        private bool ExceptionRecordExists(int id)
        {
            return _context.ExceptionRecords.Any(e => e.Id == id);
        }
    }
}