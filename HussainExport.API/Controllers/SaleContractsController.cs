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
    public class SaleContractsController : ControllerBase
    {
        private readonly HEDBContext _context;

        public SaleContractsController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/SaleContracts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleContract>>> GetSaleContract()
        {
            return await _context.SaleContract.ToListAsync();
        }

        // GET: api/SaleContracts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleContract>> GetSaleContract(long id)
        {
            var saleContract = await _context.SaleContract.FindAsync(id);

            if (saleContract == null)
            {
                return NotFound();
            }

            return saleContract;
        }

        // PUT: api/SaleContracts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleContract(long id, SaleContract saleContract)
        {
            if (id != saleContract.SaleContractId)
            {
                return BadRequest();
            }

            _context.Entry(saleContract).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleContractExists(id))
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

        // POST: api/SaleContracts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SaleContract>> PostSaleContract(SaleContract saleContract)
        {
            _context.SaleContract.Add(saleContract);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSaleContract", new { id = saleContract.SaleContractId }, saleContract);
        }

        // DELETE: api/SaleContracts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SaleContract>> DeleteSaleContract(long id)
        {
            var saleContract = await _context.SaleContract.FindAsync(id);
            if (saleContract == null)
            {
                return NotFound();
            }

            _context.SaleContract.Remove(saleContract);
            await _context.SaveChangesAsync();

            return saleContract;
        }

        private bool SaleContractExists(long id)
        {
            return _context.SaleContract.Any(e => e.SaleContractId == id);
        }
    }
}
