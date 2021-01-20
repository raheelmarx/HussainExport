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
    public class RoleController : Controller
    {
        private readonly HEClientContext _context;
        APIHelper _helperAPI = new APIHelper();

        public RoleController(HEClientContext context)
        {
            _context = context;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            List<RoleVM> roleVM = new List<RoleVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage roleVMsRes = await client.GetAsync("api/Roles");

            if (roleVMsRes.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (roleVMsRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = roleVMsRes.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the role list    
                roleVM = JsonConvert.DeserializeObject<List<RoleVM>>(result);

            }
            //returning the role list to view    
            return View(roleVM);
        }

        // GET: Role/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RoleVM roleVM = new RoleVM();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            //var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            HttpResponseMessage roleVMRes = await client.GetAsync("api/Roles/"+id);

            if (roleVMRes.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (roleVMRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = roleVMRes.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the Role list    
                roleVM = JsonConvert.DeserializeObject<RoleVM>(result);

            }
            if (roleVM == null)
            {
                return NotFound();
            }

            return View(roleVM);
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsActive,DateAdded,DateUpdated")] RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = _helperAPI.InitializeClient();

                var content = new StringContent(JsonConvert.SerializeObject(roleVM), Encoding.UTF8, "application/json");
                //Task has been cancelled exception occured here, and Api method never hits while debugging
                HttpResponseMessage res = client.PostAsync("api/Roles", content).Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(roleVM);
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RoleVM roleVM = new RoleVM();
            HttpClient client = _helperAPI.InitializeClient();
            HttpResponseMessage res = await client.GetAsync("api/roles/"+id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                roleVM = JsonConvert.DeserializeObject<RoleVM>(result);
            }

            if (roleVM == null)
            {
                return NotFound();
            }
            return View(roleVM);
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,IsActive,DateAdded,DateUpdated")] RoleVM roleVM)
        {
            if (id != roleVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    HttpClient client = _helperAPI.InitializeClient();

                    var content = new StringContent(JsonConvert.SerializeObject(roleVM), Encoding.UTF8, "application/json");
                    HttpResponseMessage res = client.PutAsync("api/roles", content).Result;
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
            return View(roleVM);
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RoleVM roleVM = new RoleVM();
            HttpClient client = _helperAPI.InitializeClient();
            HttpResponseMessage res = await client.GetAsync("api/roles/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                roleVM = JsonConvert.DeserializeObject<RoleVM>(result);
            }
            if (roleVM == null)
            {
                return NotFound();
            }

            return View(roleVM);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            HttpClient client = _helperAPI.InitializeClient();
            HttpResponseMessage res = client.DeleteAsync($"api/roles/{id}").Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RoleVMExists(long id)
        {
            if (id == 0)
            {
                return false;
            }

            RoleVM roleVM = new RoleVM();
            HttpClient client = _helperAPI.InitializeClient();
            HttpResponseMessage res = await client.GetAsync("api/roles/" + id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                roleVM = JsonConvert.DeserializeObject<RoleVM>(result);
            }
            if (roleVM == null)
            {
                return false;
            }

            return true;
        }
    }
}
