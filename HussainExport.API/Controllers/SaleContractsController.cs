using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HussainExport.API.Entities;
using HussainExport.API.Helpers;

namespace HussainExport.API.Controllers
{
    //[Authorize]
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
            //var saleContractItems = _context.SaleContractItem.Where(x => x.SaleContractId == id).ToList();
            //saleContract.SaleContractItem = saleContractItems;

            if (saleContract == null)
            {
                return NotFound();
            }

            return saleContract;
        }
        // GET: api/SaleContracts/5
        [HttpGet("GetSaleContractDetails/{id}")]
        public SaleContract GetSaleContractDetails(long id)
        {
            var saleContract =  _context.SaleContract.Where(x=>x.SaleContractId == id).FirstOrDefault();
            var saleContractItems = _context.SaleContractItem.Where(x => x.SaleContractId == id).ToList();
            saleContract.SaleContractItem = saleContractItems;

            //if (saleContract == null)
            //{
            //    return NotFound();
            //}

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
                var receivableExist = _context.Receivable.Where(x => x.CustomerId == saleContract.CustomerId).FirstOrDefault();

                if (receivableExist != null)
                {
                    var tblAccountSaleContractExist = _context.TblAccount.Where(x => x.AccountCode == saleContract.SaleContractNumber && x.PayableId == saleContract.SaleContractId);

                    //// Add Sale Contract Account
                    //TblAccount tblAccountSaleContract = new TblAccount()
                    //{
                    //    AccountCode = saleContract.SaleContractNumber,
                    //    AccountDescription = saleContract.SaleContractNumber,
                    //    AccountTitle = saleContract.SaleContractNumber,
                    //    AccountTypeId = _context.AccountType.Where(x => x.AccountTypeName == "Liabilities").Select(x => x.AccountTypeId).FirstOrDefault(),
                    //    DateAdded = DateTime.Now,
                    //    IsActive = true,
                    //    PayableId = saleContract.SaleContractId
                    //};
                    //_context.TblAccount.Add(tblAccountSaleContract);

                    await _context.SaveChangesAsync();

                    var tblAccountReceivable = _context.TblAccount.Where(x => x.AccountCode == receivableExist.ReceivableId.ToString() && x.AccountTitle == receivableExist.ReceivableName && x.ReceivablesId == receivableExist.ReceivableId).FirstOrDefault();
                    // Add Double Entry of Receivable (DR) and Sale Contract Account (CR)
                    //AccountTransaction accountTransaction = new AccountTransaction()
                    //{
                    //    Type = _context.TransactionType.Where(x => x.TransactionTypeName == "SalesContract").Select(x => x.TransactionTypeId).FirstOrDefault(),
                    //    AccountDebitId = tblAccountReceivable.AccountId,
                    //    AccountCreditId = tblAccountSaleContract.AccountId,
                    //    AccountDebitCode = tblAccountReceivable.AccountCode,
                    //    AccountCreditCode = tblAccountSaleContract.AccountCode,
                    //    Narration = "Sale Contract Creation",
                    //    AmountDebit = saleContract.TotalAmount,
                    //    AmountCredit = saleContract.TotalAmount,
                    //    SaleContractNumber = saleContract.SaleContractNumber,
                    //    DateAdded = DateTime.Now,
                    //    IsActive = true
                    //};
                }
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

            var receivableExist = _context.Receivable.Where(x => x.CustomerId == saleContract.CustomerId).FirstOrDefault();

            if (receivableExist != null)
            {
                //var tblAccountSaleContractExist = _context.TblAccount.Where(x => x.AccountCode == saleContract.SaleContractNumber && x.PayableId == saleContract.SaleContractId);

                // Add Sale Contract Account
                TblAccount tblAccountSaleContract = new TblAccount()
                {
                    AccountCode = saleContract.SaleContractNumber,
                    AccountDescription = saleContract.SaleContractNumber,
                    AccountTitle = saleContract.SaleContractNumber,
                    AccountTypeId = _context.AccountType.Where(x => x.AccountTypeName == "Liabilities").Select(x => x.AccountTypeId).FirstOrDefault(),
                    DateAdded = DateTime.Now,
                    IsActive = true,
                    PayableId = saleContract.SaleContractId
                };
                _context.TblAccount.Add(tblAccountSaleContract);

                await _context.SaveChangesAsync();

                var tblAccountReceivable = _context.TblAccount.Where(x => x.AccountCode == receivableExist.ReceivableId.ToString() && x.AccountTitle == receivableExist.ReceivableName && x.ReceivablesId == receivableExist.ReceivableId).FirstOrDefault();
                // Add Double Entry of Receivable (DR) and Sale Contract Account (CR)
                AccountTransaction accountTransaction = new AccountTransaction()
                {
                    Type = _context.TransactionType.Where(x => x.TransactionTypeName == "SalesContract").Select(x => x.TransactionTypeId).FirstOrDefault(),
                    AccountDebitId = tblAccountReceivable.AccountId,
                    AccountCreditId = tblAccountSaleContract.AccountId,
                    AccountDebitCode = tblAccountReceivable.AccountCode,
                    AccountCreditCode = tblAccountSaleContract.AccountCode,
                    Narration = "Sale Contract Creation",
                    AmountDebit = saleContract.TotalAmount,
                    AmountCredit = saleContract.TotalAmount,
                    SaleContractNumber = saleContract.SaleContractNumber,
                    DateAdded = DateTime.Now,
                    IsActive = true
                };
            }

            if (receivableExist == null)
            {
                //Add Receiveable
                var customer = await _context.Customer.FindAsync(saleContract.CustomerId);
                Receivable receivable = new Receivable()
                {
                    CustomerId = customer.CustomerId,
                    DateAdded = DateTime.Now,
                    IsActive = true,
                    ReceivableAddress = customer.Address,
                    ReceivableDescription = "Customer with Sale Contract",
                    ReceivableName = customer.CustomerName,
                    ReceivablePhone = customer.Contact
                };
                _context.Receivable.Add(receivable);
                await _context.SaveChangesAsync();

                //Add Receiveable Account
                TblAccount tblAccountReceivable = new TblAccount()
                {
                    AccountCode = receivable.ReceivableId.ToString(),
                    AccountDescription = saleContract.SaleContractNumber,
                    AccountTitle = receivable.ReceivableName,
                    AccountTypeId = _context.AccountType.Where(x => x.AccountTypeName == "Receivables").Select(x => x.AccountTypeId).FirstOrDefault(),
                    DateAdded = DateTime.Now,
                    IsActive = true,
                    ReceivablesId = receivable.ReceivableId
                };
                _context.TblAccount.Add(tblAccountReceivable);

                // Add Sale Contract Account
                TblAccount tblAccountSaleContract = new TblAccount()
                {
                    AccountCode = saleContract.SaleContractNumber,
                    AccountDescription = saleContract.SaleContractNumber,
                    AccountTitle = saleContract.SaleContractNumber,
                    AccountTypeId = _context.AccountType.Where(x => x.AccountTypeName == "Liabilities").Select(x => x.AccountTypeId).FirstOrDefault(),
                    DateAdded = DateTime.Now,
                    IsActive = true,
                    PayableId = saleContract.SaleContractId
                };
                _context.TblAccount.Add(tblAccountSaleContract);

                await _context.SaveChangesAsync();

                // Add Double Entry of Receivable (DR) and Sale Contract Account (CR)
                AccountTransaction accountTransaction = new AccountTransaction()
                {
                    Type = _context.TransactionType.Where(x => x.TransactionTypeName == "SalesContract").Select(x => x.TransactionTypeId).FirstOrDefault(),
                    AccountDebitId = tblAccountReceivable.AccountId,
                    AccountCreditId = tblAccountSaleContract.AccountId,
                    AccountDebitCode = tblAccountReceivable.AccountCode,
                    AccountCreditCode = tblAccountSaleContract.AccountCode,
                    Narration = "Sale Contract Creation",
                    AmountDebit = saleContract.TotalAmount,
                    AmountCredit = saleContract.TotalAmount,
                    SaleContractNumber = saleContract.SaleContractNumber,
                    DateAdded = DateTime.Now,
                    IsActive = true
                };

                _context.AccountTransaction.Add(accountTransaction);
                await _context.SaveChangesAsync();
            }


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
