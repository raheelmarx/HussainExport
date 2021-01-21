using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HussainExport.Client.Data;
using HussainExport.Client.Models;

namespace HussainExport.Client.Controllers
{
    public class SaleContractItemController : Controller
    {
        private readonly HEClientContext _context;

        public SaleContractItemController(HEClientContext context)
        {
            _context = context;
        }

        // GET: SaleContractItem
        public async Task<IActionResult> Index()
        {
            var hEClientContext = _context.SaleContractItemVM.Include(s => s.SaleContract).Include(s => s.Unit);
            return View(await hEClientContext.ToListAsync());
        }

        // GET: SaleContractItem/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleContractItemVM = await _context.SaleContractItemVM
                .Include(s => s.SaleContract)
                .Include(s => s.Unit)
                .FirstOrDefaultAsync(m => m.SaleContractItemId == id);
            if (saleContractItemVM == null)
            {
                return NotFound();
            }

            return View(saleContractItemVM);
        }

        // GET: SaleContractItem/Create
        public IActionResult Create()
        {
            ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId");
            ViewData["UnitId"] = new SelectList(_context.UnitVM, "UnitId", "UnitId");
            return View();
        }

        // POST: SaleContractItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleContractItemId,SaleContractId,Quality,Article,Color,Size,UnitId,Price,Quantity,Amount,IsActive,DateAdded,DateUpdated")] SaleContractItemVM saleContractItemVM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(saleContractItemVM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", saleContractItemVM.SaleContractId);
            ViewData["UnitId"] = new SelectList(_context.UnitVM, "UnitId", "UnitId", saleContractItemVM.UnitId);
            return View(saleContractItemVM);
        }

        // GET: SaleContractItem/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleContractItemVM = await _context.SaleContractItemVM.FindAsync(id);
            if (saleContractItemVM == null)
            {
                return NotFound();
            }
            ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", saleContractItemVM.SaleContractId);
            ViewData["UnitId"] = new SelectList(_context.UnitVM, "UnitId", "UnitId", saleContractItemVM.UnitId);
            return View(saleContractItemVM);
        }

        // POST: SaleContractItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SaleContractItemId,SaleContractId,Quality,Article,Color,Size,UnitId,Price,Quantity,Amount,IsActive,DateAdded,DateUpdated")] SaleContractItemVM saleContractItemVM)
        {
            if (id != saleContractItemVM.SaleContractItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleContractItemVM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleContractItemVMExists(saleContractItemVM.SaleContractItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", saleContractItemVM.SaleContractId);
            ViewData["UnitId"] = new SelectList(_context.UnitVM, "UnitId", "UnitId", saleContractItemVM.UnitId);
            return View(saleContractItemVM);
        }

        // GET: SaleContractItem/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleContractItemVM = await _context.SaleContractItemVM
                .Include(s => s.SaleContract)
                .Include(s => s.Unit)
                .FirstOrDefaultAsync(m => m.SaleContractItemId == id);
            if (saleContractItemVM == null)
            {
                return NotFound();
            }

            return View(saleContractItemVM);
        }

        // POST: SaleContractItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var saleContractItemVM = await _context.SaleContractItemVM.FindAsync(id);
            _context.SaleContractItemVM.Remove(saleContractItemVM);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleContractItemVMExists(long id)
        {
            return _context.SaleContractItemVM.Any(e => e.SaleContractItemId == id);
        }
    }
}
