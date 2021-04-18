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
    public class FabricPurchaseItemController : Controller
    {
        private readonly HEClientContext _context;

        public FabricPurchaseItemController(HEClientContext context)
        {
            _context = context;
        }

        // GET: FabricPurchaseItem
        public async Task<IActionResult> Index()
        {
            var hEClientContext = _context.FabricPurchaseItemVM.Include(f => f.FabricPurchase);
            return View(await hEClientContext.ToListAsync());
        }

        // GET: FabricPurchaseItem/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricPurchaseItemVM = await _context.FabricPurchaseItemVM
                .Include(f => f.FabricPurchase)
                .FirstOrDefaultAsync(m => m.FabricPurchaseItemId == id);
            if (fabricPurchaseItemVM == null)
            {
                return NotFound();
            }

            return View(fabricPurchaseItemVM);
        }

        // GET: FabricPurchaseItem/Create
        public IActionResult Create()
        {
            ViewData["FabricPurchaseId"] = new SelectList(_context.FabricPurchaseVM, "FabricPurchaseId", "FabricPurchaseId");
            return View();
        }

        // POST: FabricPurchaseItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FabricPurchaseItemId,FabricPurchaseId,Yarn,YarnRatePerIbs,CountMargin,WeightPerMeterIbs,RequiredBags,FabricRatePerMeter,IsActive,DateAdded,DateUpdated")] FabricPurchaseItemVM fabricPurchaseItemVM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fabricPurchaseItemVM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FabricPurchaseId"] = new SelectList(_context.FabricPurchaseVM, "FabricPurchaseId", "FabricPurchaseId", fabricPurchaseItemVM.FabricPurchaseId);
            return View(fabricPurchaseItemVM);
        }

        // GET: FabricPurchaseItem/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricPurchaseItemVM = await _context.FabricPurchaseItemVM.FindAsync(id);
            if (fabricPurchaseItemVM == null)
            {
                return NotFound();
            }
            ViewData["FabricPurchaseId"] = new SelectList(_context.FabricPurchaseVM, "FabricPurchaseId", "FabricPurchaseId", fabricPurchaseItemVM.FabricPurchaseId);
            return View(fabricPurchaseItemVM);
        }

        // POST: FabricPurchaseItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("FabricPurchaseItemId,FabricPurchaseId,Yarn,YarnRatePerIbs,CountMargin,WeightPerMeterIbs,RequiredBags,FabricRatePerMeter,IsActive,DateAdded,DateUpdated")] FabricPurchaseItemVM fabricPurchaseItemVM)
        {
            if (id != fabricPurchaseItemVM.FabricPurchaseItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fabricPurchaseItemVM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FabricPurchaseItemVMExists(fabricPurchaseItemVM.FabricPurchaseItemId))
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
            ViewData["FabricPurchaseId"] = new SelectList(_context.FabricPurchaseVM, "FabricPurchaseId", "FabricPurchaseId", fabricPurchaseItemVM.FabricPurchaseId);
            return View(fabricPurchaseItemVM);
        }

        // GET: FabricPurchaseItem/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricPurchaseItemVM = await _context.FabricPurchaseItemVM
                .Include(f => f.FabricPurchase)
                .FirstOrDefaultAsync(m => m.FabricPurchaseItemId == id);
            if (fabricPurchaseItemVM == null)
            {
                return NotFound();
            }

            return View(fabricPurchaseItemVM);
        }

        // POST: FabricPurchaseItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var fabricPurchaseItemVM = await _context.FabricPurchaseItemVM.FindAsync(id);
            _context.FabricPurchaseItemVM.Remove(fabricPurchaseItemVM);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FabricPurchaseItemVMExists(long id)
        {
            return _context.FabricPurchaseItemVM.Any(e => e.FabricPurchaseItemId == id);
        }
    }
}
