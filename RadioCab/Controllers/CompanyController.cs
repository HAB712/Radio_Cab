using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RadioCab.Controllers
{
    public class CompanyController : Controller
    {
        [Authorize(Roles = "Company")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
