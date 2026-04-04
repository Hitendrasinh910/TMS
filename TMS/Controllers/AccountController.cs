using Microsoft.AspNetCore.Mvc;

namespace TMS.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        // Only Admins should ideally see this page
        public IActionResult UserRights()
        {
            return View();
        }
    }
}
