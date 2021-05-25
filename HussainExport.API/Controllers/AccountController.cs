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
    public class AccountController : ControllerBase
    {
        private readonly HEDBContext _context;

        public AccountController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/Account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblAccount>>> GetTblAccounts()
        {
            return await _context.TblAccounts.ToListAsync();
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblAccount>> GetTblAccount(long id)
        {
            var tblAccount = await _context.TblAccounts.FindAsync(id);

            if (tblAccount == null)
            {
                return NotFound();
            }

            return tblAccount;
        }

        // PUT: api/Account/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblAccount(long id, TblAccount tblAccount)
        {
            if (id != tblAccount.AccountId)
            {
                return BadRequest();
            }

            _context.Entry(tblAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblAccountExists(id))
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

        // POST: api/Account
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblAccount>> PostTblAccount(TblAccount tblAccount)
        {
            _context.TblAccounts.Add(tblAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblAccount", new { id = tblAccount.AccountId }, tblAccount);
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblAccount(long id)
        {
            var tblAccount = await _context.TblAccounts.FindAsync(id);
            if (tblAccount == null)
            {
                return NotFound();
            }

            _context.TblAccounts.Remove(tblAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // GET: api/Account/5
        [HttpGet("Ledger/{id}")]
        public async Task<ActionResult<TblAccount>> Ledger(long id)
        {
            TblAccount tblAccount = new TblAccount();
            try
            {
               // List<AccountTransaction> data = await _context.AccountTransactions.OrderByDescending(x => x.DateAdded).Include(x => x.AccountDebit).Include(x => x.AccountCredit).Where(x => x.AccountDebitId == id || x.AccountCreditId == id).ToListAsync();

                
                 tblAccount = await _context.TblAccounts.Include(x => x.AccountTransactionAccountDebits).OrderByDescending(x => x.DateAdded).Include(x => x.AccountTransactionAccountCredits).OrderByDescending(x => x.DateAdded).Include(x=>x.AccountType).Include(x => x.Receivables).Include(x => x.Payable).FirstOrDefaultAsync(x => x.AccountId == id);

                foreach(var item in tblAccount.AccountTransactionAccountCredits)
                {
                    item.AccountCredit = await _context.TblAccounts.FindAsync(item.AccountCreditId);
                    item.AccountDebit = await _context.TblAccounts.FindAsync(item.AccountDebitId);
                }

                foreach (var item in tblAccount.AccountTransactionAccountDebits)
                {
                    item.AccountCredit = await _context.TblAccounts.FindAsync(item.AccountCreditId);
                    item.AccountDebit = await _context.TblAccounts.FindAsync(item.AccountDebitId);
                }
            }
            catch(Exception ex)
            {
                return NotFound();
            }
            if (tblAccount == null)
            {
                return NotFound();
            }

            return tblAccount;
        }
        private bool TblAccountExists(long id)
        {
            return _context.TblAccounts.Any(e => e.AccountId == id);
        }
    }
}
