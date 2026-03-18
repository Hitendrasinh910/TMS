using Microsoft.AspNetCore.Mvc;

namespace TMS.Controllers
{
    public class MasterController : Controller
    {
        public IActionResult User() { return View(); }
        public IActionResult Country() { return View(); }
        public IActionResult State() { return View(); }
        public IActionResult City() { return View(); }
        public IActionResult PartyAccount() { return View(); }
        public IActionResult AccountType() { return View(); }
        public IActionResult BalanceType() { return View(); }
        public IActionResult BillToParty() { return View(); }
        public IActionResult Truck() { return View(); }
        public IActionResult Driver() { return View(); }
    }
}
