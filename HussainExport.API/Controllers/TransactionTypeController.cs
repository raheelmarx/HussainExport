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
    public class TransactionTypeController : ControllerBase
    {
        private readonly HEDBContext _context;

        public TransactionTypeController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/TransactionType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionType>>> GetTransactionTypes()
        {
            return await _context.TransactionTypes.ToListAsync();
        }

        // GET: api/TransactionType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionType>> GetTransactionType(long id)
        {
            var transactionType = await _context.TransactionTypes.FindAsync(id);

            if (transactionType == null)
            {
                return NotFound();
            }

            return transactionType;
        }

        // PUT: api/TransactionType/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionType(long id, TransactionType transactionType)
        {
            if (id != transactionType.TransactionTypeId)
            {
                return BadRequest();
            }

            _context.Entry(transactionType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionTypeExists(id))
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

        // POST: api/TransactionType
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransactionType>> PostTransactionType(TransactionType transactionType)
        {
            _context.TransactionTypes.Add(transactionType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactionType", new { id = transactionType.TransactionTypeId }, transactionType);
        }

        // DELETE: api/TransactionType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionType(long id)
        {
            var transactionType = await _context.TransactionTypes.FindAsync(id);
            if (transactionType == null)
            {
                return NotFound();
            }

            _context.TransactionTypes.Remove(transactionType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionTypeExists(long id)
        {
            return _context.TransactionTypes.Any(e => e.TransactionTypeId == id);
        }
    }
}
