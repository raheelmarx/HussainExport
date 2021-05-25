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

namespace HussainExport.Client.Controllers
{
    //[ServiceFilter(typeof(AuthorizeAttribute))]
    public class CustomerController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public CustomerController(HEClientContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            List<CustomerVM> customerVM = new List<CustomerVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());

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
            //returning the role list to view    
            return View(customerVM);
            //return View(await _context.CustomerVM.ToListAsync());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomerVM customerVM = new CustomerVM();
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = await client.GetAsync("api/Customers/" + id);

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
                customerVM = JsonConvert.DeserializeObject<CustomerVM>(result);

            }
            if (customerVM == null)
            {
                return NotFound();
            }

            return View(customerVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var customerVM = await _context.CustomerVM
            //    .FirstOrDefaultAsync(m => m.CustomerId == id);
            //if (customerVM == null)
            //{
            //    return NotFound();
            //}

            //return View(customerVM);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,CustomerBussinessDetails,CustomerDescription,Contact,Address,Email,IsActive,DateAdded,DateUpdated")] CustomerVM customerVM)
        {
            if (ModelState.IsValid)
            {
                customerVM.DateAdded = DateTime.Now;
                customerVM.IsActive = true;
                HttpClient client = _helperAPI.InitializeClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
                var content = new StringContent(JsonConvert.SerializeObject(customerVM), Encoding.UTF8, "application/json");
                //Task has been cancelled exception occured here, and Api method never hits while debugging
                HttpResponseMessage res = client.PostAsync("api/Customers", content).Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(customerVM);
            //if (ModelState.IsValid)
            //{
            //    _context.Add(customerVM);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(customerVM);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomerVM customerVM = new CustomerVM();
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = await client.GetAsync("api/Customers/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                customerVM = JsonConvert.DeserializeObject<CustomerVM>(result);
            }

            if (customerVM == null)
            {
                return NotFound();
            }
            return View(customerVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var customerVM = await _context.CustomerVM.FindAsync(id);
            //if (customerVM == null)
            //{
            //    return NotFound();
            //}
            //return View(customerVM);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CustomerId,CustomerName,CustomerBussinessDetails,CustomerDescription,Contact,Address,Email,IsActive,DateAdded,DateUpdated")] CustomerVM customerVM)
        {
            if (id != customerVM.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customerVM.DateUpdated = DateTime.Now;
                    HttpClient client = _helperAPI.InitializeClient();
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
                    var content = new StringContent(JsonConvert.SerializeObject(customerVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage res = client.PutAsync("api/Customers/" + id, content).Result;
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
            return View(customerVM);
            //if (id != customerVM.CustomerId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(customerVM);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!CustomerVMExists(customerVM.CustomerId))
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
            //return View(customerVM);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomerVM customerVM = new CustomerVM();
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = await client.GetAsync("api/Customers/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                customerVM = JsonConvert.DeserializeObject<CustomerVM>(result);
            }
            if (customerVM == null)
            {
                return NotFound();
            }

            return View(customerVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var customerVM = await _context.CustomerVM
            //    .FirstOrDefaultAsync(m => m.CustomerId == id);
            //if (customerVM == null)
            //{
            //    return NotFound();
            //}

            //return View(customerVM);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = client.DeleteAsync($"api/Customers/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));
            //var customerVM = await _context.CustomerVM.FindAsync(id);
            //_context.CustomerVM.Remove(customerVM);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CustomerVMExists(long id)
        {
            if (id == 0)
            {
                return false;
            }

            CustomerVM customerVM = new CustomerVM();
            HttpClient client = _helperAPI.InitializeClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData.Peek("Token").ToString());
            HttpResponseMessage res = await client.GetAsync("api/Customers/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                customerVM = JsonConvert.DeserializeObject<CustomerVM>(result);
            }
            if (customerVM == null)
            {
                return false;
            }

            return true;
            //return _context.CustomerVM.Any(e => e.CustomerId == id);
        }
    }
}
