
using AcademyGraduationPreparation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AcademyGraduationPreparation.Controllers
{
    public class SupplyRequestController : Controller
    {
        private List<Offer> GetOffers()
        {
            return new List<Offer>
            {
                new Offer
                {
                    Category = "Certificates",
                    SupplierName = "Supplier A",
                    OfferName = "Premium Certificate Package",
                    Quantity = 35,
                    UnitPrice = 2
                },

                new Offer
                {
                    Category = "Certificates",
                    SupplierName = "Supplier B",
                    OfferName = "Standard Certificate Package",
                    Quantity = 35,
                    UnitPrice = 1.5m
                },

                new Offer
                {
                    Category = "Certificates",
                    SupplierName = "Supplier C",
                    OfferName = "Luxury Certificate Package",
                    Quantity = 35,
                    UnitPrice = 2.5m
                },

                new Offer
                {
                    Category = "Hospitality",
                    SupplierName = "Supplier D",
                    OfferName = "Coffee & Drinks Package",
                    Quantity = 100,
                    UnitPrice = 2
                },

                new Offer
                {
                    Category = "Hospitality",
                    SupplierName = "Supplier E",
                    OfferName = "Sandwiches & Snacks Package",
                    Quantity = 100,
                    UnitPrice = 3
                },

                new Offer
                {
                    Category = "Hospitality",
                    SupplierName = "Supplier F",
                    OfferName = "Full Hospitality Package",
                    Quantity = 100,
                    UnitPrice = 4
                },

                new Offer
                {
                    Category = "Gifts",
                    SupplierName = "Supplier G",
                    OfferName = "Customized Mugs Package",
                    Quantity = 35,
                    UnitPrice = 2
                },

                new Offer
                {
                    Category = "Gifts",
                    SupplierName = "Supplier H",
                    OfferName = "Graduation Bags Package",
                    Quantity = 35,
                    UnitPrice = 3
                },

                new Offer
                {
                    Category = "Gifts",
                    SupplierName = "Supplier I",
                    OfferName = "Premium Gift Boxes Package",
                    Quantity = 35,
                    UnitPrice = 4
                }
            };
        }

       
        public IActionResult Index()
        {
            ViewBag.Offers = GetOffers();

            return View();
        }

        [HttpPost]
        public IActionResult SelectOffer(
            string category,
            string supplierName,
            string offerName,
            int quantity,
            decimal unitPrice)
        {
            
            Offer offer = GetOffers().FirstOrDefault(x => x.Category == category && x.SupplierName == supplierName && x.OfferName == offerName && x.Quantity == quantity &&x.UnitPrice == unitPrice);

            if (offer == null)
            {
                return RedirectToAction("Index");
            }


            string jsonOffers =
                HttpContext.Session.GetString("SelectedOffers");

            List<Offer> selectedOffers = new List<Offer>();

            if (jsonOffers != null)
            {
                selectedOffers =
                    JsonSerializer.Deserialize<List<Offer>>(jsonOffers)
                    ?? new List<Offer>();
            }

            
            selectedOffers.RemoveAll(x => x.Category == category);

            
            selectedOffers.Add(offer);

            // Convert List to JSON
            string updatedJson =
                JsonSerializer.Serialize(selectedOffers);

            // Save List in Session
            HttpContext.Session.SetString("SelectedOffers", updatedJson);

            CookieOptions options = new CookieOptions();

            options.Expires = DateTime.Now.AddDays(7);

            Response.Cookies.Append(
                "LastSelectedCategory",
                category,
                options);

            TempData["Message"] = "Offer selected successfully";

            return RedirectToAction("Index");
        }

        public IActionResult Review()
        {
            string jsonOffers =
                HttpContext.Session.GetString("SelectedOffers");

            if (jsonOffers == null)
            {
                TempData["Message"] =
                    "Please select one offer from each category.";

                return RedirectToAction("Index");
            }

            // Convert JSON to List
            List<Offer> selectedOffers =
                JsonSerializer.Deserialize<List<Offer>>(jsonOffers)
                ?? new List<Offer>();

            // Check if all categories have one selected offer
            if (selectedOffers.Count != 3 ||
                !selectedOffers.Any(x => x.Category == "Certificates") ||
                !selectedOffers.Any(x => x.Category == "Hospitality") ||
                !selectedOffers.Any(x => x.Category == "Gifts"))
            {
                TempData["Message"] =
                    "Please select one offer from each category.";

                return RedirectToAction("Index");
            }

            string lastCategory =
                Request.Cookies["LastSelectedCategory"];

            ViewBag.LastCategory = lastCategory;

            List<decimal> offerTotals = new List<decimal>();

            decimal totalPrice = 0;

            foreach (var selectedOffer in selectedOffers)
            {
                decimal offerTotal =
                    selectedOffer.Quantity * selectedOffer.UnitPrice;

                offerTotals.Add(offerTotal);

                totalPrice += offerTotal;
            }

            decimal discountPercentage = 0;

            if (totalPrice > 500)
            {
                discountPercentage = 0.15m;
            }
            else if (totalPrice > 300)
            {
                discountPercentage = 0.10m;
            }

            decimal discountAmount =
                totalPrice * discountPercentage;

            decimal finalTotal =
                totalPrice - discountAmount;

          
            ViewBag.TotalPrice = totalPrice;
            ViewBag.DiscountPercentage = discountPercentage * 100;
            ViewBag.DiscountAmount = discountAmount;
            ViewBag.FinalTotal = finalTotal;
            ViewBag.OfferTotals = offerTotals;

            return View(selectedOffers);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
