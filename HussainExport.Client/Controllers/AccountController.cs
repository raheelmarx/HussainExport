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
    public class AccountController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public AccountController(HEClientContext context)
        {
            _context = context;
        }

        // GET: Account
        public async Task<IActionResult> Index()
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
            //returning the role list to view    
            return View(tblAccountVM);
            //return View(await _context.TblAccountVM.ToListAsync());
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblAccountVM tblAccountVM = new TblAccountVM();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            //var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            HttpResponseMessage tblAccountVMRes = await client.GetAsync("api/Account/" + id);

            if (tblAccountVMRes.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (tblAccountVMRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = tblAccountVMRes.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the Role list    
                tblAccountVM = JsonConvert.DeserializeObject<TblAccountVM>(result);

            }
            if (tblAccountVM == null)
            {
                return NotFound();
            }

            return View(tblAccountVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var tblAccountVM = await _context.TblAccountVM
            //    .FirstOrDefaultAsync(m => m.AccountId == id);
            //if (tblAccountVM == null)
            //{
            //    return NotFound();
            //}

            //return View(tblAccountVM);
        }

        // GET: Account/Create
        public async Task<IActionResult> Create()
        {
            List<PayableVM> payableVM = new List<PayableVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res = await client.GetAsync("api/Payable");

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
                payableVM = JsonConvert.DeserializeObject<List<PayableVM>>(result);

            }


            List<ReceivableVM> receivableVM = new List<ReceivableVM>();

            HttpResponseMessage resp = await client.GetAsync("api/Receivable");

            if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (resp.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = resp.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                receivableVM = JsonConvert.DeserializeObject<List<ReceivableVM>>(result);

            }


            List<AccountTypeVM> accountTypeVM = new List<AccountTypeVM>();

            HttpResponseMessage respo = await client.GetAsync("api/AccountType");

            if (respo.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (respo.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = respo.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                accountTypeVM = JsonConvert.DeserializeObject<List<AccountTypeVM>>(result);

            }
            ViewData["PayableId"] = new SelectList(payableVM, "PayableId", "PayableName");
            ViewData["ReceivablesId"] = new SelectList(receivableVM, "ReceivableId", "ReceivableName");
            ViewData["AccountTypeId"] = new SelectList(accountTypeVM, "AccountTypeId", "AccountTypeName");
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,AccountTitle,AccountCode,AccountDescription,ReceivablesId,PayableId,DateAdded,DateUpdated,IsActive,AccountTypeId")] TblAccountVM tblAccountVM)
        { 
            HttpClient client = _helperAPI.InitializeClient();
            if (ModelState.IsValid)
            {
                tblAccountVM.DateAdded = DateTime.Now;
                tblAccountVM.IsActive = true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var content = new StringContent(JsonConvert.SerializeObject(tblAccountVM), Encoding.UTF8, "application/json");
                //Task has been cancelled exception occured here, and Api method never hits while debugging
                HttpResponseMessage response = client.PostAsync("api/Account", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            List<PayableVM> payableVM = new List<PayableVM>();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res = await client.GetAsync("api/Payable");

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
                payableVM = JsonConvert.DeserializeObject<List<PayableVM>>(result);

            }


            List<ReceivableVM> receivableVM = new List<ReceivableVM>();

            HttpResponseMessage resp = await client.GetAsync("api/Receivable");

            if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (resp.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = resp.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                receivableVM = JsonConvert.DeserializeObject<List<ReceivableVM>>(result);

            }


            List<AccountTypeVM> accountTypeVM = new List<AccountTypeVM>();

            HttpResponseMessage respo = await client.GetAsync("api/AccountType");

            if (respo.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (respo.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = respo.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                accountTypeVM = JsonConvert.DeserializeObject<List<AccountTypeVM>>(result);

            }
            ViewData["PayableId"] = new SelectList(payableVM, "PayableId", "PayableName", tblAccountVM.PayableId);
            ViewData["ReceivablesId"] = new SelectList(receivableVM, "ReceivableId", "ReceivableName", tblAccountVM.ReceivablesId);
            ViewData["AccountTypeId"] = new SelectList(accountTypeVM, "AccountTypeId", "AccountTypeName", tblAccountVM.AccountTypeId);
            return View(tblAccountVM);
            //if (ModelState.IsValid)
            //{
            //    _context.Add(tblAccountVM);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(tblAccountVM);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblAccountVM tblAccountVM = new TblAccountVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync("api/Account/" + id);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                tblAccountVM = JsonConvert.DeserializeObject<TblAccountVM>(result);
            }

            if (tblAccountVM == null)
            {
                return NotFound();
            }
            List<PayableVM> payableVM = new List<PayableVM>();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res = await client.GetAsync("api/Payable");

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
                payableVM = JsonConvert.DeserializeObject<List<PayableVM>>(result);

            }


            List<ReceivableVM> receivableVM = new List<ReceivableVM>();

            HttpResponseMessage resp = await client.GetAsync("api/Receivable");

            if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (resp.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = resp.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                receivableVM = JsonConvert.DeserializeObject<List<ReceivableVM>>(result);

            }


            List<AccountTypeVM> accountTypeVM = new List<AccountTypeVM>();

            HttpResponseMessage respo = await client.GetAsync("api/AccountType");

            if (respo.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (respo.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = respo.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                accountTypeVM = JsonConvert.DeserializeObject<List<AccountTypeVM>>(result);

            }
            ViewData["PayableId"] = new SelectList(payableVM, "PayableId", "PayableName", tblAccountVM.PayableId);
            ViewData["ReceivablesId"] = new SelectList(receivableVM, "ReceivableId", "ReceivableName", tblAccountVM.ReceivablesId);
            ViewData["AccountTypeId"] = new SelectList(accountTypeVM, "AccountTypeId", "AccountTypeName", tblAccountVM.AccountTypeId);

            return View(tblAccountVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var tblAccountVM = await _context.TblAccountVM.FindAsync(id);
            //if (tblAccountVM == null)
            //{
            //    return NotFound();
            //}
            //return View(tblAccountVM);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AccountId,AccountTitle,AccountCode,AccountDescription,ReceivablesId,PayableId,DateAdded,DateUpdated,IsActive,AccountTypeId")] TblAccountVM tblAccountVM)
        {
            HttpClient client = _helperAPI.InitializeClient();
            if (id != tblAccountVM.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tblAccountVM.DateUpdated = DateTime.Now;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    var content = new StringContent(JsonConvert.SerializeObject(tblAccountVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync("api/Account/" + id, content);
                    if (response.IsSuccessStatusCode)
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

            List<PayableVM> payableVM = new List<PayableVM>();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res = await client.GetAsync("api/Payable");

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
                payableVM = JsonConvert.DeserializeObject<List<PayableVM>>(result);

            }


            List<ReceivableVM> receivableVM = new List<ReceivableVM>();

            HttpResponseMessage resp = await client.GetAsync("api/Receivable");

            if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (resp.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = resp.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                receivableVM = JsonConvert.DeserializeObject<List<ReceivableVM>>(result);

            }


            List<AccountTypeVM> accountTypeVM = new List<AccountTypeVM>();

            HttpResponseMessage respo = await client.GetAsync("api/AccountType");

            if (respo.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (respo.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = respo.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                accountTypeVM = JsonConvert.DeserializeObject<List<AccountTypeVM>>(result);

            }
            ViewData["PayableId"] = new SelectList(payableVM, "PayableId", "PayableName", tblAccountVM.PayableId);
            ViewData["ReceivablesId"] = new SelectList(receivableVM, "ReceivableId", "ReceivableName", tblAccountVM.ReceivablesId);
            ViewData["AccountTypeId"] = new SelectList(accountTypeVM, "AccountTypeId", "AccountTypeName", tblAccountVM.AccountTypeId);

            return View(tblAccountVM);
            //if (id != tblAccountVM.AccountId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(tblAccountVM);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!TblAccountVMExists(tblAccountVM.AccountId))
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
            //return View(tblAccountVM);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblAccountVM tblAccountVM = new TblAccountVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/Account/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                tblAccountVM = JsonConvert.DeserializeObject<TblAccountVM>(result);
            }
            if (tblAccountVM == null)
            {
                return NotFound();
            }

            return View(tblAccountVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var tblAccountVM = await _context.TblAccountVM
            //    .FirstOrDefaultAsync(m => m.AccountId == id);
            //if (tblAccountVM == null)
            //{
            //    return NotFound();
            //}

            //return View(tblAccountVM);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = client.DeleteAsync($"api/Account/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));
            //var tblAccountVM = await _context.TblAccountVM.FindAsync(id);
            //_context.TblAccountVM.Remove(tblAccountVM);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Ledger(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblAccountVM tblAccountVM = new TblAccountVM();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            //var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            HttpResponseMessage tblAccountVMRes = await client.GetAsync("api/Account/Ledger/" + id);

            if (tblAccountVMRes.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (tblAccountVMRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = tblAccountVMRes.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the Role list    
                tblAccountVM = JsonConvert.DeserializeObject<TblAccountVM>(result);

            }
            if (tblAccountVM == null)
            {
                return NotFound();
            }

            return View(tblAccountVM);
        }

        private bool TblAccountVMExists(long id)
        {
            return _context.TblAccountVM.Any(e => e.AccountId == id);
        }
    }
}
