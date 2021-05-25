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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FabricPurchaseController : ControllerBase
    {
        private readonly HEDBContext _context;

        public FabricPurchaseController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/FabricPurchase
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FabricPurchase>>> GetFabricPurchase()
        {
            return await _context.FabricPurchases.ToListAsync();
        }

        // GET: api/FabricPurchase/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FabricPurchase>> GetFabricPurchase(long id)
        {
            var fabricPurchase = await _context.FabricPurchases.FindAsync(id);

            if (fabricPurchase == null)
            {
                return NotFound();
            }

            return fabricPurchase;
        }

        // PUT: api/FabricPurchase/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFabricPurchase(long id, FabricPurchase fabricPurchase)
        {
            if (id != fabricPurchase.FabricPurchaseId)
            {
                return BadRequest();
            }

            _context.Entry(fabricPurchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var payableExist = _context.Payables.Where(x => x.PayableId == fabricPurchase.Weaver && x.IsActive == true).FirstOrDefault();

                if (payableExist != null)
                {
                    var tblAccountPayableExist = _context.TblAccounts.Where(x => x.AccountCode == payableExist.PayableId.ToString() && x.AccountTitle == payableExist.PayableName && x.PayableId == payableExist.PayableId).FirstOrDefault();
                    if (tblAccountPayableExist != null)
                    {
                        var tblAccountSaleContractExist = _context.TblAccounts.Where(x => x.AccountCode == fabricPurchase.SaleContractNumber).FirstOrDefault();
                        // Edit Double Entry of Payable (CR) and Sale Contract Account (DR)

                        var accountTransactionExist = _context.AccountTransactions.Where(x => x.AccountCreditCode == tblAccountPayableExist.AccountCode && x.AccountCreditId == tblAccountPayableExist.AccountId && x.Type==  _context.TransactionTypes.Where(x => x.TransactionTypeName == "FabricPurchase").Select(x => x.TransactionTypeId).FirstOrDefault()).FirstOrDefault();


                        accountTransactionExist.Type = _context.TransactionTypes.Where(x => x.TransactionTypeName == "FabricPurchase").Select(x => x.TransactionTypeId).FirstOrDefault();
                            //accountTransactionExist.AccountDebitId = tblAccountSaleContractExist.AccountId;
                            //accountTransactionExist.AccountCreditId = tblAccountPayableExist.AccountId;
                            //accountTransactionExist.AccountDebitCode = tblAccountSaleContractExist.AccountCode;
                            //accountTransactionExist.AccountCreditCode = tblAccountPayableExist.AccountCode;
                            accountTransactionExist.Narration = "Fabric Purchase Contract Update";
                            accountTransactionExist.AmountDebit = fabricPurchase.QuantityInMeters * fabricPurchase.PerMeterRate;
                            accountTransactionExist.AmountCredit = fabricPurchase.QuantityInMeters * fabricPurchase.PerMeterRate;
                            //accountTransactionExist.SaleContractNumber = fabricPurchase.SaleContractNumber;
                            accountTransactionExist.DateUpdated = DateTime.Now;
                            accountTransactionExist.IsActive = true;


                    _context.Entry(accountTransactionExist).State = EntityState.Modified;
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
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FabricPurchaseExists(id))
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

        // POST: api/FabricPurchase
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FabricPurchase>> PostFabricPurchase(FabricPurchase fabricPurchase)
        {
            _context.FabricPurchases.Add(fabricPurchase);
            await _context.SaveChangesAsync();

            //if (fabricPurchase.IsConversionContract == false)
            //{
                // For Fabric Purchase Contract
                var payableExist = _context.Payables.Where(x => x.PayableId == fabricPurchase.Weaver && x.IsActive == true).FirstOrDefault();

                if (payableExist != null)
                {
                    var tblAccountPayableExist = _context.TblAccounts.Where(x => x.AccountCode == payableExist.PayableId.ToString() && x.AccountTitle == payableExist.PayableName && x.PayableId == payableExist.PayableId).FirstOrDefault();
                    if (tblAccountPayableExist != null)
                    {
                        var tblAccountSaleContractExist = _context.TblAccounts.Where(x => x.AccountCode == fabricPurchase.SaleContractNumber).FirstOrDefault();

                        // Add Double Entry of PAYABLE (CR) and Sale Contract Account (DR)
                        AccountTransaction accountTransaction = new AccountTransaction()
                        {
                            Type = _context.TransactionTypes.Where(x => x.TransactionTypeName == "FabricPurchase").Select(x => x.TransactionTypeId).FirstOrDefault(),
                            AccountDebitId = tblAccountSaleContractExist.AccountId,
                            AccountCreditId = tblAccountPayableExist.AccountId,
                            AccountDebitCode = tblAccountSaleContractExist.AccountCode,
                            AccountCreditCode = tblAccountPayableExist.AccountCode,
                            Narration = "Fabric Purchase Contract Creation",
                            AmountDebit = fabricPurchase.QuantityInMeters * fabricPurchase.PerMeterRate,
                            AmountCredit = fabricPurchase.QuantityInMeters * fabricPurchase.PerMeterRate,
                            SaleContractNumber = fabricPurchase.SaleContractNumber,
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
                        //Add Payable Account
                        TblAccount tblAccountPayable = new TblAccount()
                        {
                            AccountCode = payableExist.PayableId.ToString(),
                            AccountDescription = fabricPurchase.SaleContractNumber,
                            AccountTitle = payableExist.PayableName,
                            AccountTypeId = _context.AccountTypes.Where(x => x.AccountTypeName == "Payable").Select(x => x.AccountTypeId).FirstOrDefault(),
                            DateAdded = DateTime.Now,
                            IsActive = true,
                            ReceivablesId = payableExist.PayableId
                        };
                        _context.TblAccounts.Add(tblAccountPayable);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        var x = 0;
                    }

                    var tblAccountSaleContractExist = _context.TblAccounts.Where(x => x.AccountCode == fabricPurchase.SaleContractNumber).FirstOrDefault();


                        // Add Double Entry of PAYABLE (CR) and Sale Contract Account (DR)
                        AccountTransaction accountTransaction = new AccountTransaction()
                        {
                            Type = _context.TransactionTypes.Where(x => x.TransactionTypeName == "FabricPurchase").Select(x => x.TransactionTypeId).FirstOrDefault(),
                            AccountDebitId = tblAccountSaleContractExist.AccountId,
                            AccountCreditId = tblAccountPayable.AccountId,
                            AccountDebitCode = tblAccountSaleContractExist.AccountCode,
                            AccountCreditCode = tblAccountPayable.AccountCode,
                            Narration = "Fabric Purchase Contract Creation",
                            AmountDebit = fabricPurchase.QuantityInMeters * fabricPurchase.PerMeterRate,
                            AmountCredit = fabricPurchase.QuantityInMeters * fabricPurchase.PerMeterRate,
                            SaleContractNumber = fabricPurchase.SaleContractNumber,
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
            //}
            //else
            //{
            //    //programming for conversion contract
            //}
                return CreatedAtAction("GetFabricPurchase", new { id = fabricPurchase.FabricPurchaseId }, fabricPurchase);
        }

        // DELETE: api/FabricPurchase/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabricPurchase(long id)
        {
            var fabricPurchase = await _context.FabricPurchases.FindAsync(id);
            if (fabricPurchase == null)
            {
                return NotFound();
            }

            _context.FabricPurchases.Remove(fabricPurchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FabricPurchaseExists(long id)
        {
            return _context.FabricPurchases.Any(e => e.FabricPurchaseId == id);
        }
    }
}
