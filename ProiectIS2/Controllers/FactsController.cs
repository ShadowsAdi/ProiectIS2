using Microsoft.AspNetCore.Mvc;
using ProiectIS2.Models.DataTransferObjects.FactsDTO;
using ProiectIS2.Models.DataTransferObjects.Pagination;
using ProiectIS2.Services.Abstractions;

namespace ProiectIS2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactsController(IObjectService<FactRecord, SearchPaginationQueryParams, FactAddRecord, FactUpdateRecord> factService) : ControllerBase
    {

        // GET: api/Facts
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FactRecord>>> GetCatFacts([FromQuery]SearchPaginationQueryParams queryParams)
        {
            return Ok(await factService.GetObjects(queryParams));
        }

        // GET: api/Facts/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FactRecord>> GetFacts([FromRoute]int id)
        {
            var fact = await factService.GetObject(id);

            return fact == null ? NotFound() : Ok(fact);
        }

        // PUT: api/Facts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFacts([FromBody] FactUpdateRecord fact)
        {
            await factService.UpdateObject(fact);
        
            return Ok();
        }

        // POST: api/Facts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<FactRecord>> PostFacts([FromBody]FactAddRecord fact)
        {
            var entId = await factService.AddObject(fact);
        
            return CreatedAtAction(nameof(GetFacts), new { id = entId }, new { Id = entId });
        }

        // DELETE: api/Facts/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteFact([FromRoute]int id)
        {
            await factService.DeleteObject(id);
        
            return Accepted();
        }
    }
}
