using System.Diagnostics;
using Mediar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mediar.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
