using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectIS2.Contexts;
using ProiectIS2.Models.Domain;

namespace ProiectIS2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatResponseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CatResponseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CatResponse
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CatImgResponses>>> GetCatImgResponses()
        {
            return await _context.CatImgResponses.ToListAsync();
        }
        
        // GET: api/CatResponse/5
        
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CatImgResponses>> GetCatImgResponses(int id)
        {
            var catImgResponses = await _context.CatImgResponses.FindAsync(id);

            if (catImgResponses == null)
            {
                return NotFound();
            }

            return catImgResponses;
        }
        
        // GET: api/CatResponse/5
        
        [HttpGet("{id}.jpg")]
        [Produces("image/jpeg")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCatImgResponsesJpg(int id)
        {
            var catImgResponses = await _context.CatImgResponses.FindAsync(id);

            if (catImgResponses == null)
            {
                return NotFound();
            }

            return File(catImgResponses.Data, "image/jpeg");
        }

        // PUT: api/CatResponse/200
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCatImgResponses(int id, CatImgResponses catImgResponses)
        {
            if (id != catImgResponses.ResponseCode)
            {
                return BadRequest();
            }

            _context.Entry(catImgResponses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatImgResponsesExists(id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // POST: api/CatResponse
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CatImgResponses>> PostCatImgResponses(CatImgResponses catImgResponses)
        {
            _context.CatImgResponses.Add(catImgResponses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCatImgResponses", new { id = catImgResponses.ResponseCode }, catImgResponses);
        }

        // DELETE: api/CatResponse/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCatImgResponses(int id)
        {
            var catImgResponses = await _context.CatImgResponses.FindAsync(id);
            if (catImgResponses == null)
            {
                return NotFound();
            }

            _context.CatImgResponses.Remove(catImgResponses);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CatImgResponsesExists(int id)
        {
            return _context.CatImgResponses.Any(e => e.ResponseCode == id);
        }
    }
}
