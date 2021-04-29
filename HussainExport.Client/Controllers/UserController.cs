using HussainExport.Client.Helpers;
using HussainExport.Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HussainExport.Client.Controllers
{
    public class UserController : Controller
    {
        APIHelper _helperAPI = new APIHelper();

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(AuthenticateVM authenticateVM)
        {
            UserVM UserVM = new UserVM();

            HttpClient client = _helperAPI.InitializeClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var content = new StringContent(JsonConvert.SerializeObject(authenticateVM), Encoding.UTF8, "application/json");

            HttpResponseMessage UserVMRes = await client.PostAsync("api/users/authenticate", content);

            // HttpResponseMessage UserVMRes = await client.GetAsync("api/AspNetUserVMs/authenticate");

            //Checking the response is successful or not which is sent using HttpClient    
            if (UserVMRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = UserVMRes.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list    
                UserVM = JsonConvert.DeserializeObject<UserVM>(result);
                TempData["Token"] = UserVM.Token;
                HttpContext.Session.SetString("Token", UserVM.Token);
                //TempData["User"] = UserVM;
                //HttpContext.Session.SetString("Name", UserVM.FirstName + ' ' + UserVM.LastName);
                //HttpContext.Session.SetString("Role", UserVM.RoleId.ToString());
                //HttpContext.Session.SetString("token", UserVM.Token);
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(UserVM));

            }
            //return View( "~/Views/Home/Index", UserVM);
            //returning the employee list to view    
            //return RedirectToAction("Index", "Home",  UserVM.Token );
            return RedirectToAction("Index", "Home", new { Token = UserVM.Token });
        }

        //[ServiceFilter(typeof(AuthorizeAttribute))]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
            //HttpContext.Session.Remove("Name");
            //HttpContext.Session.Remove("Role");
            ViewBag.Message = "UserVM logged out successfully!";
            return RedirectToAction("SignIn", "User");
        }

        public IActionResult Welcome()
        {
            return View();
        }

        //[ServiceFilter(typeof(AuthorizeAttribute))]
        public async Task<IActionResult> Create()
        {
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
                ViewBag.RoleVM = JsonConvert.DeserializeObject<List<RoleVM>>(result);

            }

            return View();
        }
        // POST: AspNetRoles/Create  
        //[ServiceFilter(typeof(AuthorizeAttribute))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,UserName,Password,RoleId")] UserVM UserVM)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = _helperAPI.InitializeClient();
                //UserVM.Id = Guid.NewGuid().ToString();
                //var UserVMId = this.UserVM.FindFirstValue(ClaimTypes.NameIdentifier);
                var content = new StringContent(JsonConvert.SerializeObject(UserVM), Encoding.UTF8, "application/json");

                HttpResponseMessage aspNetUserVMsRes = client.PostAsync("api/Users", content).Result;
                if (aspNetUserVMsRes.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(UserVM);
        }

        //[ServiceFilter(typeof(AuthorizeAttribute))]
        public async Task<IActionResult> GetAll()
        {
            List<UserVM> UserVMs = new List<UserVM>();

            HttpClient client = _helperAPI.InitializeClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage UserVMsRes = await client.GetAsync("UserVMs/GetAll");

            if (UserVMsRes.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            //Checking the response is successful or not which is sent using HttpClient    
            if (UserVMsRes.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = UserVMsRes.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the Employee list    
                UserVMs = JsonConvert.DeserializeObject<List<UserVM>>(result);

            }
            //returning the employee list to view    
            return View(UserVMs);
        }

    }
}
