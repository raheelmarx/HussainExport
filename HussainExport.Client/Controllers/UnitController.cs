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
    public class UnitController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public UnitController(HEClientContext context)
        {
            _context = context;
        }

        // GET: Unit
        public async Task<IActionResult> Index()
        {
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
            //returning the role list to view    
            return View(unitVM);
            //return View(await _context.UnitVM.ToListAsync());
        }

        // GET: Unit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UnitVM unitVM = new UnitVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/Units/" + id);

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
                unitVM = JsonConvert.DeserializeObject<UnitVM>(result);

            }
            if (unitVM == null)
            {
                return NotFound();
            }

            return View(unitVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var unitVM = await _context.UnitVM
            //    .FirstOrDefaultAsync(m => m.UnitId == id);
            //if (unitVM == null)
            //{
            //    return NotFound();
            //}

            //return View(unitVM);
        }

        // GET: Unit/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Unit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnitId,UnitName,UnitSymbol,Description,IsActive,DateAdded,DateUpdated")] UnitVM unitVM)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = _helperAPI.InitializeClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var content = new StringContent(JsonConvert.SerializeObject(unitVM), Encoding.UTF8, "application/json");
                //Task has been cancelled exception occured here, and Api method never hits while debugging
                HttpResponseMessage res = client.PostAsync("api/Units", content).Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(unitVM);
            //if (ModelState.IsValid)
            //{
            //    _context.Add(unitVM);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(unitVM);
        }

        // GET: Unit/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UnitVM unitVM = new UnitVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/Units/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                unitVM = JsonConvert.DeserializeObject<UnitVM>(result);
            }

            if (unitVM == null)
            {
                return NotFound();
            }
            return View(unitVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var unitVM = await _context.UnitVM.FindAsync(id);
            //if (unitVM == null)
            //{
            //    return NotFound();
            //}
            //return View(unitVM);
        }

        // POST: Unit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UnitId,UnitName,UnitSymbol,Description,IsActive,DateAdded,DateUpdated")] UnitVM unitVM)
        {
            if (id != unitVM.UnitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    HttpClient client = _helperAPI.InitializeClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    var content = new StringContent(JsonConvert.SerializeObject(unitVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage res = client.PutAsync("api/Units/" + id, content).Result;
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
            return View(unitVM);
            //if (id != unitVM.UnitId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(unitVM);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!UnitVMExists(unitVM.UnitId))
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
            //return View(unitVM);
        }

        // GET: Unit/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UnitVM unitVM = new UnitVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/Units/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                unitVM = JsonConvert.DeserializeObject<UnitVM>(result);
            }
            if (unitVM == null)
            {
                return NotFound();
            }

            return View(unitVM);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var unitVM = await _context.UnitVM
            //    .FirstOrDefaultAsync(m => m.UnitId == id);
            //if (unitVM == null)
            //{
            //    return NotFound();
            //}

            //return View(unitVM);
        }

        // POST: Unit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = client.DeleteAsync($"api/Units/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));
            //var unitVM = await _context.UnitVM.FindAsync(id);
            //_context.UnitVM.Remove(unitVM);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UnitVMExists(int id)
        {
            if (id == 0)
            {
                return false;
            }

            UnitVM unitVM = new UnitVM();
            HttpClient client = _helperAPI.InitializeClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage res = await client.GetAsync("api/Units/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                unitVM = JsonConvert.DeserializeObject<UnitVM>(result);
            }
            if (unitVM == null)
            {
                return false;
            }

            return true;
            //return _context.UnitVM.Any(e => e.UnitId == id);
        }
    }
}
