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
    //[ServiceFilter(typeof(AuthorizeAttribute))]
    public class CurrencyController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public CurrencyController(HEClientContext context)
        {
            _context = context;
        }

        // GET: Currency
        public async Task<IActionResult> Index()
        {
            List<CurrencyVM> CurrencyVM = new List<CurrencyVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage CurrencyVMsRes = await client.GetAsync("api/Currencies");

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
                CurrencyVM = JsonConvert.DeserializeObject<List<CurrencyVM>>(result);

            }
            //returning the role list to view    
            return View(CurrencyVM);
            //return View(await _context.CurrencyVM.ToListAsync());
        }

        // GET: Currency/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CurrencyVM currencyVM = new CurrencyVM();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            //var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            HttpResponseMessage currencyVMRes = await client.GetAsync("api/Currencies/" + id);

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
                currencyVM = JsonConvert.DeserializeObject<CurrencyVM>(result);

            }
            if (currencyVM == null)
            {
                return NotFound();
            }

            return View(currencyVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var currencyVM = await _context.CurrencyVM
            //    .FirstOrDefaultAsync(m => m.CurrencyId == id);
            //if (currencyVM == null)
            //{
            //    return NotFound();
            //}

            //return View(currencyVM);
        }

        // GET: Currency/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Currency/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CurrencyId,CurrencyName,CurrencySymbol,Description,IsActive,DateAdded,DateUpdated")] CurrencyVM currencyVM)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = _helperAPI.InitializeClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var content = new StringContent(JsonConvert.SerializeObject(currencyVM), Encoding.UTF8, "application/json");
                //Task has been cancelled exception occured here, and Api method never hits while debugging
                HttpResponseMessage res = client.PostAsync("api/Currencies", content).Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(currencyVM);
            //if (ModelState.IsValid)
            //{
            //    _context.Add(currencyVM);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(currencyVM);
        }

        // GET: Currency/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CurrencyVM currencyVM = new CurrencyVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/Currencies/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                currencyVM = JsonConvert.DeserializeObject<CurrencyVM>(result);
            }

            if (currencyVM == null)
            {
                return NotFound();
            }
            return View(currencyVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var currencyVM = await _context.CurrencyVM.FindAsync(id);
            //if (currencyVM == null)
            //{
            //    return NotFound();
            //}
            //return View(currencyVM);
        }

        // POST: Currency/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CurrencyId,CurrencyName,CurrencySymbol,Description,IsActive,DateAdded,DateUpdated")] CurrencyVM currencyVM)
        {
            if (id != currencyVM.CurrencyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    HttpClient client = _helperAPI.InitializeClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    var content = new StringContent(JsonConvert.SerializeObject(currencyVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage res = await client.PutAsync("api/Currencies/" + id, content);
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
            return View(currencyVM);
            //if (id != currencyVM.CurrencyId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(currencyVM);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!CurrencyVMExists(currencyVM.CurrencyId))
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
            //return View(currencyVM);
        }

        // GET: Currency/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CurrencyVM currencyVM = new CurrencyVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/Currencies/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                currencyVM = JsonConvert.DeserializeObject<CurrencyVM>(result);
            }
            if (currencyVM == null)
            {
                return NotFound();
            }

            return View(currencyVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var currencyVM = await _context.CurrencyVM
            //    .FirstOrDefaultAsync(m => m.CurrencyId == id);
            //if (currencyVM == null)
            //{
            //    return NotFound();
            //}

            //return View(currencyVM);
        }

        // POST: Currency/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res =  client.DeleteAsync($"api/Currencies/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));
            //var currencyVM = await _context.CurrencyVM.FindAsync(id);
            //_context.CurrencyVM.Remove(currencyVM);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CurrencyVMExists(int id)
        {
            if (id == 0)
            {
                return false;
            }

            CurrencyVM currencyVM = new CurrencyVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/Currencies/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                currencyVM = JsonConvert.DeserializeObject<CurrencyVM>(result);
            }
            if (currencyVM == null)
            {
                return false;
            }

            return true;
            //return _context.CurrencyVM.Any(e => e.CurrencyId == id);
        }
    }
}
