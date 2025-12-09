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
using ProiectIS2.Models.DTOs.Pagination;
using ProiectIS2.Services.Abstractions;
using ProiectIS2.Services.Implementations;

namespace ProiectIS2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatResponseController(IObjectService<CatImgResponsesRecord, PaginationQueryParams, CatImgResponseAddRecord, CatImgResponseUpdateRecord> catImgResponsesService) : ControllerBase
    {
        // GET: api/CatResponse
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResponse<CatImgResponses>>> GetCatImgResponses([FromQuery]PaginationQueryParams queryParams)
        {
            return Ok(await catImgResponsesService.GetObjects(queryParams));
        }
        
        // GET: api/CatResponse/404
        [HttpGet("{responseCode:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CatImgResponses>> GetCatImgResponse([FromRoute]int responseCode)
        {
            var catImgResponses = await catImgResponsesService.GetObject(responseCode);

            return catImgResponses == null ? NotFound() : Ok(catImgResponses);
        }
        
        // GET: api/CatResponse/404.jpg
        [HttpGet("{responseCode:int}.jpg")]
        [Produces("image/jpeg")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCatImgResponsesJpg([FromRoute]int responseCode)
        {
            var catImgResponses = await catImgResponsesService.GetObject(responseCode);

            if (catImgResponses == null)
            {
                return NotFound();
            }
            
            string filePath = catImgResponses.DataLocation;

            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"Error: File not found at path: {filePath}");
                return NotFound();
            }

            try
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                
                return File(fileBytes, "image/jpeg");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading file from path {filePath}: {ex.Message}");
                return StatusCode(500, "Could not read the image file from the server.");
            }
        }

        // PUT: api/CatResponse/200
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCatImgResponses(
            [FromForm] CatImgResponseUpdateRecord catImgResponsesUpdate)
        {
            try
            {
                await catImgResponsesService.UpdateObject(catImgResponsesUpdate);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Image {catImgResponsesUpdate.ResponseCode} not found on server. Upload it first.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating image.");
            }
        }

        // POST: api/CatResponse
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostCatImgResponses(
            [FromForm] CatImgResponseAddRecord catImgResponsesRecord) 
        {
            try
            {
                await catImgResponsesService.AddObject(catImgResponsesRecord);

            
                return CreatedAtAction(nameof(GetCatImgResponsesJpg), 
                    new { responseCode = catImgResponsesRecord.ResponseCode }, 
                    null);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during image upload: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error processing file upload.");
            }
        }

        // DELETE: api/CatResponse/404
        [HttpDelete("{responseCode:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCatImgResponses(int responseCode)
        {
            await catImgResponsesService.DeleteObject(responseCode);
            
            return Ok();
        }
    }
}
