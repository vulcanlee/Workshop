using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOBCore.Models;

namespace LOBCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LobUsersController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;

        public LobUsersController(LOBDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/LobUsers
        [HttpGet]
        public IEnumerable<LobUser> GetLobUsers()
        {
            return _context.LobUsers;
        }

        // GET: api/LobUsers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLobUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lobUser = await _context.LobUsers.FindAsync(id);

            if (lobUser == null)
            {
                return NotFound();
            }

            return Ok(lobUser);
        }

        // PUT: api/LobUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLobUser([FromRoute] int id, [FromBody] LobUser lobUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lobUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(lobUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LobUserExists(id))
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

        // POST: api/LobUsers
        [HttpPost]
        public async Task<IActionResult> PostLobUser([FromBody] LobUser lobUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.LobUsers.Add(lobUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLobUser", new { id = lobUser.Id }, lobUser);
        }

        // DELETE: api/LobUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLobUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lobUser = await _context.LobUsers.FindAsync(id);
            if (lobUser == null)
            {
                return NotFound();
            }

            _context.LobUsers.Remove(lobUser);
            await _context.SaveChangesAsync();

            return Ok(lobUser);
        }

        private bool LobUserExists(int id)
        {
            return _context.LobUsers.Any(e => e.Id == id);
        }
    }
}