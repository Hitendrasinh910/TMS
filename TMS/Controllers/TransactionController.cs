using Microsoft.AspNetCore.Mvc;

namespace TMS.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult LREntry()
        {
            return View();
        }

        public IActionResult LRList()
        {
            return View();
        }

        public IActionResult PaymentReceive()
        {
            return View();
        }

        public IActionResult BillEntry()
        {
            return View();
        }

        // The List Page
        public IActionResult BillList()
        {
            return View();
        }

        public IActionResult ChallanEntry()
        {
            return View();
        }

        // The List Page
        public IActionResult ChallanList()
        {
            return View();
        }
    }
}
