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
    public class NotificationTokensController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;

        public NotificationTokensController(LOBDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/NotificationTokens
        [HttpGet]
        public IEnumerable<NotificationToken> GetNotificationTokens()
        {
            return _context.NotificationTokens;
        }

        // GET: api/NotificationTokens/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationToken([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notificationToken = await _context.NotificationTokens.FindAsync(id);

            if (notificationToken == null)
            {
                return NotFound();
            }

            return Ok(notificationToken);
        }

        // PUT: api/NotificationTokens/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotificationToken([FromRoute] int id, [FromBody] NotificationToken notificationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notificationToken.Id)
            {
                return BadRequest();
            }

            _context.Entry(notificationToken).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationTokenExists(id))
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

        // POST: api/NotificationTokens
        [HttpPost]
        public async Task<IActionResult> PostNotificationToken([FromBody] NotificationToken notificationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.NotificationTokens.Add(notificationToken);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotificationToken", new { id = notificationToken.Id }, notificationToken);
        }

        // DELETE: api/NotificationTokens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotificationToken([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notificationToken = await _context.NotificationTokens.FindAsync(id);
            if (notificationToken == null)
            {
                return NotFound();
            }

            _context.NotificationTokens.Remove(notificationToken);
            await _context.SaveChangesAsync();

            return Ok(notificationToken);
        }

        private bool NotificationTokenExists(int id)
        {
            return _context.NotificationTokens.Any(e => e.Id == id);
        }
    }
}