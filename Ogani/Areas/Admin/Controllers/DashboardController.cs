using Microsoft.AspNetCore.Mvc;

namespace Ogani.Areas.Admin.Controllers
{
    public class DashboardController : AdminController
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

