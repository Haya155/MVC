using Microsoft.AspNetCore.Mvc;


namespace UniversityCafeteria3.Controllers
{
    public class SupplyController : Controller
    {
        public IActionResult Index()
        {
            string[] categories = { "Drinks", "Food", "Snacks", "Hot Drinks", "Cold Drinks" };
            ViewBag.Categories= categories;
            return View();
        }
        public IActionResult Products()
        {
            ViewData["Title"] = "Cafeteria Products";
            ViewData["Manager Name"] = "Ahmad Ali";
            ViewData["Num of Products"] = 5;
            ViewData["Products"] = new string[]
            {
                "Water", "Orange Juice", "Coffee", "Sandwich", "Chocolate"
            };
            return View();
        }
        public IActionResult Suppliers()
        {
            ViewBag.PageTitle = "Our Suppliers";
            ViewBag.NumOfSuppliers = 3;
            ViewBag.Suppliers = new string[]
                {
                "Fresh Food Company","Happy Drinks Company","Daily Supplies Company"

                };
                return View();
        }
        [HttpGet]
        public IActionResult CreateRequest()
        {

            return View();
        }
        [HttpPost]
        public IActionResult SubmitRequest()
        {
            string employeeName = Request.Form["EmployeeName"];
            string productName = Request.Form["ProductName"];
            string requiredDate = Request.Form["RequiredDate"];

            if (string.IsNullOrWhiteSpace(employeeName))
            {
                ViewBag.Error = "Employee Name is required!";
                return View("CreateRequest");
            }

            if (!int.TryParse(Request.Form["Quantity"], out int quantity) || quantity <= 0)
            {
                ViewBag.Error = "Please enter a valid quantity!";
                return View("CreateRequest");
            }

            TempData["Message"] = $"Request for {quantity} x {productName} submitted successfully!";

            return RedirectToAction("About");
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult CheckQuantity(int? quantity)
        {
            if (quantity.HasValue)
            {
                string result;
                if (quantity < 20) result = "Small Order";
                else if (quantity <= 50) result = "Medium Order";
                else result = "Large Order";

                ViewBag.Result = result;
                ViewBag.Quantity = quantity;
            }
            return View();
        }
    }
}
