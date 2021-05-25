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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace HussainExport.Client.Controllers
{
    public class AccountTransactionController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public AccountTransactionController(HEClientContext context)
        {
            _context = context;
        }

        // GET: AccountTransaction
        public async Task<IActionResult> Index()
        {
            List<AccountTransactionVM> accountTransactionVM = new List<AccountTransactionVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());

            HttpResponseMessage res = await client.GetAsync("api/AccountTransaction");

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
                accountTransactionVM = JsonConvert.DeserializeObject<List<AccountTransactionVM>>(result);

            }
            //returning the role list to view    
            return View(accountTransactionVM);

            //var hEClientContext = _context.AccountTransactionVM.Include(a => a.AccountCredit).Include(a => a.AccountDebit).Include(a => a.SaleContract);
            //return View(await hEClientContext.ToListAsync());
        }

        // GET: AccountTransaction/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            AccountTransactionVM accountTransactionVM = new AccountTransactionVM();
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = await client.GetAsync("api/AccountTransaction/" + id);

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
                accountTransactionVM = JsonConvert.DeserializeObject<AccountTransactionVM>(result);

            }
            if (accountTransactionVM == null)
            {
                return NotFound();
            }

            return View(accountTransactionVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var accountTransactionVM = await _context.AccountTransactionVM
            //    .Include(a => a.AccountCredit)
            //    .Include(a => a.AccountDebit)
            //    .Include(a => a.SaleContract)
            //    .FirstOrDefaultAsync(m => m.AccountTransactionId == id);
            //if (accountTransactionVM == null)
            //{
            //    return NotFound();
            //}

            //return View(accountTransactionVM);
        }

        // GET: AccountTransaction/Create
        public async Task<IActionResult> Create()
        {
            List<TblAccountVM> tblAccountVM = new List<TblAccountVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

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
            ViewData["AccountCreditId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle");
            ViewData["AccountDebitId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle");



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

            List<TransactionTypeVM> transactionTypeVM = new List<TransactionTypeVM>();
            HttpResponseMessage respo = await client.GetAsync("api/TransactionType");
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
                transactionTypeVM = JsonConvert.DeserializeObject<List<TransactionTypeVM>>(result);

            }
            ViewData["Type"] = new SelectList(transactionTypeVM, "TransactionTypeId", "Description");
            return View();
        }

        // POST: AccountTransaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountTransactionId,Type,AccountDebitId,AccountCreditId,VoucherNo,VoucherDate,Narration,AmountDebit,AmountCredit,SaleContractId,SaleContractNumber")] AccountTransactionVM accountTransactionVM)//,DateAdded,DateUpdated,IsActive,AccountDebitCode,AccountCreditCode,
        {   
            HttpClient client = _helperAPI.InitializeClient();
            if (ModelState.IsValid)
            {
                accountTransactionVM.DateAdded = DateTime.Now;
                accountTransactionVM.IsActive = true;
             
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
                var content = new StringContent(JsonConvert.SerializeObject(accountTransactionVM), Encoding.UTF8, "application/json");
                HttpResponseMessage res = client.PostAsync("api/AccountTransaction", content).Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            List<TblAccountVM> tblAccountVM = new List<TblAccountVM>();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

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
            ViewData["AccountCreditId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle", accountTransactionVM.AccountCreditId);
            ViewData["AccountDebitId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle", accountTransactionVM.AccountCreditId);



            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();
            HttpResponseMessage resp = await client.GetAsync("api/SaleContracts");
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
                saleContractVM = JsonConvert.DeserializeObject<List<SaleContractVM>>(result);

            }
            ViewData["SaleContractId"] = new SelectList(saleContractVM, "SaleContractId", "SaleContractNumber", accountTransactionVM.SaleContractId);

            List<TransactionTypeVM> transactionTypeVM = new List<TransactionTypeVM>();
            HttpResponseMessage respo = await client.GetAsync("api/TransactionType");
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
                transactionTypeVM = JsonConvert.DeserializeObject<List<TransactionTypeVM>>(result);

            }
            ViewData["Type"] = new SelectList(transactionTypeVM, "TransactionTypeId", "Description", accountTransactionVM.Type);

            return View(accountTransactionVM);
            //if (ModelState.IsValid)
            //{
            //    _context.Add(accountTransactionVM);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["AccountCreditId"] = new SelectList(_context.TblAccountVM, "AccountId", "AccountId", accountTransactionVM.AccountCreditId);
            //ViewData["AccountDebitId"] = new SelectList(_context.TblAccountVM, "AccountId", "AccountId", accountTransactionVM.AccountDebitId);
            //ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", accountTransactionVM.SaleContractId);
            //return View(accountTransactionVM);
        }

        // GET: AccountTransaction/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccountTransactionVM accountTransactionVM = new AccountTransactionVM();
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = await client.GetAsync("api/AccountTransaction/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                accountTransactionVM = JsonConvert.DeserializeObject<AccountTransactionVM>(result);
            }

            if (accountTransactionVM == null)
            {
                return NotFound();
            }
            List<TblAccountVM> tblAccountVM = new List<TblAccountVM>();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

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
            ViewData["AccountCreditId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle", accountTransactionVM.AccountCreditId);
            ViewData["AccountDebitId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle", accountTransactionVM.AccountCreditId);



            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();
            HttpResponseMessage resp = await client.GetAsync("api/SaleContracts");
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
                saleContractVM = JsonConvert.DeserializeObject<List<SaleContractVM>>(result);

            }
            ViewData["SaleContractId"] = new SelectList(saleContractVM, "SaleContractId", "SaleContractNumber", accountTransactionVM.SaleContractId);


            List<TransactionTypeVM> transactionTypeVM = new List<TransactionTypeVM>();
            HttpResponseMessage respo = await client.GetAsync("api/TransactionType");
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
                transactionTypeVM = JsonConvert.DeserializeObject<List<TransactionTypeVM>>(result);

            }
            ViewData["Type"] = new SelectList(transactionTypeVM, "TransactionTypeId", "Description", accountTransactionVM.Type);

            return View(accountTransactionVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var accountTransactionVM = await _context.AccountTransactionVM.FindAsync(id);
            //if (accountTransactionVM == null)
            //{
            //    return NotFound();
            //}
            //ViewData["AccountCreditId"] = new SelectList(_context.TblAccountVM, "AccountId", "AccountId", accountTransactionVM.AccountCreditId);
            //ViewData["AccountDebitId"] = new SelectList(_context.TblAccountVM, "AccountId", "AccountId", accountTransactionVM.AccountDebitId);
            //ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", accountTransactionVM.SaleContractId);
            return View(accountTransactionVM);
        }

        // POST: AccountTransaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AccountTransactionId,Type,AccountDebitId,AccountCreditId,VoucherNo,VoucherDate,Narration,AmountDebit,AmountCredit,SaleContractId,SaleContractNumber,DateAdded,IsActive")] AccountTransactionVM accountTransactionVM)//,DateUpdated,AccountDebitCode,AccountCreditCode,
        {
            if (id != accountTransactionVM.AccountTransactionId)
            {
                return NotFound();
            }
            HttpClient client = _helperAPI.InitializeClient();
            if (ModelState.IsValid)
            {
                try
                {
                    accountTransactionVM.DateUpdated = DateTime.Now;
                    
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
                    var content = new StringContent(JsonConvert.SerializeObject(accountTransactionVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage res = client.PutAsync("api/AccountTransaction/" + id, content).Result;
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
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

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
            ViewData["AccountCreditId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle", accountTransactionVM.AccountCreditId);
            ViewData["AccountDebitId"] = new SelectList(tblAccountVM, "AccountId", "AccountTitle", accountTransactionVM.AccountCreditId);



            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();
            HttpResponseMessage resp = await client.GetAsync("api/SaleContracts");
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
                saleContractVM = JsonConvert.DeserializeObject<List<SaleContractVM>>(result);

            }
            ViewData["SaleContractId"] = new SelectList(saleContractVM, "SaleContractId", "SaleContractNumber", accountTransactionVM.SaleContractId);


            List<TransactionTypeVM> transactionTypeVM = new List<TransactionTypeVM>();
            HttpResponseMessage respo = await client.GetAsync("api/TransactionType");
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
                transactionTypeVM = JsonConvert.DeserializeObject<List<TransactionTypeVM>>(result);

            }
            ViewData["Type"] = new SelectList(transactionTypeVM, "TransactionTypeId", "Description", accountTransactionVM.Type);
            //if (id != accountTransactionVM.AccountTransactionId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(accountTransactionVM);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!AccountTransactionVMExists(accountTransactionVM.AccountTransactionId))
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
            //ViewData["AccountCreditId"] = new SelectList(_context.TblAccountVM, "AccountId", "AccountId", accountTransactionVM.AccountCreditId);
            //ViewData["AccountDebitId"] = new SelectList(_context.TblAccountVM, "AccountId", "AccountId", accountTransactionVM.AccountDebitId);
            //ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", accountTransactionVM.SaleContractId);
            return View(accountTransactionVM);
        }

        // GET: AccountTransaction/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccountTransactionVM accountTransactionVM = new AccountTransactionVM();
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = await client.GetAsync("api/AccountTransaction/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                accountTransactionVM = JsonConvert.DeserializeObject<AccountTransactionVM>(result);
            }
            if (accountTransactionVM == null)
            {
                return NotFound();
            }
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var accountTransactionVM = await _context.AccountTransactionVM
            //    .Include(a => a.AccountCredit)
            //    .Include(a => a.AccountDebit)
            //    .Include(a => a.SaleContract)
            //    .FirstOrDefaultAsync(m => m.AccountTransactionId == id);
            //if (accountTransactionVM == null)
            //{
            //    return NotFound();
            //}

            return View(accountTransactionVM);
        }

        // POST: AccountTransaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = client.DeleteAsync($"api/AccountTransaction/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));
            //var accountTransactionVM = await _context.AccountTransactionVM.FindAsync(id);
            //_context.AccountTransactionVM.Remove(accountTransactionVM);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private bool AccountTransactionVMExists(long id)
        {
            return _context.AccountTransactionVM.Any(e => e.AccountTransactionId == id);
        }
    }
}
