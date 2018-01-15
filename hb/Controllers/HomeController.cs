using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hb.Models;
using Microsoft.AspNetCore.Identity;
using hb.Data;
using Microsoft.AspNetCore.Authorization;

namespace hb.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context) : base(userManager, context) { }
      
       //   public HomeController(UserManager<ApplicationUser> userManager) : base(userManager) { }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
