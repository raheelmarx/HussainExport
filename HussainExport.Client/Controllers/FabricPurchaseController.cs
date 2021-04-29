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
    public class FabricPurchaseController : Controller
    {
        private readonly HEClientContext _context;

        public FabricPurchaseController(HEClientContext context)
        {
            _context = context;
        }

        // GET: FabricPurchase
        public async Task<IActionResult> Index()
        {
            var hEClientContext = _context.FabricPurchaseVM.Include(f => f.SaleContract);
            return View(await hEClientContext.ToListAsync());
        }

        // GET: FabricPurchase/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricPurchaseVM = await _context.FabricPurchaseVM
                .Include(f => f.SaleContract)
                .FirstOrDefaultAsync(m => m.FabricPurchaseId == id);
            if (fabricPurchaseVM == null)
            {
                return NotFound();
            }

            return View(fabricPurchaseVM);
        }

        // GET: FabricPurchase/Create
        public IActionResult Create()
        {
            ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId");
            return View();
        }

        // POST: FabricPurchase/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FabricPurchaseId,SaleContractId,SaleContractNumber,Weaver,ContQuality,Gstquality,IsConversionContract,ConversionRate,PerPickRate,PerMeterRate,QuantityInMeters,Broker,DeliveryTime,Description,IsActive,DateAdded,DateUpdated")] FabricPurchaseVM fabricPurchaseVM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fabricPurchaseVM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", fabricPurchaseVM.SaleContractId);
            return View(fabricPurchaseVM);
        }

        // GET: FabricPurchase/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricPurchaseVM = await _context.FabricPurchaseVM.FindAsync(id);
            if (fabricPurchaseVM == null)
            {
                return NotFound();
            }
            ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", fabricPurchaseVM.SaleContractId);
            return View(fabricPurchaseVM);
        }

        // POST: FabricPurchase/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("FabricPurchaseId,SaleContractId,SaleContractNumber,Weaver,ContQuality,Gstquality,IsConversionContract,ConversionRate,PerPickRate,PerMeterRate,QuantityInMeters,Broker,DeliveryTime,Description,IsActive,DateAdded,DateUpdated")] FabricPurchaseVM fabricPurchaseVM)
        {
            if (id != fabricPurchaseVM.FabricPurchaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fabricPurchaseVM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FabricPurchaseVMExists(fabricPurchaseVM.FabricPurchaseId))
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
            ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", fabricPurchaseVM.SaleContractId);
            return View(fabricPurchaseVM);
        }

        // GET: FabricPurchase/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricPurchaseVM = await _context.FabricPurchaseVM
                .Include(f => f.SaleContract)
                .FirstOrDefaultAsync(m => m.FabricPurchaseId == id);
            if (fabricPurchaseVM == null)
            {
                return NotFound();
            }

            return View(fabricPurchaseVM);
        }

        // POST: FabricPurchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var fabricPurchaseVM = await _context.FabricPurchaseVM.FindAsync(id);
            _context.FabricPurchaseVM.Remove(fabricPurchaseVM);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FabricPurchaseVMExists(long id)
        {
            return _context.FabricPurchaseVM.Any(e => e.FabricPurchaseId == id);
        }
    }
}
