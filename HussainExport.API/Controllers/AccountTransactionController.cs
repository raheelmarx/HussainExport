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
    public class AccountTransactionController : ControllerBase
    {
        private readonly HEDBContext _context;

        public AccountTransactionController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/AccountTransaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountTransaction>>> GetAccountTransactions()
        {
            return await _context.AccountTransactions.OrderByDescending(x => x.DateAdded).Include(x=>x.AccountCredit).Include(a => a.AccountDebit).Include(a => a.SaleContract).Include(a => a.TypeNavigation).ToListAsync();
        }

        // GET: api/AccountTransaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountTransaction>> GetAccountTransaction(long id)
        {
            //var accountTransaction = await _context.AccountTransactions.FindAsync(id);
            var accountTransaction = await _context.AccountTransactions.Include(x => x.AccountCredit).Include(a => a.AccountDebit).Include(a => a.SaleContract).Include(a => a.TypeNavigation).FirstOrDefaultAsync(x => x.AccountTransactionId == id);

            if (accountTransaction == null)
            {
                return NotFound();
            }

            return accountTransaction;
        }

        // PUT: api/AccountTransaction/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountTransaction(long id, AccountTransaction accountTransaction)
        {
            if (id != accountTransaction.AccountTransactionId)
            {
                return BadRequest();
            }

            _context.Entry(accountTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountTransactionExists(id))
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

        // POST: api/AccountTransaction
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountTransaction>> PostAccountTransaction(AccountTransaction accountTransaction)
        {
            _context.AccountTransactions.Add(accountTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccountTransaction", new { id = accountTransaction.AccountTransactionId }, accountTransaction);
        }

        // DELETE: api/AccountTransaction/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountTransaction(long id)
        {
            var accountTransaction = await _context.AccountTransactions.FindAsync(id);
            if (accountTransaction == null)
            {
                return NotFound();
            }

            _context.AccountTransactions.Remove(accountTransaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountTransactionExists(long id)
        {
            return _context.AccountTransactions.Any(e => e.AccountTransactionId == id);
        }
    }
}
