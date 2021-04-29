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
    public class FabricPurchaseController : ControllerBase
    {
        private readonly HEDBContext _context;

        public FabricPurchaseController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/FabricPurchase
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FabricPurchase>>> GetFabricPurchase()
        {
            return await _context.FabricPurchase.ToListAsync();
        }

        // GET: api/FabricPurchase/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FabricPurchase>> GetFabricPurchase(long id)
        {
            var fabricPurchase = await _context.FabricPurchase.FindAsync(id);

            if (fabricPurchase == null)
            {
                return NotFound();
            }

            return fabricPurchase;
        }

        // PUT: api/FabricPurchase/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFabricPurchase(long id, FabricPurchase fabricPurchase)
        {
            if (id != fabricPurchase.FabricPurchaseId)
            {
                return BadRequest();
            }

            _context.Entry(fabricPurchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FabricPurchaseExists(id))
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

        // POST: api/FabricPurchase
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FabricPurchase>> PostFabricPurchase(FabricPurchase fabricPurchase)
        {
            _context.FabricPurchase.Add(fabricPurchase);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFabricPurchase", new { id = fabricPurchase.FabricPurchaseId }, fabricPurchase);
        }

        // DELETE: api/FabricPurchase/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabricPurchase(long id)
        {
            var fabricPurchase = await _context.FabricPurchase.FindAsync(id);
            if (fabricPurchase == null)
            {
                return NotFound();
            }

            _context.FabricPurchase.Remove(fabricPurchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FabricPurchaseExists(long id)
        {
            return _context.FabricPurchase.Any(e => e.FabricPurchaseId == id);
        }
    }
}
