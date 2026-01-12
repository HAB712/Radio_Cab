using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RadioCab.Controllers
{
    public class DriverController : Controller
    {
        [Authorize(Roles = "Driver")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
