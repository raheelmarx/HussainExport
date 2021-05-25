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
    public class FabricPurchaseController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public FabricPurchaseController(HEClientContext context)
        {
            _context = context;
        }

        // GET: FabricPurchase
        public async Task<IActionResult> Index()
        {
            List<FabricPurchaseVM> fabricPurchaseVM = new List<FabricPurchaseVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage CurrencyVMsRes = await client.GetAsync("api/FabricPurchase");

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
                fabricPurchaseVM = JsonConvert.DeserializeObject<List<FabricPurchaseVM>>(result);

            }
            //returning the role list to view    
            return View(fabricPurchaseVM);
            //var hEClientContext = _context.FabricPurchaseVM.Include(f => f.SaleContract);
            //return View(await hEClientContext.ToListAsync());
        }

        // GET: FabricPurchase/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FabricPurchaseVM fabricPurchaseVM = new FabricPurchaseVM();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            //var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            HttpResponseMessage currencyVMRes = await client.GetAsync("api/FabricPurchase/" + id);

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
                fabricPurchaseVM = JsonConvert.DeserializeObject<FabricPurchaseVM>(result);

            }
            if (fabricPurchaseVM == null)
            {
                return NotFound();
            }

            return View(fabricPurchaseVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var fabricPurchaseVM = await _context.FabricPurchaseVM
            //    .Include(f => f.SaleContract)
            //    .FirstOrDefaultAsync(m => m.FabricPurchaseId == id);
            //if (fabricPurchaseVM == null)
            //{
            //    return NotFound();
            //}

            //return View(fabricPurchaseVM);
        }

        // GET: FabricPurchase/Create
        public async Task<IActionResult> Create()
        {
            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue(@"application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
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

        // POST: FabricPurchase/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FabricPurchaseId,SaleContractId,SaleContractNumber,Weaver,ContQuality,Gstquality,IsConversionContract,ConversionRate,PerPickRate,PerMeterRate,QuantityInMeters,Broker,DeliveryTime,Description,IsActive,DateAdded,DateUpdated")] FabricPurchaseVM fabricPurchaseVM)
        { 
            HttpClient client = _helperAPI.InitializeClient();
            if (ModelState.IsValid)
            {
               
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var content = new StringContent(JsonConvert.SerializeObject(fabricPurchaseVM), Encoding.UTF8, "application/json");
                //Task has been cancelled exception occured here, and Api method never hits while debugging
                HttpResponseMessage response = client.PostAsync("api/FabricPurchase", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                //_context.Add(fabricPurchaseVM);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();

            var contentType = new MediaTypeWithQualityHeaderValue(@"application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
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
            ViewData["SaleContractId"] = new SelectList(saleContractVM, "SaleContractId", "SaleContractNumber", fabricPurchaseVM.SaleContractId);
            return View(fabricPurchaseVM);
        }

        // GET: FabricPurchase/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var fabricPurchaseVM = await _context.FabricPurchaseVM.FindAsync(id);
            //if (fabricPurchaseVM == null)
            //{
            //    return NotFound();
            //}
            FabricPurchaseVM fabricPurchaseVM = new FabricPurchaseVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync("api/FabricPurchase/" + id);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                fabricPurchaseVM = JsonConvert.DeserializeObject<FabricPurchaseVM>(result);
            }

            if (fabricPurchaseVM == null)
            {
                return NotFound();
            }
            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();

            var contentType = new MediaTypeWithQualityHeaderValue(@"application/json");
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
            ViewData["SaleContractId"] = new SelectList(saleContractVM, "SaleContractId", "SaleContractNumber", fabricPurchaseVM.SaleContractId);
            //ViewData["SaleContractId"] = new SelectList(_context.SaleContractVM, "SaleContractId", "SaleContractId", fabricPurchaseVM.SaleContractId);
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
                    HttpClient client = _helperAPI.InitializeClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    var content = new StringContent(JsonConvert.SerializeObject(fabricPurchaseVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage res = await client.PutAsync("api/FabricPurchase/" + id, content);
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    //_context.Update(fabricPurchaseVM);
                    //await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!FabricPurchaseVMExists(fabricPurchaseVM.FabricPurchaseId))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
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

            FabricPurchaseVM fabricPurchaseVM = new FabricPurchaseVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/FabricPurchase/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                fabricPurchaseVM = JsonConvert.DeserializeObject<FabricPurchaseVM>(result);
            }
            if (fabricPurchaseVM == null)
            {
                return NotFound();
            }
            //var fabricPurchaseVM = await _context.FabricPurchaseVM
            //    .Include(f => f.SaleContract)
            //    .FirstOrDefaultAsync(m => m.FabricPurchaseId == id);
            //if (fabricPurchaseVM == null)
            //{
            //    return NotFound();
            //}

            return View(fabricPurchaseVM);
        }

        // POST: FabricPurchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = client.DeleteAsync($"api/FabricPurchase/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            //var fabricPurchaseVM = await _context.FabricPurchaseVM.FindAsync(id);
            //_context.FabricPurchaseVM.Remove(fabricPurchaseVM);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FabricPurchaseVMExists(long id)
        {
            return _context.FabricPurchaseVM.Any(e => e.FabricPurchaseId == id);
        }
    }
}
