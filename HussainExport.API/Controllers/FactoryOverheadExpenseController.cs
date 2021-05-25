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
    public class FactoryOverheadExpenseController : ControllerBase
    {
        private readonly HEDBContext _context;

        public FactoryOverheadExpenseController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/FactoryOverheadExpense
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FactoryOverheadExpense>>> GetFactoryOverheadExpenses()
        {
            return await _context.FactoryOverheadExpenses.ToListAsync();
        }

        // GET: api/FactoryOverheadExpense/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FactoryOverheadExpense>> GetFactoryOverheadExpense(long id)
        {
            var factoryOverheadExpense = await _context.FactoryOverheadExpenses.FindAsync(id);

            if (factoryOverheadExpense == null)
            {
                return NotFound();
            }

            return factoryOverheadExpense;
        }

        // PUT: api/FactoryOverheadExpense/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactoryOverheadExpense(long id, FactoryOverheadExpense factoryOverheadExpense)
        {
            if (id != factoryOverheadExpense.Fohid)
            {
                return BadRequest();
            }

            _context.Entry(factoryOverheadExpense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactoryOverheadExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var factoryOverheadAccountExist = _context.TblAccounts.Where(x => x.AccountCode == "FOH").FirstOrDefault();
            var paymentSourceAccountExist = _context.TblAccounts.Where(x => x.AccountId == factoryOverheadExpense.PaymentSourceAccountId).FirstOrDefault();

            if (factoryOverheadAccountExist != null)
            {
                var fohAccountTransactionExist = _context.AccountTransactions.Where(x => x.AccountDebitId == factoryOverheadAccountExist.AccountId && x.AccountCreditId == paymentSourceAccountExist.AccountId).FirstOrDefault();

                if (fohAccountTransactionExist != null)
                {
                    fohAccountTransactionExist.Type = _context.TransactionTypes.Where(x => x.TransactionTypeName == "FOH").Select(x => x.TransactionTypeId).FirstOrDefault();
                    fohAccountTransactionExist.AccountDebitId = factoryOverheadAccountExist.AccountId;
                    fohAccountTransactionExist.AccountCreditId = paymentSourceAccountExist.AccountId;
                    fohAccountTransactionExist.AccountDebitCode = factoryOverheadAccountExist.AccountCode;
                    fohAccountTransactionExist.AccountCreditCode = paymentSourceAccountExist.AccountCode;
                    fohAccountTransactionExist.Narration = factoryOverheadExpense.Description;
                    fohAccountTransactionExist.AmountDebit = factoryOverheadExpense.Amount;
                    fohAccountTransactionExist.AmountCredit = factoryOverheadExpense.Amount;
                    fohAccountTransactionExist.SaleContractNumber = "N/A";
                    fohAccountTransactionExist.DateAdded = DateTime.Now;
                    fohAccountTransactionExist.IsActive = true;

                    _context.Entry(fohAccountTransactionExist).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        var x = 0;
                    }
                }
                else
                {
                    // Add Double Entry of Factory Overhead Account (DR) and Cash/Bank Account (CR)
                    AccountTransaction accountTransaction = new AccountTransaction()
                    {
                        Type = _context.TransactionTypes.Where(x => x.TransactionTypeName == "FOH").Select(x => x.TransactionTypeId).FirstOrDefault(),
                        AccountDebitId = factoryOverheadAccountExist.AccountId,
                        AccountCreditId = paymentSourceAccountExist.AccountId,
                        AccountDebitCode = factoryOverheadAccountExist.AccountCode,
                        AccountCreditCode = paymentSourceAccountExist.AccountCode,
                        Narration = factoryOverheadExpense.Description,
                        AmountDebit = factoryOverheadExpense.Amount,
                        AmountCredit = factoryOverheadExpense.Amount,
                        SaleContractNumber = "N/A",
                        DateAdded = DateTime.Now,
                        IsActive = true
                    };
                    _context.AccountTransactions.Add(accountTransaction);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        var x = 0;
                    }
                }
            }

            return NoContent();
        }

        // POST: api/FactoryOverheadExpense
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FactoryOverheadExpense>> PostFactoryOverheadExpense(FactoryOverheadExpense factoryOverheadExpense)
        {
            _context.FactoryOverheadExpenses.Add(factoryOverheadExpense);
            await _context.SaveChangesAsync();

            var factoryOverheadAccountExist = _context.TblAccounts.Where(x => x.AccountCode == "FOH").FirstOrDefault();
            var paymentSourceAccountExist = _context.TblAccounts.Where(x => x.AccountId == factoryOverheadExpense.PaymentSourceAccountId).FirstOrDefault();

            if (factoryOverheadAccountExist!=null)
            {
                // Add Double Entry of Factory Overhead Account (DR) and Cash/Bank Account (CR)
                AccountTransaction accountTransaction = new AccountTransaction()
                {
                    Type = _context.TransactionTypes.Where(x => x.TransactionTypeName == "FOH").Select(x => x.TransactionTypeId).FirstOrDefault(),
                    AccountDebitId = factoryOverheadAccountExist.AccountId,
                    AccountCreditId = paymentSourceAccountExist.AccountId,
                    AccountDebitCode = factoryOverheadAccountExist.AccountCode,
                    AccountCreditCode = paymentSourceAccountExist.AccountCode,
                    Narration = factoryOverheadExpense.Description,
                    AmountDebit = factoryOverheadExpense.Amount,
                    AmountCredit = factoryOverheadExpense.Amount,
                    SaleContractNumber = "N/A",
                    DateAdded = DateTime.Now,
                    IsActive = true
                };
                _context.AccountTransactions.Add(accountTransaction);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var x = 0;
                }
            }
            else
            {

                //Add FOH Account
                TblAccount factoryOverheadAccount = new TblAccount()
                {
                    AccountCode = "FOH",
                    AccountDescription ="Factory Overhead Account",
                    AccountTitle = "Factory Overhead",
                    AccountTypeId = _context.AccountTypes.Where(x => x.AccountTypeName == "Expense").Select(x => x.AccountTypeId).FirstOrDefault(),
                    DateAdded = DateTime.Now,
                    IsActive = true
                };
                _context.TblAccounts.Add(factoryOverheadAccount);
                await _context.SaveChangesAsync();


                // Add Double Entry of Factory Overhead Account (DR) and Cash/Bank Account (CR)
                AccountTransaction accountTransaction = new AccountTransaction()
                {
                    Type = _context.TransactionTypes.Where(x => x.TransactionTypeName == "FOH").Select(x => x.TransactionTypeId).FirstOrDefault(),
                    AccountDebitId = factoryOverheadAccount.AccountId,
                    AccountCreditId = paymentSourceAccountExist.AccountId,
                    AccountDebitCode = factoryOverheadAccount.AccountCode,
                    AccountCreditCode = paymentSourceAccountExist.AccountCode,
                    Narration = factoryOverheadExpense.Description,
                    AmountDebit = factoryOverheadExpense.Amount,
                    AmountCredit = factoryOverheadExpense.Amount,
                    SaleContractNumber = "N/A",
                    DateAdded = DateTime.Now,
                    IsActive = true
                };
                _context.AccountTransactions.Add(accountTransaction);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var x = 0;
                }
            }

            return CreatedAtAction("GetFactoryOverheadExpense", new { id = factoryOverheadExpense.Fohid }, factoryOverheadExpense);
        }

        // DELETE: api/FactoryOverheadExpense/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactoryOverheadExpense(long id)
        {
            var factoryOverheadExpense = await _context.FactoryOverheadExpenses.FindAsync(id);
            if (factoryOverheadExpense == null)
            {
                return NotFound();
            }

            _context.FactoryOverheadExpenses.Remove(factoryOverheadExpense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FactoryOverheadExpenseExists(long id)
        {
            return _context.FactoryOverheadExpenses.Any(e => e.Fohid == id);
        }
    }
}
