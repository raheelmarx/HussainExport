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
    public class PayableController : ControllerBase
    {
        private readonly HEDBContext _context;

        public PayableController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/Payable
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payable>>> GetPayables()
        {
            return await _context.Payables.ToListAsync();
        }

        // GET: api/Payable/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payable>> GetPayable(long id)
        {
            var payable = await _context.Payables.FindAsync(id);

            if (payable == null)
            {
                return NotFound();
            }

            return payable;
        }

        // PUT: api/Payable/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayable(long id, Payable payable)
        {
            if (id != payable.PayableId)
            {
                return BadRequest();
            }

            _context.Entry(payable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayableExists(id))
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

        // POST: api/Payable
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Payable>> PostPayable(Payable payable)
        {
            _context.Payables.Add(payable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayable", new { id = payable.PayableId }, payable);
        }

        // DELETE: api/Payable/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayable(long id)
        {
            var payable = await _context.Payables.FindAsync(id);
            if (payable == null)
            {
                return NotFound();
            }

            _context.Payables.Remove(payable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PayableExists(long id)
        {
            return _context.Payables.Any(e => e.PayableId == id);
        }
    }
}
