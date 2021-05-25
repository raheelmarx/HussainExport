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
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace HussainExport.Client.Controllers
{
    public class SaleContractExpenseController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public SaleContractExpenseController(HEClientContext context)
        {
            _context = context;
        }

        // GET: SaleContractExpense
        public async Task<IActionResult> Index()
        {
            List<SaleContractExpenseVM> saleContractExpenseVM = new List<SaleContractExpenseVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res = await client.GetAsync("api/SaleContractExpense");

            if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                saleContractExpenseVM = JsonConvert.DeserializeObject<List<SaleContractExpenseVM>>(result);

            }
            //returning the role list to view    
            return View(saleContractExpenseVM);
            //var hEClientContext = _context.SaleContractExpenseVM.Include(s => s.PaymentSourceAccount).Include(s => s.SaleContract);
            //return View(await hEClientContext.ToListAsync());
        }

        // GET: SaleContractExpense/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SaleContractExpenseVM saleContractExpenseVM = new SaleContractExpenseVM();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            //var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            HttpResponseMessage res = await client.GetAsync("api/SaleContractExpense/" + id);

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
                saleContractExpenseVM = JsonConvert.DeserializeObject<SaleContractExpenseVM>(result);

            }
            if (saleContractExpenseVM == null)
            {
                return NotFound();
            }

            return View(saleContractExpenseVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var saleContractExpenseVM = await _context.SaleContractExpenseVM
            //    .Include(s => s.PaymentSourceAccount)
            //    .Include(s => s.SaleContract)
            //    .FirstOrDefaultAsync(m => m.ExpenseId == id);
            //if (saleContractExpenseVM == null)
            //{
            //    return NotFound();
            //}

            //return View(saleContractExpenseVM);
        }

        // GET: SaleContractExpense/Create
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

            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();
            HttpResponseMessage res = await client.GetAsync("api/SaleContracts");
            if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                saleContractVM = JsonConvert.DeserializeObject<List<SaleContractVM>>(result);

            }
            ViewData["SaleContractId"] = new SelectList(saleContractVM, "SaleContractId", "SaleContractNumber");
            return View();
        }

        // POST: SaleContractExpense/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpenseId,SaleContractId,SaleContractNumber,PaymentSourceAccountId,PaymentSourceAccountCode,Amount,Description,IsActive,DateAdded,DateUpdated,AddedBy,UpdatedBy")] SaleContractExpenseVM saleContractExpenseVM)
        { 
            HttpClient client = _helperAPI.InitializeClient();
            if (ModelState.IsValid)
            {
                saleContractExpenseVM.DateAdded = DateTime.Now;
                saleContractExpenseVM.IsActive = true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var content = new StringContent(JsonConvert.SerializeObject(saleContractExpenseVM), Encoding.UTF8, "application/json");
                //Task has been cancelled exception occured here, and Api method never hits while debugging
                HttpResponseMessage result = client.PostAsync("api/SaleContractExpense", content).Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(saleContractExpenseVM);
            //if (ModelState.IsValid)
            //{
            //    _context.Add(saleContractExpenseVM);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

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

            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();
            HttpResponseMessage res = await client.GetAsync("api/SaleContracts");
            if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                saleContractVM = JsonConvert.DeserializeObject<List<SaleContractVM>>(result);

            }
            ViewData["PaymentSourceAccountId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle", saleContractExpenseVM.PaymentSourceAccountId);
            ViewData["SaleContractId"] = new SelectList(saleContractVM, "SaleContractId", "SaleContractNumber", saleContractExpenseVM.SaleContractId);
            return View(saleContractExpenseVM);
        }

        // GET: SaleContractExpense/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var saleContractExpenseVM = await _context.SaleContractExpenseVM.FindAsync(id);
            //if (saleContractExpenseVM == null)
            //{
            //    return NotFound();
            //}
            if (id == null)
            {
                return NotFound();
            }

            SaleContractExpenseVM saleContractExpenseVM = new SaleContractExpenseVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage responce = await client.GetAsync("api/SaleContractExpense/" + id);

            if (responce.IsSuccessStatusCode)
            {
                var result = responce.Content.ReadAsStringAsync().Result;
                saleContractExpenseVM = JsonConvert.DeserializeObject<SaleContractExpenseVM>(result);
            }

            if (saleContractExpenseVM == null)
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

            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();
            HttpResponseMessage res = await client.GetAsync("api/SaleContracts");
            if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                saleContractVM = JsonConvert.DeserializeObject<List<SaleContractVM>>(result);

            }
            ViewData["PaymentSourceAccountId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle", saleContractExpenseVM.PaymentSourceAccountId);
            ViewData["SaleContractId"] = new SelectList(saleContractVM, "SaleContractId", "SaleContractNumber", saleContractExpenseVM.SaleContractId);
            return View(saleContractExpenseVM);
        }

        // POST: SaleContractExpense/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ExpenseId,SaleContractId,SaleContractNumber,PaymentSourceAccountId,PaymentSourceAccountCode,Amount,Description,IsActive,DateAdded,DateUpdated,AddedBy,UpdatedBy")] SaleContractExpenseVM saleContractExpenseVM)
        {
            //if (id != saleContractExpenseVM.ExpenseId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(saleContractExpenseVM);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!SaleContractExpenseVMExists(saleContractExpenseVM.ExpenseId))
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
            HttpClient client = _helperAPI.InitializeClient();
            if (id != saleContractExpenseVM.ExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    saleContractExpenseVM.DateUpdated = DateTime.Now;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    var content = new StringContent(JsonConvert.SerializeObject(saleContractExpenseVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage responce = await client.PutAsync("api/SaleContractExpense/" + id, content);
                    if (responce.IsSuccessStatusCode)
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

            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();
            HttpResponseMessage res = await client.GetAsync("api/SaleContracts");
            if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                saleContractVM = JsonConvert.DeserializeObject<List<SaleContractVM>>(result);

            }
            ViewData["PaymentSourceAccountId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle", saleContractExpenseVM.PaymentSourceAccountId);
            ViewData["SaleContractId"] = new SelectList(saleContractVM, "SaleContractId", "SaleContractNumber", saleContractExpenseVM.SaleContractId);
            //ViewData["PaymentSourceAccountId"] = new SelectList(_context.TblAccountVM, "AccountId", "AccountId", saleContractExpenseVM.PaymentSourceAccountId);
            //ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", saleContractExpenseVM.SaleContractId);
            return View(saleContractExpenseVM);
        }

        // GET: SaleContractExpense/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var saleContractExpenseVM = await _context.SaleContractExpenseVM
            //    .Include(s => s.PaymentSourceAccount)
            //    .Include(s => s.SaleContract)
            //    .FirstOrDefaultAsync(m => m.ExpenseId == id);
            //if (saleContractExpenseVM == null)
            //{
            //    return NotFound();
            //}

            if (id == null)
            {
                return NotFound();
            }

            SaleContractExpenseVM saleContractExpenseVM = new SaleContractExpenseVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/SaleContractExpense/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                saleContractExpenseVM = JsonConvert.DeserializeObject<SaleContractExpenseVM>(result);
            }
            if (saleContractExpenseVM == null)
            {
                return NotFound();
            }

            return View(saleContractExpenseVM);
        }

        // POST: SaleContractExpense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = client.DeleteAsync($"api/SaleContractExpense/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));
            //var saleContractExpenseVM = await _context.SaleContractExpenseVM.FindAsync(id);
            //_context.SaleContractExpenseVM.Remove(saleContractExpenseVM);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private bool SaleContractExpenseVMExists(long id)
        {
            return _context.SaleContractExpenseVM.Any(e => e.ExpenseId == id);
        }
    }
}
