using Bistronger.Data;
using Bistronger.Data.Design;
using Bistronger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;
        private readonly IAdvertManager _advertManager;

        public HomeController(ApplicationDbContext db, ILogger<HomeController> logger, IAdvertManager advertManager)
        {
            _db = db;
            _logger = logger;
            _advertManager = advertManager;
        }

        // Search bar
        [AllowAnonymous]
        public IActionResult Index()
        {
            _advertManager.ReduceAdvertViewCount();
            HomeViewModel model = new HomeViewModel()
            {
                filter = new BusinessFilter()
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Contact()
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