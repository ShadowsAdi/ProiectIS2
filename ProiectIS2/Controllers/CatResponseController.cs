using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectIS2.Contexts;
using ProiectIS2.Models.Domain;
using ProiectIS2.Models.DTOs;

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
        
        [HttpGet("{ResponseCode}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CatImgResponses>> GetCatImgResponses(int ResponseCode)
        {
            var catImgResponses = await _context.CatImgResponses.FindAsync(ResponseCode);

            if (catImgResponses == null)
            {
                return NotFound();
            }

            return catImgResponses;
        }
        
        // GET: api/CatResponse/5
        
        [HttpGet("{ResponseCode}.jpg")]
        [Produces("image/jpeg")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCatImgResponsesJpg(int ResponseCode)
        {
            var catImgResponses = await _context.CatImgResponses.FindAsync(ResponseCode);

            if (catImgResponses == null)
            {
                return NotFound();
            }

            return File(catImgResponses.Data, "image/jpeg");
        }

        // PUT: api/CatResponse/200
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPut("{ResponseCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCatImgResponses([FromRoute] int ResponseCode, [FromForm] CatImgResponsesDto catImgResponses)
        {
            var catImg = await _context.Set<CatImgResponses>().FirstOrDefaultAsync(e => e.ResponseCode == ResponseCode);

            if (catImg == null)
            {
                return NotFound();
            }

            using var memoryStream = new MemoryStream();
            await catImgResponses.Data.CopyToAsync(memoryStream);
            catImg.Data = memoryStream.ToArray();
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatImgResponsesExists(ResponseCode))
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

            return CreatedAtAction("GetCatImgResponses", new { catImgResponses.ResponseCode }, catImgResponses);
        }

        // DELETE: api/CatResponse/5
        [HttpDelete("{ResponseCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCatImgResponses(int ResponseCode)
        {
            var catImgResponses = await _context.CatImgResponses.FindAsync(ResponseCode);
            if (catImgResponses == null)
            {
                return NotFound();
            }

            _context.CatImgResponses.Remove(catImgResponses);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CatImgResponsesExists(int ResponseCode)
        {
            return _context.CatImgResponses.Any(e => e.ResponseCode == ResponseCode);
        }
    }
}
