
using AcademyGraduationPreparation.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AcademyGraduationPreparation.Controllers
{
    public class SupplyRequestController : Controller
    {
        private readonly MyDbContext _context;

        public SupplyRequestController(MyDbContext context)
        {
            _context = context;
        }

        private List<AcademyGraduationPreparation.Models.Offer> GetOffers()
        {
            return _context.Offers
                .Include(o => o.Supplier)
                .ThenInclude(s => s.Category)
                .Select(o => new AcademyGraduationPreparation.Models.Offer
                {
                    Category = o.Supplier.Category.CategoryName,
                    SupplierName = o.Supplier.SupplierName,
                    OfferName = o.OfferName,
                    Quantity = o.Quantity,
                    UnitPrice = o.UnitPrice
                })
                .ToList();
        }

        public IActionResult Index()
        {
            ViewBag.Offers = GetOffers();

            return View();
        }

        [HttpPost]
        public IActionResult SelectOffer(string category, string supplierName,
            string offerName, int quantity, decimal unitPrice)
        {
            AcademyGraduationPreparation.Models.Offer offer =
                new AcademyGraduationPreparation.Models.Offer
                {
                    Category = category,
                    SupplierName = supplierName,
                    OfferName = offerName,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };

            if (category != "Certificates" &&
                category != "Hospitality" &&
                category != "Gifts")
            {
                return RedirectToAction("Index");
            }

            string jsonOffers = HttpContext.Session.GetString("SelectedOffers");

            List<AcademyGraduationPreparation.Models.Offer> selectedOffers =
                new List<AcademyGraduationPreparation.Models.Offer>();

            if (jsonOffers != null)
            {
                selectedOffers =
                    JsonSerializer.Deserialize<List<AcademyGraduationPreparation.Models.Offer>>(jsonOffers)
                    ?? new List<AcademyGraduationPreparation.Models.Offer>();
            }

            selectedOffers.RemoveAll(x => x.Category == category);

            selectedOffers.Add(offer);

            string updatedJson = JsonSerializer.Serialize(selectedOffers);

            HttpContext.Session.SetString("SelectedOffers", updatedJson);

            CookieOptions options = new CookieOptions();

            options.Expires = DateTime.Now.AddDays(7);

            Response.Cookies.Append("LastSelectedCategory", category, options);

            TempData["Message"] = "Offer selected successfully";

            return RedirectToAction("Index");
        }

        public IActionResult Review()
        {
            string jsonOffers = HttpContext.Session.GetString("SelectedOffers");

            if (jsonOffers == null)
            {
                TempData["Message"] = "Please select one offer from each category.";

                return RedirectToAction("Index");
            }

            List<AcademyGraduationPreparation.Models.Offer> selectedOffers =
                JsonSerializer.Deserialize<List<AcademyGraduationPreparation.Models.Offer>>(jsonOffers)
                ?? new List<AcademyGraduationPreparation.Models.Offer>();

            if (selectedOffers.Count != 3 ||
                !selectedOffers.Any(x => x.Category == "Certificates") ||
                !selectedOffers.Any(x => x.Category == "Hospitality") ||
                !selectedOffers.Any(x => x.Category == "Gifts"))
            {
                TempData["Message"] = "Please select one offer from each category.";

                return RedirectToAction("Index");
            }

            string lastCategory = Request.Cookies["LastSelectedCategory"];

            ViewBag.LastCategory = lastCategory;

            List<decimal> offerTotals = new List<decimal>();

            decimal totalPrice = 0;

            foreach (var selectedOffer in selectedOffers)
            {
                decimal offerTotal = selectedOffer.Quantity * selectedOffer.UnitPrice;

                offerTotals.Add(offerTotal);

                totalPrice += offerTotal;
            }

            decimal discountPersentage = 0;

            if (totalPrice > 500)
            {
                discountPersentage = 0.15m;
            }
            else if (totalPrice > 300)
            {
                discountPersentage = 0.10m;
            }

            decimal discountAmount = totalPrice * discountPersentage;

            decimal finalTotal = totalPrice - discountAmount;

            ViewBag.TotalPrice = totalPrice;
            ViewBag.DiscountPercentage = discountPersentage * 100;
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
