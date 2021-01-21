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
    public class SaleContractItemsController : ControllerBase
    {
        private readonly HEDBContext _context;

        public SaleContractItemsController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/SaleContractItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleContractItem>>> GetSaleContractItem()
        {
            return await _context.SaleContractItem.ToListAsync();
        }

        // GET: api/SaleContractItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleContractItem>> GetSaleContractItem(long id)
        {
            var saleContractItem = await _context.SaleContractItem.FindAsync(id);

            if (saleContractItem == null)
            {
                return NotFound();
            }

            return saleContractItem;
        }

        // PUT: api/SaleContractItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleContractItem(long id, SaleContractItem saleContractItem)
        {
            if (id != saleContractItem.SaleContractItemId)
            {
                return BadRequest();
            }

            _context.Entry(saleContractItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleContractItemExists(id))
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

        // POST: api/SaleContractItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SaleContractItem>> PostSaleContractItem(SaleContractItem saleContractItem)
        {
            _context.SaleContractItem.Add(saleContractItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSaleContractItem", new { id = saleContractItem.SaleContractItemId }, saleContractItem);
        }

        // DELETE: api/SaleContractItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SaleContractItem>> DeleteSaleContractItem(long id)
        {
            var saleContractItem = await _context.SaleContractItem.FindAsync(id);
            if (saleContractItem == null)
            {
                return NotFound();
            }

            _context.SaleContractItem.Remove(saleContractItem);
            await _context.SaveChangesAsync();

            return saleContractItem;
        }

        private bool SaleContractItemExists(long id)
        {
            return _context.SaleContractItem.Any(e => e.SaleContractItemId == id);
        }
    }
}
