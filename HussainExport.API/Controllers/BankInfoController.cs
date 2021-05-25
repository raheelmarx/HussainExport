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
    public class BankInfoController : ControllerBase
    {
        private readonly HEDBContext _context;

        public BankInfoController(HEDBContext context)
        {
            _context = context;
        }

        // GET: api/BankInfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankInfo>>> GetBankInfos()
        {
            return await _context.BankInfos.ToListAsync();
        }

        // GET: api/BankInfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankInfo>> GetBankInfo(int id)
        {
            var bankInfo = await _context.BankInfos.FindAsync(id);

            if (bankInfo == null)
            {
                return NotFound();
            }

            return bankInfo;
        }

        // PUT: api/BankInfo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankInfo(int id, BankInfo bankInfo)
        {
            if (id != bankInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(bankInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankInfoExists(id))
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

        // POST: api/BankInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BankInfo>> PostBankInfo(BankInfo bankInfo)
        {
            _context.BankInfos.Add(bankInfo);
            await _context.SaveChangesAsync();

            TblAccount tblAccount = new TblAccount()
            {
                AccountCode = bankInfo.AccountNo,
                AccountDescription = bankInfo.Iban,
                AccountTitle = bankInfo.Title,
                AccountTypeId = _context.AccountTypes.Where(x => x.AccountTypeName == "Asset").Select(x => x.AccountTypeId).FirstOrDefault(),
                DateAdded = DateTime.Now,
                IsActive = true
            };
            _context.TblAccounts.Add(tblAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBankInfo", new { id = bankInfo.Id }, bankInfo);
        }

        // DELETE: api/BankInfo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBankInfo(int id)
        {
            var bankInfo = await _context.BankInfos.FindAsync(id);
            if (bankInfo == null)
            {
                return NotFound();
            }

            _context.BankInfos.Remove(bankInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BankInfoExists(int id)
        {
            return _context.BankInfos.Any(e => e.Id == id);
        }
    }
}
