using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dogs.Models;

namespace Dogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogsItemsController : ControllerBase
    {
        private readonly DogsContext _context;

        public DogsItemsController(DogsContext context)
        {
            _context = context;
        }

        // GET: api/DogsItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DogItemDTO>>> GetDogsItem()
        {
            return await _context.DogsItem
             .Select(x => ItemToDTO(x))
             .ToListAsync();
        }

        // GET: api/DogsItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DogItemDTO>> GetDogsItem(long id)
        {
            var dogItem = await _context.DogsItem.FindAsync(id);

            if (dogItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(dogItem);
        }

        // PUT: api/DogsItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDogsItem(long id, DogItemDTO dogItemDTO)
        {
            if (id != dogItemDTO.Id)
            {
                return BadRequest();
            }

            var dogsItem = await _context.DogsItem.FindAsync(id);
            if (dogsItem == null)
            {
                return NotFound();
            }

            dogsItem.Name = dogItemDTO.Name;
            dogsItem.Height = dogItemDTO.Height;
            dogsItem.Weight = dogItemDTO.Weight;
            dogsItem.Breed = dogItemDTO.Breed;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!DogsItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/DogsItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DogItemDTO>> PostDogsItem(DogItemDTO dogItemDTO)
        {
            var dogItem = new DogsItem
            {
                Height = dogItemDTO.Height,
                Name = dogItemDTO.Name,
                Weight = dogItemDTO.Weight,
                Breed = dogItemDTO.Breed,

            };

            _context.DogsItem.Add(dogItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetDogsItem),
                new { id = dogItem.Id },
                ItemToDTO(dogItem));
        }

        // DELETE: api/DogsItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDogsItem(long id)
        {
            var dogsItem = await _context.DogsItem.FindAsync(id);
            if (dogsItem == null)
            {
                return NotFound();
            }

            _context.DogsItem.Remove(dogsItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DogsItemExists(long id)
        {
            return _context.DogsItem.Any(e => e.Id == id);
        }
        private static DogItemDTO ItemToDTO(DogsItem dogsItem)
        {
            return new DogItemDTO
            {
                Id = dogsItem.Id,
                Name = dogsItem.Name,
                Height = dogsItem.Height,
                Weight = dogsItem.Weight,
                Breed = dogsItem.Breed
            };
        }
    }
}

