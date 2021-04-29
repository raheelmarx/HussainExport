using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HussainExport.API.Entities;

namespace HussainExport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FabricPurchaseItemController : ControllerBase
    {
        private readonly HEDBContext _context;

        public FabricPurchaseItemController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/FabricPurchaseItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FabricPurchaseItem>>> GetFabricPurchaseItem()
        {
            return await _context.FabricPurchaseItem.ToListAsync();
        }

        // GET: api/FabricPurchaseItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FabricPurchaseItem>> GetFabricPurchaseItem(long id)
        {
            var fabricPurchaseItem = await _context.FabricPurchaseItem.FindAsync(id);

            if (fabricPurchaseItem == null)
            {
                return NotFound();
            }

            return fabricPurchaseItem;
        }

        // PUT: api/FabricPurchaseItem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFabricPurchaseItem(long id, FabricPurchaseItem fabricPurchaseItem)
        {
            if (id != fabricPurchaseItem.FabricPurchaseItemId)
            {
                return BadRequest();
            }

            _context.Entry(fabricPurchaseItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FabricPurchaseItemExists(id))
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

        // POST: api/FabricPurchaseItem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FabricPurchaseItem>> PostFabricPurchaseItem(FabricPurchaseItem fabricPurchaseItem)
        {
            _context.FabricPurchaseItem.Add(fabricPurchaseItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFabricPurchaseItem", new { id = fabricPurchaseItem.FabricPurchaseItemId }, fabricPurchaseItem);
        }

        // DELETE: api/FabricPurchaseItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabricPurchaseItem(long id)
        {
            var fabricPurchaseItem = await _context.FabricPurchaseItem.FindAsync(id);
            if (fabricPurchaseItem == null)
            {
                return NotFound();
            }

            _context.FabricPurchaseItem.Remove(fabricPurchaseItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FabricPurchaseItemExists(long id)
        {
            return _context.FabricPurchaseItem.Any(e => e.FabricPurchaseItemId == id);
        }
    }
}
