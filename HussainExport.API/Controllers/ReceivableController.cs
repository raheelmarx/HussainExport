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
    public class ReceivableController : ControllerBase
    {
        private readonly HEDBContext _context;

        public ReceivableController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/Receivable
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receivable>>> GetReceivables()
        {
            return await _context.Receivables.ToListAsync();
        }

        // GET: api/Receivable/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Receivable>> GetReceivable(long id)
        {
            var receivable = await _context.Receivables.FindAsync(id);

            if (receivable == null)
            {
                return NotFound();
            }

            return receivable;
        }

        // PUT: api/Receivable/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReceivable(long id, Receivable receivable)
        {
            if (id != receivable.ReceivableId)
            {
                return BadRequest();
            }

            _context.Entry(receivable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceivableExists(id))
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

        // POST: api/Receivable
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Receivable>> PostReceivable(Receivable receivable)
        {
            _context.Receivables.Add(receivable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReceivable", new { id = receivable.ReceivableId }, receivable);
        }

        // DELETE: api/Receivable/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceivable(long id)
        {
            var receivable = await _context.Receivables.FindAsync(id);
            if (receivable == null)
            {
                return NotFound();
            }

            _context.Receivables.Remove(receivable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReceivableExists(long id)
        {
            return _context.Receivables.Any(e => e.ReceivableId == id);
        }
    }
}
