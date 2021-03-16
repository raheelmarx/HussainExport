using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HussainExport.Client.Data;
using HussainExport.Client.Models;
using Microsoft.AspNetCore.Http;
using HussainExport.Client.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HussainExport.Client.Controllers
{
    //[ServiceFilter(typeof(AuthorizeAttribute))]
    public class SaleContractItemController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public SaleContractItemController(HEClientContext context)
        {
            _context = context;
        }

        // GET: SaleContractItem
        public async Task<IActionResult> Index()
        {

            List<SaleContractItemVM> saleContractItemVM = new List<SaleContractItemVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res = await client.GetAsync("api/SaleContractItems");

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
                saleContractItemVM = JsonConvert.DeserializeObject<List<SaleContractItemVM>>(result);

            }
            //returning the role list to view    
            return View(saleContractItemVM);
            //var hEClientContext = _context.SaleContractItemVM.Include(s => s.SaleContract).Include(s => s.Unit);
            //return View(await hEClientContext.ToListAsync());
        }

        // GET: SaleContractItem/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SaleContractItemVM saleContractItemVM = new SaleContractItemVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/SaleContractItems/" + id);

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
                saleContractItemVM = JsonConvert.DeserializeObject<SaleContractItemVM>(result);

            }
            if (saleContractItemVM == null)
            {
                return NotFound();
            }

            return View(saleContractItemVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var saleContractItemVM = await _context.SaleContractItemVM
            //    .Include(s => s.SaleContract)
            //    .Include(s => s.Unit)
            //    .FirstOrDefaultAsync(m => m.SaleContractItemId == id);
            //if (saleContractItemVM == null)
            //{
            //    return NotFound();
            //}

            //return View(saleContractItemVM);
        }

        // GET: SaleContractItem/Create
        public async Task<IActionResult> Create()
        {
            HttpClient client = _helperAPI.InitializeClient();
            #region Get DLL
            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

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


            List<UnitVM> unitVM = new List<UnitVM>();
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res2 = await client.GetAsync("api/Units");

            if (res2.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res2.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res2.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                unitVM = JsonConvert.DeserializeObject<List<UnitVM>>(result);

            }

            ViewData["UnitId"] = new SelectList(unitVM, "UnitId", "UnitName");
            #endregion
            return View();
            //ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId");
            //ViewData["UnitId"] = new SelectList(_context.UnitVM, "UnitId", "UnitId");
            //return View();
        }

        // POST: SaleContractItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleContractItemId,SaleContractId,Quality,Article,Color,Size,UnitId,Price,Quantity,Amount,IsActive,DateAdded,DateUpdated")] SaleContractItemVM saleContractItemVM)
        {
            HttpClient client = _helperAPI.InitializeClient();
            if (ModelState.IsValid)
            {
                saleContractItemVM.IsActive = true;
                saleContractItemVM.DateAdded = DateTime.Now;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var content = new StringContent(JsonConvert.SerializeObject(saleContractItemVM), Encoding.UTF8, "application/json");
                //Task has been cancelled exception occured here, and Api method never hits while debugging
                HttpResponseMessage res1 = client.PostAsync("api/SaleContractItems", content).Result;
                if (res1.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            #region Get DLL
            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

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


            List<UnitVM> unitVM = new List<UnitVM>();
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res2 = await client.GetAsync("api/Units");

            if (res2.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res2.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res2.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                unitVM = JsonConvert.DeserializeObject<List<UnitVM>>(result);

            }

            ViewData["UnitId"] = new SelectList(unitVM, "UnitId", "UnitName");
            #endregion
            //if (ModelState.IsValid)
            //{
            //    _context.Add(saleContractItemVM);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", saleContractItemVM.SaleContractId);
            //ViewData["UnitId"] = new SelectList(_context.UnitVM, "UnitId", "UnitId", saleContractItemVM.UnitId);
            return View(saleContractItemVM);
        }

        // GET: SaleContractItem/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SaleContractItemVM saleContractItemVM = new SaleContractItemVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res1 = await client.GetAsync("api/SaleContractItems/" + id);

            if (res1.IsSuccessStatusCode)
            {
                var result = res1.Content.ReadAsStringAsync().Result;
                saleContractItemVM = JsonConvert.DeserializeObject<SaleContractItemVM>(result);
            }

            if (saleContractItemVM == null)
            {
                return NotFound();
            }
            #region Get DLL
            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

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


            List<UnitVM> unitVM = new List<UnitVM>();
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res2 = await client.GetAsync("api/Units");

            if (res2.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res2.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res2.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                unitVM = JsonConvert.DeserializeObject<List<UnitVM>>(result);

            }

            ViewData["UnitId"] = new SelectList(unitVM, "UnitId", "UnitName");
            #endregion
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var saleContractItemVM = await _context.SaleContractItemVM.FindAsync(id);
            //if (saleContractItemVM == null)
            //{
            //    return NotFound();
            //}
            //ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", saleContractItemVM.SaleContractId);
            //ViewData["UnitId"] = new SelectList(_context.UnitVM, "UnitId", "UnitId", saleContractItemVM.UnitId);
            return View(saleContractItemVM);
        }

        // POST: SaleContractItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SaleContractItemId,SaleContractId,Quality,Article,Color,Size,UnitId,Price,Quantity,Amount,IsActive,DateAdded,DateUpdated")] SaleContractItemVM saleContractItemVM)
        {
            HttpClient client = _helperAPI.InitializeClient();
            if (id != saleContractItemVM.SaleContractItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    saleContractItemVM.IsActive = true;
                    saleContractItemVM.DateUpdated = DateTime.Now;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    var content = new StringContent(JsonConvert.SerializeObject(saleContractItemVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage res1 = client.PutAsync("api/SaleContracts/" + id, content).Result;
                    if (res1.IsSuccessStatusCode)
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
            #region Get DLL
            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

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


            List<UnitVM> unitVM = new List<UnitVM>();
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res2 = await client.GetAsync("api/Units");

            if (res2.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res2.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res2.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                unitVM = JsonConvert.DeserializeObject<List<UnitVM>>(result);

            }

            ViewData["UnitId"] = new SelectList(unitVM, "UnitId", "UnitName");
            #endregion
            //if (id != saleContractItemVM.SaleContractItemId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(saleContractItemVM);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!SaleContractItemVMExists(saleContractItemVM.SaleContractItemId))
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
            //ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", saleContractItemVM.SaleContractId);
            //ViewData["UnitId"] = new SelectList(_context.UnitVM, "UnitId", "UnitId", saleContractItemVM.UnitId);
            return View(saleContractItemVM);
        }

        // GET: SaleContractItem/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SaleContractItemVM saleContractItemVM = new SaleContractItemVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/SaleContractItems/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                saleContractItemVM = JsonConvert.DeserializeObject<SaleContractItemVM>(result);
            }
            if (saleContractItemVM == null)
            {
                return NotFound();
            }

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var saleContractItemVM = await _context.SaleContractItemVM
            //    .Include(s => s.SaleContract)
            //    .Include(s => s.Unit)
            //    .FirstOrDefaultAsync(m => m.SaleContractItemId == id);
            //if (saleContractItemVM == null)
            //{
            //    return NotFound();
            //}

            return View(saleContractItemVM);
        }

        // POST: SaleContractItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = client.DeleteAsync($"api/SaleContractItems/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));
            //var saleContractItemVM = await _context.SaleContractItemVM.FindAsync(id);
            //_context.SaleContractItemVM.Remove(saleContractItemVM);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SaleContractItemVMExists(long id)
        {
            if (id == 0)
            {
                return false;
            }

            SaleContractItemVM saleContractItemVM = new SaleContractItemVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/SaleContractItems/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                saleContractItemVM = JsonConvert.DeserializeObject<SaleContractItemVM>(result);
            }
            if (saleContractItemVM == null)
            {
                return false;
            }

            return true;
            //return _context.SaleContractItemVM.Any(e => e.SaleContractItemId == id);
        }
    }
}
