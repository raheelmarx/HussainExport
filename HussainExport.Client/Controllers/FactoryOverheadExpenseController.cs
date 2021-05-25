using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HussainExport.Client.Data;
using HussainExport.Client.Models;
using HussainExport.Client.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace HussainExport.Client.Controllers
{
    public class FactoryOverheadExpenseController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public FactoryOverheadExpenseController(HEClientContext context)
        {
            _context = context;
        }

        // GET: FactoryOverheadExpense
        public async Task<IActionResult> Index()
        {
            List<FactoryOverheadExpenseVM> factoryOverheadExpenseVM = new List<FactoryOverheadExpenseVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage CurrencyVMsRes = await client.GetAsync("api/FactoryOverheadExpense");

            if (CurrencyVMsRes.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (CurrencyVMsRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = CurrencyVMsRes.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                factoryOverheadExpenseVM = JsonConvert.DeserializeObject<List<FactoryOverheadExpenseVM>>(result);

            }
            //returning the role list to view    
            return View(factoryOverheadExpenseVM);
            //var hEClientContext = _context.FactoryOverheadExpenseVM.Include(f => f.PaymentSourceAccount);
            //return View(await hEClientContext.ToListAsync());
        }

        // GET: FactoryOverheadExpense/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FactoryOverheadExpenseVM factoryOverheadExpenseVM = new FactoryOverheadExpenseVM();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            //var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            HttpResponseMessage res = await client.GetAsync("api/FactoryOverheadExpense/" + id);

            if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the Role list    
                factoryOverheadExpenseVM = JsonConvert.DeserializeObject<FactoryOverheadExpenseVM>(result);

            }
            if (factoryOverheadExpenseVM == null)
            {
                return NotFound();
            }

            return View(factoryOverheadExpenseVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var factoryOverheadExpenseVM = await _context.FactoryOverheadExpenseVM
            //    .Include(f => f.PaymentSourceAccount)
            //    .FirstOrDefaultAsync(m => m.Fohid == id);
            //if (factoryOverheadExpenseVM == null)
            //{
            //    return NotFound();
            //}

            //return View(factoryOverheadExpenseVM);
        }

        // GET: FactoryOverheadExpense/Create
        public async Task<IActionResult> Create()
        {
            List<TblAccountVM> tblAccountVM = new List<TblAccountVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage tblAccountVMRes = await client.GetAsync("api/Account");

            if (tblAccountVMRes.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (tblAccountVMRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = tblAccountVMRes.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                tblAccountVM = JsonConvert.DeserializeObject<List<TblAccountVM>>(result);

            }
            ViewData["PaymentSourceAccountId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle");
            return View();
        }

        // POST: FactoryOverheadExpense/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Fohid,PaymentSourceAccountId,PaymentSourceAccountCode,Amount,Description,IsActive,DateAdded,DateUpdated,AddedBy,UpdatedBy")] FactoryOverheadExpenseVM factoryOverheadExpenseVM)
        {
            HttpClient client = _helperAPI.InitializeClient();
            if (ModelState.IsValid)
            {
                factoryOverheadExpenseVM.DateAdded = DateTime.Now;
                factoryOverheadExpenseVM.IsActive = true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var content = new StringContent(JsonConvert.SerializeObject(factoryOverheadExpenseVM), Encoding.UTF8, "application/json");
                //Task has been cancelled exception occured here, and Api method never hits while debugging
                HttpResponseMessage res = client.PostAsync("api/FactoryOverheadExpense", content).Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            List<TblAccountVM> tblAccountVM = new List<TblAccountVM>();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage tblAccountVMRes = await client.GetAsync("api/Account");

            if (tblAccountVMRes.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (tblAccountVMRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = tblAccountVMRes.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                tblAccountVM = JsonConvert.DeserializeObject<List<TblAccountVM>>(result);

            }
            ViewData["PaymentSourceAccountId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle", factoryOverheadExpenseVM.PaymentSourceAccountId);
            return View(factoryOverheadExpenseVM);
            //if (ModelState.IsValid)
            //{
            //    _context.Add(factoryOverheadExpenseVM);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["PaymentSourceAccountId"] = new SelectList(_context.TblAccountVM, "AccountId", "AccountId", factoryOverheadExpenseVM.PaymentSourceAccountId);
            //return View(factoryOverheadExpenseVM);
        }

        // GET: FactoryOverheadExpense/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FactoryOverheadExpenseVM factoryOverheadExpenseVM = new FactoryOverheadExpenseVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/FactoryOverheadExpense/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                factoryOverheadExpenseVM = JsonConvert.DeserializeObject<FactoryOverheadExpenseVM>(result);
            }

            if (factoryOverheadExpenseVM == null)
            {
                return NotFound();
            }

            List<TblAccountVM> tblAccountVM = new List<TblAccountVM>();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage tblAccountVMRes = await client.GetAsync("api/Account");

            if (tblAccountVMRes.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (tblAccountVMRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = tblAccountVMRes.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                tblAccountVM = JsonConvert.DeserializeObject<List<TblAccountVM>>(result);

            }
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var factoryOverheadExpenseVM = await _context.FactoryOverheadExpenseVM.FindAsync(id);
            //if (factoryOverheadExpenseVM == null)
            //{
            //    return NotFound();
            //}
            ViewData["PaymentSourceAccountId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle", factoryOverheadExpenseVM.PaymentSourceAccountId);
            return View(factoryOverheadExpenseVM);
        }

        // POST: FactoryOverheadExpense/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Fohid,PaymentSourceAccountId,PaymentSourceAccountCode,Amount,Description,IsActive,DateAdded,DateUpdated,AddedBy,UpdatedBy")] FactoryOverheadExpenseVM factoryOverheadExpenseVM)
        {
            HttpClient client = _helperAPI.InitializeClient();
            if (id != factoryOverheadExpenseVM.Fohid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    factoryOverheadExpenseVM.DateUpdated = DateTime.Now;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    var content = new StringContent(JsonConvert.SerializeObject(factoryOverheadExpenseVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage res = await client.PutAsync("api/FactoryOverheadExpense/" + id, content);
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!RoleVMExists(roleVM.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            List<TblAccountVM> tblAccountVM = new List<TblAccountVM>();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage tblAccountVMRes = await client.GetAsync("api/Account");

            if (tblAccountVMRes.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (tblAccountVMRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = tblAccountVMRes.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                tblAccountVM = JsonConvert.DeserializeObject<List<TblAccountVM>>(result);

            }
            //if (id != factoryOverheadExpenseVM.Fohid)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(factoryOverheadExpenseVM);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!FactoryOverheadExpenseVMExists(factoryOverheadExpenseVM.Fohid))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            ViewData["PaymentSourceAccountId"] = new SelectList(_context.TblAccountVM, "AccountId", "AccountTitle", factoryOverheadExpenseVM.PaymentSourceAccountId);
            return View(factoryOverheadExpenseVM);
        }

        // GET: FactoryOverheadExpense/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FactoryOverheadExpenseVM factoryOverheadExpenseVM = new FactoryOverheadExpenseVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/FactoryOverheadExpense/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                factoryOverheadExpenseVM = JsonConvert.DeserializeObject<FactoryOverheadExpenseVM>(result);
            }
            if (factoryOverheadExpenseVM == null)
            {
                return NotFound();
            }
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var factoryOverheadExpenseVM = await _context.FactoryOverheadExpenseVM
            //    .Include(f => f.PaymentSourceAccount)
            //    .FirstOrDefaultAsync(m => m.Fohid == id);
            //if (factoryOverheadExpenseVM == null)
            //{
            //    return NotFound();
            //}

            return View(factoryOverheadExpenseVM);
        }

        // POST: FactoryOverheadExpense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = client.DeleteAsync($"api/FactoryOverheadExpense/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));
            //var factoryOverheadExpenseVM = await _context.FactoryOverheadExpenseVM.FindAsync(id);
            //_context.FactoryOverheadExpenseVM.Remove(factoryOverheadExpenseVM);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private bool FactoryOverheadExpenseVMExists(long id)
        {
            return _context.FactoryOverheadExpenseVM.Any(e => e.Fohid == id);
        }
    }
}
