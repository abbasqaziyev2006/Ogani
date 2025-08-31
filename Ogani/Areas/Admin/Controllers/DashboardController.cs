using Microsoft.AspNetCore.Mvc;

namespace Ogani.Areas.Admin.Controllers
{
    public class DashboardController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

