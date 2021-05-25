using HussainExport.Client.Helpers;
using HussainExport.Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Controllers
{
    //[ServiceFilter(typeof(AuthorizeAttribute))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                string token = TempData.Peek("Token").ToString();
                if (token == null)
                {
                    return RedirectToAction("SignIn", "User");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("SignIn", "User");
            }
           
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
