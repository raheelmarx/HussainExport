﻿using System;
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
                var receivableExist = _context.Receivable.Where(x => x.CustomerId == saleContract.CustomerId && x.IsActive == true).FirstOrDefault();

                var payableExist = _context.Payable.Where(x => x.PayableName == saleContract.SaleContractNumber && x.IsActive == true).FirstOrDefault();

                if (receivableExist != null)
                {
                    var tblAccountSaleContractExist = _context.TblAccount.Where(x => x.AccountCode == saleContract.SaleContractNumber && x.PayableId == payableExist.PayableId).FirstOrDefault();

                    // Edit Sale Contract Account

                    tblAccountSaleContractExist.AccountCode = saleContract.SaleContractNumber;
                    tblAccountSaleContractExist.AccountDescription = saleContract.SaleContractNumber;
                    tblAccountSaleContractExist.AccountTitle = saleContract.SaleContractNumber;
                    tblAccountSaleContractExist.AccountTypeId = _context.AccountType.Where(x => x.AccountTypeName == "Liabilities").Select(x => x.AccountTypeId).FirstOrDefault();
                    tblAccountSaleContractExist.DateAdded = DateTime.Now;
                    tblAccountSaleContractExist.IsActive = true;
                    tblAccountSaleContractExist.PayableId = payableExist.PayableId;
                    _context.Entry(tblAccountSaleContractExist).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    var tblAccountReceivable = _context.TblAccount.Where(x => x.AccountCode == receivableExist.ReceivableId.ToString() && x.AccountTitle == receivableExist.ReceivableName && x.ReceivablesId == receivableExist.ReceivableId).FirstOrDefault();
                    // Edit Double Entry of Receivable (DR) and Sale Contract Account (CR)

                    var accountTransactionExist = _context.AccountTransaction.Where(x => x.AccountCreditCode == tblAccountSaleContractExist.AccountCode && x.AccountCreditId == tblAccountSaleContractExist.AccountId).FirstOrDefault();

                    accountTransactionExist.Type = _context.TransactionType.Where(x => x.TransactionTypeName == "SalesContract").Select(x => x.TransactionTypeId).FirstOrDefault();
                    accountTransactionExist.AccountDebitId = tblAccountReceivable.AccountId;
                    accountTransactionExist.AccountCreditId = tblAccountSaleContractExist.AccountId;
                    accountTransactionExist.AccountDebitCode = tblAccountReceivable.AccountCode;
                    accountTransactionExist.AccountCreditCode = tblAccountSaleContractExist.AccountCode;
                    accountTransactionExist.Narration = "Sale Contract Creation";
                    accountTransactionExist.AmountDebit = saleContract.TotalAmount;
                    accountTransactionExist.AmountCredit = saleContract.TotalAmount;
                    accountTransactionExist.SaleContractNumber = saleContract.SaleContractNumber;
                    accountTransactionExist.DateAdded = DateTime.Now;
                    accountTransactionExist.IsActive = true;

                    _context.Entry(accountTransactionExist).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

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

            var receivableExist = _context.Receivable.Where(x => x.CustomerId == saleContract.CustomerId && x.IsActive==true).FirstOrDefault();
            //var payableExist = _context.Payable.Where(x => x.PayableName == saleContract.SaleContractNumber && x.IsActive==true).FirstOrDefault();

            if (receivableExist != null)
            {
                //var tblAccountSaleContractExist = _context.TblAccount.Where(x => x.AccountCode == saleContract.SaleContractNumber && x.PayableId == saleContract.SaleContractId);

                //Add Payable
                Payable payable = new Payable()
                {
                    DateAdded = DateTime.Now,
                    IsActive = true,
                    PayableAddress = saleContract.ShipmentDetails,
                    PayableDescription = "Sale Contract is Paybale",
                    PayableName = saleContract.SaleContractNumber,
                    PayablePhone = receivableExist.ReceivablePhone
                };
                _context.Payable.Add(payable);
                await _context.SaveChangesAsync();


                // Add Sale Contract Account
                TblAccount tblAccountSaleContract = new TblAccount()
                {
                    AccountCode = saleContract.SaleContractNumber,
                    AccountDescription = saleContract.SaleContractNumber,
                    AccountTitle = saleContract.SaleContractNumber,
                    AccountTypeId = _context.AccountType.Where(x => x.AccountTypeName == "Liabilities").Select(x => x.AccountTypeId).FirstOrDefault(),
                    DateAdded = DateTime.Now,
                    IsActive = true,
                    PayableId = payable.PayableId
                };
                _context.TblAccount.Add(tblAccountSaleContract);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var x = 0;
                }

                var tblAccountReceivableExist = _context.TblAccount.Where(x => x.AccountCode == receivableExist.ReceivableId.ToString() && x.AccountTitle == receivableExist.ReceivableName && x.ReceivablesId == receivableExist.ReceivableId).FirstOrDefault();
                if (tblAccountReceivableExist == null)
                {
                    //Add Receiveable Account
                    TblAccount tblAccountReceivable = new TblAccount()
                    {
                        AccountCode = receivableExist.ReceivableId.ToString(),
                        AccountDescription = saleContract.SaleContractNumber,
                        AccountTitle = receivableExist.ReceivableName,
                        AccountTypeId = _context.AccountType.Where(x => x.AccountTypeName == "Receivables").Select(x => x.AccountTypeId).FirstOrDefault(),
                        DateAdded = DateTime.Now,
                        IsActive = true,
                        ReceivablesId = receivableExist.ReceivableId
                    };
                    _context.TblAccount.Add(tblAccountReceivable);
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
                    // Add Double Entry of Receivable (DR) and Sale Contract Account (CR)
                    AccountTransaction accountTransaction = new AccountTransaction()
                    {
                        Type = _context.TransactionType.Where(x => x.TransactionTypeName == "SalesContract").Select(x => x.TransactionTypeId).FirstOrDefault(),
                        AccountDebitId = tblAccountReceivableExist.AccountId,
                        AccountCreditId = tblAccountSaleContract.AccountId,
                        AccountDebitCode = tblAccountReceivableExist.AccountCode,
                        AccountCreditCode = tblAccountSaleContract.AccountCode,
                        Narration = "Sale Contract Creation",
                        AmountDebit = saleContract.TotalAmount,
                        AmountCredit = saleContract.TotalAmount,
                        SaleContractNumber = saleContract.SaleContractNumber,
                        SaleContractId = saleContract.SaleContractId,
                        DateAdded = DateTime.Now,
                        IsActive = true
                    };
                    _context.AccountTransaction.Add(accountTransaction);
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

                //Add Payable
                Payable payable = new Payable()
                {
                    DateAdded = DateTime.Now,
                    IsActive = true,
                    PayableAddress = saleContract.ShipmentDetails,
                    PayableDescription = "Sale Contract is Paybale",
                    PayableName = saleContract.SaleContractNumber,
                    PayablePhone = customer.Contact
                };
                _context.Payable.Add(payable);
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
                    PayableId = payable.PayableId
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
                    SaleContractId = saleContract.SaleContractId,
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
