using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HussainExport.Client.Controllers
{
    //[ServiceFilter(typeof(AuthorizeAttribute))]
    public class SaleContractController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public SaleContractController(HEClientContext context)
        {
            _context = context;
        }

        // GET: SaleContract
        public async Task<IActionResult> Index()
        {
            List<SaleContractVM> saleContractVM = new List<SaleContractVM>();

            HttpClient client = _helperAPI.InitializeClient();

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
            //returning the role list to view    
            return View(saleContractVM);
            //var hEClientContext = _context.SaleContractVM.Include(s => s.Currency).Include(s => s.Customer);
            //return View(await hEClientContext.ToListAsync());
        }

        // GET: SaleContract/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SaleContractVM saleContractVM = new SaleContractVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/SaleContracts/" + id);

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
                saleContractVM = JsonConvert.DeserializeObject<SaleContractVM>(result);

            }
            if (saleContractVM == null)
            {
                return NotFound();
            }
            #region   Get Sale Contract Items
            List<SaleContractItemVM> saleContractItemVM = new List<SaleContractItemVM>();

       

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res1 = await client.GetAsync("api/SaleContractItems/GetSaleContractItemBySaleContractId/" + id);

            if (res1.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res1.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res1.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                saleContractItemVM = JsonConvert.DeserializeObject<List<SaleContractItemVM>>(result);
                foreach( var item in saleContractItemVM)
                {
                    #region Unit
                    UnitVM unitVM = new UnitVM();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    HttpResponseMessage resUnit = await client.GetAsync("api/Units/" + item.UnitId);

                    if (resUnit.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ViewBag.Message = "Unauthorized!";
                    }

                    //Checking the response is successful or not which is sent using HttpClient    
                    if (resUnit.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api     
                        var resultUnit = resUnit.Content.ReadAsStringAsync().Result;


                        //Deserializing the response recieved from web api and storing into the Role list    
                        unitVM = JsonConvert.DeserializeObject<UnitVM>(resultUnit);

                        item.Unit = unitVM;
                    }
                    if (unitVM == null)
                    {
                        return NotFound();
                    }

                    #endregion
                }

            }
            //returning the role list to view 
            #endregion

            saleContractVM.SaleContractItem = saleContractItemVM;
            #region Customer Information
            
            CustomerVM customerVM = new CustomerVM();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res2 = await client.GetAsync("api/Customers/" + saleContractVM.CustomerId);

            if (res2.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (res2.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res2.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the Role list    
                customerVM = JsonConvert.DeserializeObject<CustomerVM>(result);

            }
            if (customerVM == null)
            {
                return NotFound();
            }
            saleContractVM.Customer = customerVM;
            #endregion

            #region Currency
            CurrencyVM currencyVM = new CurrencyVM();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            //var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            HttpResponseMessage currencyVMRes = await client.GetAsync("api/Currencies/" + saleContractVM.CurrencyId);

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

            saleContractVM.Currency = currencyVM;
            #endregion

            
            return View(saleContractVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var saleContractVM = await _context.SaleContractVM
            //    .Include(s => s.Currency)
            //    .Include(s => s.Customer)
            //    .FirstOrDefaultAsync(m => m.SaleContractId == id);
            //if (saleContractVM == null)
            //{
            //    return NotFound();
            //}

            //return View(saleContractVM);
        }

        // GET: SaleContract/Create
        public async Task<IActionResult> Create()
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

            ViewData["CurrencyId"] = new SelectList(CurrencyVM, "CurrencyId", "CurrencyName");




            List<CustomerVM> customerVM = new List<CustomerVM>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res = await client.GetAsync("api/Customers");

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
                customerVM = JsonConvert.DeserializeObject<List<CustomerVM>>(result);

            }

            ViewData["CustomerId"] = new SelectList(customerVM, "CustomerId", "CustomerName");
            return View();
        }

        // POST: SaleContract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleContractId,SaleContractNumber,CustomerId,TotalAmount,CurrencyId,DeliveryTime,ShipmentDetails,Packing,Description,IsActive,DateAdded,DateUpdated")] SaleContractVM saleContractVM)
        { 
            HttpClient client = _helperAPI.InitializeClient();
            if (ModelState.IsValid)
            {
               
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var content = new StringContent(JsonConvert.SerializeObject(saleContractVM), Encoding.UTF8, "application/json");
                //Task has been cancelled exception occured here, and Api method never hits while debugging
                HttpResponseMessage res = client.PostAsync("api/SaleContracts", content).Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            List<CurrencyVM> CurrencyVM = new List<CurrencyVM>();


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

            ViewData["CurrencyId"] = new SelectList(CurrencyVM, "CurrencyId", "CurrencyName");




            List<CustomerVM> customerVM = new List<CustomerVM>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage resCust = await client.GetAsync("api/Customers");

            if (resCust.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (resCust.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = resCust.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                customerVM = JsonConvert.DeserializeObject<List<CustomerVM>>(result);

            }

            ViewData["CustomerId"] = new SelectList(customerVM, "CustomerId", "CustomerName");
            return View(saleContractVM);
            //if (ModelState.IsValid)
            //{
            //    _context.Add(saleContractVM);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["CurrencyId"] = new SelectList(_context.Set<CurrencyVM>(), "CurrencyId", "CurrencyId", saleContractVM.CurrencyId);
            //ViewData["CustomerId"] = new SelectList(_context.Set<CustomerVM>(), "CustomerId", "CustomerId", saleContractVM.CustomerId);
            //return View(saleContractVM);
        }

        // GET: SaleContract/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SaleContractVM saleContractVM = new SaleContractVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/SaleContracts/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                saleContractVM = JsonConvert.DeserializeObject<SaleContractVM>(result);
            }

            if (saleContractVM == null)
            {
                return NotFound();
            }

            //Get DDL
            #region Get DDL
            List<CurrencyVM> CurrencyVM = new List<CurrencyVM>();

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

            ViewData["CurrencyId"] = new SelectList(CurrencyVM, "CurrencyId", "CurrencyName");




            List<CustomerVM> customerVM = new List<CustomerVM>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage resCust = await client.GetAsync("api/Customers");

            if (resCust.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (resCust.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = resCust.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                customerVM = JsonConvert.DeserializeObject<List<CustomerVM>>(result);

            }

            ViewData["CustomerId"] = new SelectList(customerVM, "CustomerId", "CustomerName");
            #endregion Get DDL
            //Get DDL End
            return View(saleContractVM);

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var saleContractVM = await _context.SaleContractVM.FindAsync(id);
            //if (saleContractVM == null)
            //{
            //    return NotFound();
            //}
            //ViewData["CurrencyId"] = new SelectList(_context.Set<CurrencyVM>(), "CurrencyId", "CurrencyId", saleContractVM.CurrencyId);
            //ViewData["CustomerId"] = new SelectList(_context.Set<CustomerVM>(), "CustomerId", "CustomerId", saleContractVM.CustomerId);
            //return View(saleContractVM);
        }

        // POST: SaleContract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SaleContractId,SaleContractNumber,CustomerId,TotalAmount,CurrencyId,DeliveryTime,ShipmentDetails,Packing,Description,IsActive,DateAdded,DateUpdated")] SaleContractVM saleContractVM)
        {
            HttpClient client = _helperAPI.InitializeClient();
            if (id != saleContractVM.SaleContractId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    var content = new StringContent(JsonConvert.SerializeObject(saleContractVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage res = client.PutAsync("api/SaleContracts/" + id, content).Result;
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
          
            //Get DDL
            #region Get DDL
            List<CurrencyVM> CurrencyVM = new List<CurrencyVM>();

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

            ViewData["CurrencyId"] = new SelectList(CurrencyVM, "CurrencyId", "CurrencyName");




            List<CustomerVM> customerVM = new List<CustomerVM>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage resCust = await client.GetAsync("api/Customers");

            if (resCust.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (resCust.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = resCust.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                customerVM = JsonConvert.DeserializeObject<List<CustomerVM>>(result);

            }

            ViewData["CustomerId"] = new SelectList(customerVM, "CustomerId", "CustomerName");
            #endregion Get DDL
            //Get DDL End

            return View(customerVM);

            //if (id != saleContractVM.SaleContractId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(saleContractVM);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!SaleContractVMExists(saleContractVM.SaleContractId))
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
            //ViewData["CurrencyId"] = new SelectList(_context.Set<CurrencyVM>(), "CurrencyId", "CurrencyId", saleContractVM.CurrencyId);
            //ViewData["CustomerId"] = new SelectList(_context.Set<CustomerVM>(), "CustomerId", "CustomerId", saleContractVM.CustomerId);
            //return View(saleContractVM);
        }

        // GET: SaleContract/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SaleContractVM saleContractVM = new SaleContractVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/SaleContracts/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                saleContractVM = JsonConvert.DeserializeObject<SaleContractVM>(result);
            }
            if (saleContractVM == null)
            {
                return NotFound();
            }

            return View(saleContractVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var saleContractVM = await _context.SaleContractVM
            //    .Include(s => s.Currency)
            //    .Include(s => s.Customer)
            //    .FirstOrDefaultAsync(m => m.SaleContractId == id);
            //if (saleContractVM == null)
            //{
            //    return NotFound();
            //}

            //return View(saleContractVM);
        }

        // POST: SaleContract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = client.DeleteAsync($"api/SaleContracts/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));
            //var saleContractVM = await _context.SaleContractVM.FindAsync(id);
            //_context.SaleContractVM.Remove(saleContractVM);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SaleContractVMExists(long id)
        {
            if (id == 0)
            {
                return false;
            }

            SaleContractVM saleContractVM = new SaleContractVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/SaleContracts/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                saleContractVM = JsonConvert.DeserializeObject<SaleContractVM>(result);
            }
            if (saleContractVM == null)
            {
                return false;
            }

            return true;
            //return _context.SaleContractVM.Any(e => e.SaleContractId == id);
        }
        public async Task<IActionResult> AddItem()
        {
            SaleContractItemVM saleContractItemVM = new SaleContractItemVM();
            List<UnitVM> unitVM = new List<UnitVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage res = await client.GetAsync("api/Units");

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
                unitVM = JsonConvert.DeserializeObject<List<UnitVM>>(result);

            }
            ViewData["UnitId"] = new SelectList(unitVM, "UnitId", "UnitName", saleContractItemVM.UnitId);
            //return PartialView("_PartialItem", saleContractItemVM);
            return new PartialViewResult
            {
                ViewName = "_PartialItem",
                ViewData = new ViewDataDictionary<SaleContractItemVM>(ViewData, saleContractItemVM)
            };
        }

    }
}
