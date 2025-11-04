using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectIS2.Contexts;
using ProiectIS2.Models;

namespace ProiectIS2.Controllers
{
    //[ApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class FactsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Facts
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Facts>>> GetCatFacts()
        {
            return await _context.CatFacts.ToListAsync();
        }

        // GET: api/Facts/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Facts>> GetFacts(int id)
        {
            var facts = await _context.CatFacts.FindAsync(id);

            if (facts == null)
            {
                return NotFound();
            }

            return facts;
        }

        // PUT: api/Facts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFacts(int id, Facts facts)
        {
            if (id != facts.Id)
            {
                return BadRequest();
            }

            _context.Entry(facts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactsExists(id))
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

        // POST: api/Facts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Facts>> PostFacts(Facts facts)
        {
            _context.CatFacts.Add(facts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFacts", new { id = facts.Id }, facts);
        }

        // DELETE: api/Facts/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteFacts(int id)
        {
            var facts = await _context.CatFacts.FindAsync(id);
            if (facts == null)
            {
                return NotFound();
            }

            _context.CatFacts.Remove(facts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FactsExists(int id)
        {
            return _context.CatFacts.Any(e => e.Id == id);
        }
    }
}
