using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectIS2.Contexts;
using ProiectIS2.Models.Domain;
using ProiectIS2.Models.DTOs;
using TGolla.Swashbuckle.AspNetCore.SwaggerGen;

namespace ProiectIS2.Controllers
{
    //[ApiKey]
    [Route("api/[controller]")]
    [ApiController]
    [SortSwaggerPathsByMethod(1)]
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
        public async Task<ActionResult<IEnumerable<FactReadDto>>> GetCatFacts()
        {
            // Folosim .Include() pentru a încărca relația One-to-Many
            // Și .Select() pentru a mapa manual la DTO
            var facts = await _context.CatFacts
                .Include(f => f.Category) 
                .Where(f => f.DeletedAt == null) // Filtrăm ștergerile logice definite în Facts.cs
                .Select(f => new FactReadDto
                {
                    Id = f.Id,
                    FactText = f.Fact,
                    CategoryName = f.Category != null ? f.Category.Name : "No Category",
                    CreatedDate = f.CreatedAt
                })
                .ToListAsync();

            return Ok(facts);
        }

        // GET: api/Facts/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FactReadDto>> GetFacts(int id)
        {
            var fact = await _context.CatFacts
                .Include(f => f.Category)
                .Where(f => f.DeletedAt == null)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fact == null)
            {
                return NotFound();
            }
            
            // Mapăm manual la DTO
            var factDto = new FactReadDto
            {
                Id = fact.Id,
                FactText = fact.Fact,
                CategoryName = fact.Category != null ? fact.Category.Name : "No Category",
                CreatedDate = fact.CreatedAt
            };

            return Ok(factDto);
            
        }

        // PUT: api/Facts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFacts(int id, FactCreateDto factDto)
        {
            // 1. Căutăm entitatea existentă în bază
            var existingFact = await _context.CatFacts.FindAsync(id);

            if (existingFact == null || existingFact.DeletedAt != null)
            {
                return NotFound();
            }

            // 2. Verificăm dacă noua categorie există (dacă se schimbă)
            if (existingFact.CategoryId != factDto.CategoryId)
            {
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == factDto.CategoryId);
                if (!categoryExists)
                {
                    return BadRequest("Invalid Category ID");
                }
            }

            // 3. Actualizăm proprietățile entității cu datele din DTO
            existingFact.Fact = factDto.Fact;
            existingFact.CategoryId = factDto.CategoryId;
    
            // UpdatedAt este setat automat în ApplicationDbContext.SaveChangesAsync, 
            // dar poți forța modificarea stării dacă e nevoie:
            _context.Entry(existingFact).State = EntityState.Modified;

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
        public async Task<ActionResult<FactReadDto>> PostFacts(FactCreateDto factDto)
        {
            // Verificăm dacă categoria există
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == factDto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Invalid Category ID");
            }

            // Mapăm din DTO în Entitatea de Domeniu
            var newFact = new Facts
            {
                Fact = factDto.Fact,
                CategoryId = factDto.CategoryId,
                CreatedAt = DateTime.UtcNow // Setăm explicit sau lăsăm default-ul din model
            };

            _context.CatFacts.Add(newFact);
            await _context.SaveChangesAsync();

            // Pregătim răspunsul DTO
            // Reîncărcăm categoria pentru a avea numele ei în răspuns (opțional)
            var categoryName = await _context.Categories
                .Where(c => c.Id == factDto.CategoryId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            var responseDto = new FactReadDto
            {
                Id = newFact.Id,
                FactText = newFact.Fact,
                CategoryName = categoryName,
                CreatedDate = newFact.CreatedAt
            };

            return CreatedAtAction("GetFacts", new { id = newFact.Id }, responseDto);
        }

        // DELETE: api/Facts/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteFacts(int id)
        {
            var facts = await _context.CatFacts.FindAsync(id);
    
            // Verificăm dacă există și dacă nu e deja șters
            if (facts == null || facts.DeletedAt != null)
            {
                return NotFound();
            }

            // Remove va declanșa logica din SaveChangesAsync (setarea DeletedAt)
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
