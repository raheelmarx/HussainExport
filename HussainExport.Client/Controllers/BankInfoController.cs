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
    public class BankInfoController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public BankInfoController(HEClientContext context)
        {
            _context = context;
        }

        // GET: BankInfo
        public async Task<IActionResult> Index()
        {
            List<BankInfoVM> bankInfoVM = new List<BankInfoVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            //var token = TempData.Peek("Token").ToString();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());

            HttpResponseMessage CurrencyVMsRes = await client.GetAsync("api/BankInfo");

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
                bankInfoVM = JsonConvert.DeserializeObject<List<BankInfoVM>>(result);

            }
            //returning the role list to view    
            return View(bankInfoVM);
            // return View(await _context.BankInfoVM.ToListAsync());
        }

        // GET: BankInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BankInfoVM bankInfoVM = new BankInfoVM();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());

            //var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            HttpResponseMessage currencyVMRes = await client.GetAsync("api/BankInfo/" + id);

            if (currencyVMRes.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (currencyVMRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = currencyVMRes.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the Role list    
                bankInfoVM = JsonConvert.DeserializeObject<BankInfoVM>(result);

            }
            if (bankInfoVM == null)
            {
                return NotFound();
            }

            return View(bankInfoVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var bankInfoVM = await _context.BankInfoVM
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (bankInfoVM == null)
            //{
            //    return NotFound();
            //}

            //return View(bankInfoVM);
        }

        // GET: BankInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,BankName,AccountNo,Iban,BranchCode,BranchAddress,SwiftCode")] BankInfoVM bankInfoVM)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = _helperAPI.InitializeClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
                var content = new StringContent(JsonConvert.SerializeObject(bankInfoVM), Encoding.UTF8, "application/json");
                //Task has been cancelled exception occured here, and Api method never hits while debugging
                HttpResponseMessage res = client.PostAsync("api/BankInfo", content).Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(bankInfoVM);
            //if (ModelState.IsValid)
            //{
            //    _context.Add(bankInfoVM);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(bankInfoVM);
        }

        // GET: BankInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BankInfoVM bankInfoVM = new BankInfoVM();
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = await client.GetAsync("api/BankInfo/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                bankInfoVM = JsonConvert.DeserializeObject<BankInfoVM>(result);
            }

            if (bankInfoVM == null)
            {
                return NotFound();
            }
            return View(bankInfoVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var bankInfoVM = await _context.BankInfoVM.FindAsync(id);
            //if (bankInfoVM == null)
            //{
            //    return NotFound();
            //}
            //return View(bankInfoVM);
        }

        // POST: BankInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,BankName,AccountNo,Iban,BranchCode,BranchAddress,SwiftCode")] BankInfoVM bankInfoVM)
        {
            if (id != bankInfoVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    HttpClient client = _helperAPI.InitializeClient();
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
                    var content = new StringContent(JsonConvert.SerializeObject(bankInfoVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage res = await client.PutAsync("api/BankInfo/" + id, content);
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
            return View(bankInfoVM);
            //if (id != bankInfoVM.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(bankInfoVM);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!BankInfoVMExists(bankInfoVM.Id))
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
            //return View(bankInfoVM);
        }

        // GET: BankInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BankInfoVM bankInfoVM = new BankInfoVM();
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = await client.GetAsync("api/BankInfo/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                bankInfoVM = JsonConvert.DeserializeObject<BankInfoVM>(result);
            }
            if (bankInfoVM == null)
            {
                return NotFound();
            }

            return View(bankInfoVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var bankInfoVM = await _context.BankInfoVM
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (bankInfoVM == null)
            //{
            //    return NotFound();
            //}

            //return View(bankInfoVM);
        }

        // POST: BankInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = client.DeleteAsync($"api/BankInfo/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));
            //var bankInfoVM = await _context.BankInfoVM.FindAsync(id);
            //_context.BankInfoVM.Remove(bankInfoVM);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private bool BankInfoVMExists(int id)
        {
            return _context.BankInfoVM.Any(e => e.Id == id);
        }
    }
}
