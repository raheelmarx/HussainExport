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
    public class SaleContractItemsController : ControllerBase
    {
        private readonly HEDBContext _context;

        public SaleContractItemsController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/SaleContractItems
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<SaleContractItem>>> GetSaleContractItem()
        public async Task<ActionResult<IEnumerable<SaleContractItem>>> GetSaleContractItem()
        {
            return await _context.SaleContractItem.ToListAsync();
           //var data =   _context.SaleContractItem.Include(x => x.SaleContract).ToList();
           // return data;
        }

        // GET: api/SaleContractItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleContractItem>> GetSaleContractItem(long id)
        {
            var saleContractItem = await _context.SaleContractItem.FindAsync(id);

            if (saleContractItem == null)
            {
                return NotFound();
            }

            return saleContractItem;
        }


        // GET: api/SaleContractItems/5
        [HttpGet("GetSaleContractItemBySaleContractId/{id}")]
        public async Task<ActionResult<IEnumerable<SaleContractItem>>> GetSaleContractItemBySaleContractId(long id)
        {
            var saleContractItems = await _context.SaleContractItem.Where(x => x.SaleContractId == id).ToListAsync();

            if (saleContractItems == null)
            {
                return NotFound();
            }

            return saleContractItems;
        }

        // PUT: api/SaleContractItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleContractItem(long id, SaleContractItem saleContractItem)
        {
            if (id != saleContractItem.SaleContractItemId)
            {
                return BadRequest();
            }
            _context.Entry(saleContractItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleContractItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var allSaleContractItems = _context.SaleContractItem.Where(x => x.SaleContractId == saleContractItem.SaleContractId).ToList();
            var saleContract = await _context.SaleContract.FindAsync(saleContractItem.SaleContractId);
            saleContract.TotalAmount = 0;
            foreach ( var item in allSaleContractItems)
            {
                saleContract.TotalAmount = saleContract.TotalAmount + item.Amount;
            }
            _context.Entry(saleContract).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleContractItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Get Receivable and Add Credit Entry in Account
            var receivableExist = _context.Receivable.Where(x => x.CustomerId == saleContract.CustomerId).FirstOrDefault();
            var payableExist = _context.Payable.Where(x => x.PayableName == saleContract.SaleContractNumber && x.IsActive == true).FirstOrDefault();
            var tblAccountReceivable = _context.TblAccount.Where(x => x.ReceivablesId == receivableExist.ReceivableId).FirstOrDefault();
            var tblAccountSaleContractExist = _context.TblAccount.Where(x => x.AccountCode == saleContract.SaleContractNumber && x.PayableId == payableExist.PayableId).FirstOrDefault();

            // Add Double Entry of Receivable (DR) and Sale Contract Account (CR) => Update only Amount Debit and Credit
            var accountTransaction = _context.AccountTransaction.Where(x => x.AccountDebitId == tblAccountReceivable.AccountId && x.AccountCreditId == tblAccountSaleContractExist.AccountId && x.AccountCreditCode == tblAccountSaleContractExist.AccountCode).FirstOrDefault();

            accountTransaction.AmountDebit = saleContract.TotalAmount;
            accountTransaction.AmountCredit = saleContract.TotalAmount;

            _context.Entry(accountTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleContractItemExists(id))
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

        // POST: api/SaleContractItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SaleContractItem>> PostSaleContractItem(SaleContractItem saleContractItem)
        {
            try
            {
                _context.SaleContractItem.Add(saleContractItem);
                await _context.SaveChangesAsync();
                // Add/Update Sale Contract Total Amount
                var saleContract = await _context.SaleContract.FindAsync(saleContractItem.SaleContractId);
                saleContract.TotalAmount = saleContract.TotalAmount + saleContractItem.Amount;
                _context.Entry(saleContract).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Get Receivable and Add Credit Entry in Account
                var receivableExist = _context.Receivable.Where(x => x.CustomerId == saleContract.CustomerId).FirstOrDefault();
                //if(receivableExist==null)
                //{
                //    var customer = await _context.Customer.FindAsync(saleContract.CustomerId);

                //    receivableExist = new Receivable();
                //    receivableExist.ReceivableName = saleContract.Customer.CustomerName;
                //    receivableExist.ReceivablePhone = saleContract.Customer.Contact;
                //    receivableExist.ReceivableAddress = saleContract.Customer.Address;
                //    receivableExist.ReceivableDescription = saleContract.Customer.CustomerDescription;
                //    receivableExist.Customer = saleContract.Customer;
                //    receivableExist.CustomerId = saleContract.CustomerId;
                //    receivableExist.DateAdded = DateTime.Now;
                //    receivableExist.IsActive = true;

                //    _context.Receivable.Add(receivableExist);
                //}

                var tblAccountReceivable = _context.TblAccount.Where(x => x.ReceivablesId == receivableExist.ReceivableId).FirstOrDefault();
                var payableExist = _context.Payable.Where(x => x.PayableName == saleContract.SaleContractNumber && x.IsActive == true).FirstOrDefault();

                var tblAccountSaleContractExist = _context.TblAccount.Where(x => x.AccountCode == saleContract.SaleContractNumber && x.PayableId == payableExist.PayableId).FirstOrDefault();

                // Add Double Entry of Receivable (DR) and Sale Contract Account (CR) => Update only Amount Debit and Credit
                var accountTransaction = _context.AccountTransaction.Where(x => x.AccountDebitId == tblAccountReceivable.AccountId && x.AccountCreditId == tblAccountSaleContractExist.AccountId && x.AccountCreditCode == tblAccountSaleContractExist.AccountCode).FirstOrDefault();
               
                accountTransaction.AmountDebit = saleContract.TotalAmount;
                accountTransaction.AmountCredit = saleContract.TotalAmount;

                _context.Entry(accountTransaction).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetSaleContractItem", new { id = saleContractItem.SaleContractItemId }, saleContractItem);
            }
            catch (Exception ex)
            {
                return CreatedAtAction("GetSaleContractItem", new { id = saleContractItem.SaleContractItemId }, saleContractItem);
            }
        }

        // DELETE: api/SaleContractItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SaleContractItem>> DeleteSaleContractItem(long id)
        {
            var saleContractItem = await _context.SaleContractItem.FindAsync(id);
            if (saleContractItem == null)
            {
                return NotFound();
            }

            _context.SaleContractItem.Remove(saleContractItem);
            await _context.SaveChangesAsync();

            return saleContractItem;
        }

        private bool SaleContractItemExists(long id)
        {
            return _context.SaleContractItem.Any(e => e.SaleContractItemId == id);
        }
    }
}
