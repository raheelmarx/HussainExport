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
    public class SaleContractExpenseController : ControllerBase
    {
        private readonly HEDBContext _context;

        public SaleContractExpenseController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/SaleContractExpense
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleContractExpense>>> GetSaleContractExpenses()
        {
            return await _context.SaleContractExpenses.ToListAsync();
        }

        // GET: api/SaleContractExpense/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleContractExpense>> GetSaleContractExpense(long id)
        {
            var saleContractExpense = await _context.SaleContractExpenses.FindAsync(id);

            if (saleContractExpense == null)
            {
                return NotFound();
            }

            return saleContractExpense;
        }

        // PUT: api/SaleContractExpense/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleContractExpense(long id, SaleContractExpense saleContractExpense)
        {
            if (id != saleContractExpense.ExpenseId)
            {
                return BadRequest();
            }

            _context.Entry(saleContractExpense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleContractExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            var saleContractAccountExist = _context.TblAccounts.Where(x => x.AccountCode == saleContractExpense.SaleContractNumber).FirstOrDefault();
            var paymentSourceAccountExist = _context.TblAccounts.Where(x => x.AccountId == saleContractExpense.PaymentSourceAccountId).FirstOrDefault();

            if (saleContractAccountExist != null)
            {
                var saleContractAccountTransactionExist = _context.AccountTransactions.Where(x => x.AccountDebitId == saleContractAccountExist.AccountId && x.AccountCreditId == paymentSourceAccountExist.AccountId).FirstOrDefault();

                if (saleContractAccountTransactionExist != null)
                {
                    saleContractAccountTransactionExist.Type = _context.TransactionTypes.Where(x => x.TransactionTypeName == "SaleContractExpense").Select(x => x.TransactionTypeId).FirstOrDefault();
                    saleContractAccountTransactionExist.AccountDebitId = saleContractAccountExist.AccountId;
                    saleContractAccountTransactionExist.AccountCreditId = paymentSourceAccountExist.AccountId;
                    saleContractAccountTransactionExist.AccountDebitCode = saleContractAccountExist.AccountCode;
                    saleContractAccountTransactionExist.AccountCreditCode = paymentSourceAccountExist.AccountCode;
                    saleContractAccountTransactionExist.Narration = saleContractExpense.Description;
                    saleContractAccountTransactionExist.AmountDebit = saleContractExpense.Amount;
                    saleContractAccountTransactionExist.AmountCredit = saleContractExpense.Amount;
                    saleContractAccountTransactionExist.SaleContractNumber = saleContractExpense.SaleContractNumber;
                    saleContractAccountTransactionExist.DateAdded = DateTime.Now;
                    saleContractAccountTransactionExist.IsActive = true;

                    _context.Entry(saleContractAccountTransactionExist).State = EntityState.Modified;
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
                    // Add Double Entry of Sale Contract Account (DR) and Cash/Bank Account (CR)
                    AccountTransaction accountTransaction = new AccountTransaction()
                    {
                        Type = _context.TransactionTypes.Where(x => x.TransactionTypeName == "SaleContractExpense").Select(x => x.TransactionTypeId).FirstOrDefault(),
                        AccountDebitId = saleContractAccountExist.AccountId,
                        AccountCreditId = paymentSourceAccountExist.AccountId,
                        AccountDebitCode = saleContractAccountExist.AccountCode,
                        AccountCreditCode = paymentSourceAccountExist.AccountCode,
                        Narration = saleContractExpense.Description,
                        AmountDebit = saleContractExpense.Amount,
                        AmountCredit = saleContractExpense.Amount,
                        SaleContractNumber = saleContractExpense.SaleContractNumber,
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

        // POST: api/SaleContractExpense
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SaleContractExpense>> PostSaleContractExpense(SaleContractExpense saleContractExpense)
        {
            _context.SaleContractExpenses.Add(saleContractExpense);
            await _context.SaveChangesAsync();

            var saleContractAccountExist = _context.TblAccounts.Where(x => x.AccountCode == saleContractExpense.SaleContractNumber).FirstOrDefault();
            var paymentSourceAccountExist = _context.TblAccounts.Where(x => x.AccountId == saleContractExpense.PaymentSourceAccountId).FirstOrDefault();

            if (saleContractAccountExist != null)
            {
                // Add Double Entry of Sale Contract Account (DR) and Cash/Bank Account (CR)
                AccountTransaction accountTransaction = new AccountTransaction()
                {
                    Type = _context.TransactionTypes.Where(x => x.TransactionTypeName == "SaleContractExpense").Select(x => x.TransactionTypeId).FirstOrDefault(),
                    AccountDebitId = saleContractAccountExist.AccountId,
                    AccountCreditId = paymentSourceAccountExist.AccountId,
                    AccountDebitCode = saleContractAccountExist.AccountCode,
                    AccountCreditCode = paymentSourceAccountExist.AccountCode,
                    Narration = saleContractExpense.Description,
                    AmountDebit = saleContractExpense.Amount,
                    AmountCredit = saleContractExpense.Amount,
                    SaleContractNumber = saleContractExpense.SaleContractNumber,
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

                // Add Sale Contract Account
                TblAccount tblAccountSaleContract = new TblAccount()
                {
                    AccountCode = saleContractExpense.SaleContractNumber,
                    AccountDescription = saleContractExpense.SaleContractNumber,
                    AccountTitle = saleContractExpense.SaleContractNumber,
                    AccountTypeId = _context.AccountTypes.Where(x => x.AccountTypeName == "Liabilities").Select(x => x.AccountTypeId).FirstOrDefault(),
                    DateAdded = DateTime.Now,
                    IsActive = true
                };
                _context.TblAccounts.Add(tblAccountSaleContract);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var x = 0;
                }


                // Add Double Entry of Sale Contract Account (DR) and Cash/Bank Account (CR)
                AccountTransaction accountTransaction = new AccountTransaction()
                {
                    Type = _context.TransactionTypes.Where(x => x.TransactionTypeName == "SaleContractExpense").Select(x => x.TransactionTypeId).FirstOrDefault(),
                    AccountDebitId = tblAccountSaleContract.AccountId,
                    AccountCreditId = paymentSourceAccountExist.AccountId,
                    AccountDebitCode = tblAccountSaleContract.AccountCode,
                    AccountCreditCode = paymentSourceAccountExist.AccountCode,
                    Narration = saleContractExpense.Description,
                    AmountDebit = saleContractExpense.Amount,
                    AmountCredit = saleContractExpense.Amount,
                    SaleContractNumber = saleContractExpense.SaleContractNumber,
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

            return CreatedAtAction("GetSaleContractExpense", new { id = saleContractExpense.ExpenseId }, saleContractExpense);
        }

        // DELETE: api/SaleContractExpense/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaleContractExpense(long id)
        {
            var saleContractExpense = await _context.SaleContractExpenses.FindAsync(id);
            if (saleContractExpense == null)
            {
                return NotFound();
            }

            _context.SaleContractExpenses.Remove(saleContractExpense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SaleContractExpenseExists(long id)
        {
            return _context.SaleContractExpenses.Any(e => e.ExpenseId == id);
        }
    }
}
