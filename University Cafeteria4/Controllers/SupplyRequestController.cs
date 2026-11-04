using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;

namespace University_Cafeteria4.Controllers
{
    public class SupplyRequestController : Controller
    {
     
        private static List<string[]> confirmedRequests = new List<string[]>();
        private static int requestCounter = 1;

     
        private List<string[]> GetItems()
        {
            List<string[]> items = new List<string[]>();
            string saved = HttpContext.Session.GetString("Items");

            if (!string.IsNullOrEmpty(saved))
            {
                string[] rows = saved.Split(';', StringSplitOptions.RemoveEmptyEntries);
                foreach (string row in rows)
                {
                    items.Add(row.Split('|'));
                }
            }

            return items;
        }

       
        private void SaveItems(List<string[]> items)
        {
            List<string> rows = new List<string>();
            foreach (string[] item in items)
            {
                rows.Add(string.Join("|", item));
            }
            HttpContext.Session.SetString("Items", string.Join(";", rows));
        }

        public IActionResult Index()
        {
            string preferredPriority = Request.Cookies["PreferredPriority"];
            ViewData["PreferredPriority"] = preferredPriority;
            ViewData["Items"] = GetItems();
            return View();
        }

        [HttpPost]
        public IActionResult AddItem(string productName, string category, int quantity, decimal unitPrice, string priority)
        {
            if (string.IsNullOrWhiteSpace(productName) || quantity <= 0 || unitPrice <= 0)
            {
                TempData["Error"] = "Validation Error: Product name cannot be empty, and Quantity/Unit Price must be greater than zero.";
                return RedirectToAction("Index");
            }

            decimal itemTotal = quantity * unitPrice;

            List<string[]> items = GetItems();
            items.Add(new string[]
            {
                productName,
                category,
                quantity.ToString(),
                unitPrice.ToString(CultureInfo.InvariantCulture),
                itemTotal.ToString(CultureInfo.InvariantCulture)
            });

            SaveItems(items);
            HttpContext.Session.SetString("Priority", priority ?? "Normal");

            TempData["Message"] = "Product added to the request.";
            return RedirectToAction("Index");
        }

        private void CalculateCosts(decimal subtotal, string priority, out decimal prepFee, out decimal discount, out decimal finalTotal)
        {
            switch (priority)
            {
                case "Urgent":
                    prepFee = 5m;
                    break;
                case "Emergency":
                    prepFee = 10m;
                    break;
                case "Normal":
                default:
                    prepFee = 2m;
                    break;
            }

            if (subtotal > 200m)
            {
                discount = subtotal * 0.15m;
            }
            else if (subtotal > 100m)
            {
                discount = subtotal * 0.10m;
            }
            else
            {
                discount = 0m;
            }

            finalTotal = subtotal + prepFee - discount;
        }

  
        private decimal GetSubtotal(List<string[]> items)
        {
            decimal subtotal = 0;
            foreach (string[] item in items)
            {
                subtotal += decimal.Parse(item[4], CultureInfo.InvariantCulture);
            }
            return subtotal;
        }

        public IActionResult Review()
        {
            List<string[]> items = GetItems();
            decimal subtotal = GetSubtotal(items);

            string priority = HttpContext.Session.GetString("Priority") ?? "Normal";

            decimal prepFee, discount, finalTotal;
            CalculateCosts(subtotal, priority, out prepFee, out discount, out finalTotal);

            ViewData["Items"] = items;
            ViewData["Subtotal"] = subtotal;
            ViewData["Priority"] = priority;
            ViewData["PrepFee"] = prepFee;
            ViewData["Discount"] = discount;
            ViewData["FinalTotal"] = finalTotal;

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmRequest()
        {
            List<string[]> items = GetItems();

 
            if (items.Count == 0)
            {
                return RedirectToAction("Index");
            }

            decimal subtotal = GetSubtotal(items);
            string priority = HttpContext.Session.GetString("Priority") ?? "Normal";

            decimal prepFee, discount, finalTotal;
            CalculateCosts(subtotal, priority, out prepFee, out discount, out finalTotal);

            string[] reqData = new string[]
            {
                "REQ-" + requestCounter,
                items.Count.ToString(),
                subtotal.ToString(),
                prepFee.ToString(),
                discount.ToString(),
                finalTotal.ToString(),
                "Confirmed"
            };
            requestCounter++;

            confirmedRequests.Add(reqData);

       
            HttpContext.Session.Remove("Items");
            HttpContext.Session.Remove("Priority");

            return RedirectToAction("MyRequests");
        }

        [HttpPost]
        public IActionResult ClearDraft()
        {
            HttpContext.Session.Remove("Items");
            HttpContext.Session.Remove("Priority");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SavePreference(string priority)
        {
            if (!string.IsNullOrEmpty(priority))
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Append("PreferredPriority", priority, options);
                TempData["Message"] = $"Your preferred request priority is {priority}.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult MyRequests()
        {
            return View(confirmedRequests);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
