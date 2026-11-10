using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteriaSupply.Data;
using UniversityCafeteriaSupply.Models;

namespace UniversityCafeteriaSupply.Controllers
{
    [Authorize]
    public class SupplyRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupplyRequestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<SupplyRequest> requests;

            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
            {
                // admins see all requests
                requests = _context.SupplyRequests.Include(r => r.User).ToList();
            }
            else
            {
                // a normal user sees only his requests
                var userId = _userManager.GetUserId(User);
                requests = _context.SupplyRequests.Where(r => r.UserId == userId).ToList();
            }

            return View(requests);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.Where(p => p.Quantity > 0).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(int[] productIds, int[] quantities)
        {
            var request = new SupplyRequest();
            request.Status = "Pending";
            request.RequestDate = DateTime.Now;
            request.UserId = _userManager.GetUserId(User)!;

            double total = 0;

            for (int i = 0; i < productIds.Length; i++)
            {
                if (quantities[i] <= 0)
                    continue;

                var product = _context.Products.Find(productIds[i]);
                if (product == null)
                    continue;

                if (quantities[i] > product.Quantity)
                {
                    ViewBag.Error = "Only " + product.Quantity + " of " + product.Name + " available.";
                    ViewBag.Products = _context.Products.Where(p => p.Quantity > 0).ToList();
                    return View();
                }

                var item = new RequestItem();
                item.ProductId = product.Id;
                item.Quantity = quantities[i];
                item.Price = product.Price;

                request.requestItems.Add(item);
                total += product.Price * quantities[i];
            }

            if (request.requestItems.Count == 0)
            {
                ViewBag.Error = "Please enter a quantity for at least one product.";
                ViewBag.Products = _context.Products.Where(p => p.Quantity > 0).ToList();
                return View();
            }

            request.TotalPrice = total;
            _context.SupplyRequests.Add(request);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var request = _context.SupplyRequests
                .Include(r => r.User)
                .Include(r => r.requestItems).ThenInclude(i => i.product)
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
                return NotFound();

            // a normal user can open only his own request
            var isAdmin = User.IsInRole("Admin") || User.IsInRole("SuperAdmin");
            if (!isAdmin && request.UserId != _userManager.GetUserId(User))
                return Forbid();

            return View(request);
        }

        // Edit = approve or reject (Admin and SuperAdmin only)
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var request = _context.SupplyRequests.Include(r => r.User).FirstOrDefault(r => r.Id == id);
            if (request == null)
                return NotFound();

            return View(request);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public IActionResult Edit(int id, string status)
        {
            var request = _context.SupplyRequests.Find(id);
            if (request == null)
                return NotFound();

            if (status == "Pending" || status == "Approved" || status == "Rejected")
            {
                request.Status = status;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var request = _context.SupplyRequests.Find(id);
            if (request == null)
                return NotFound();

            // admin can delete any request, a user only his own pending request
            var isAdmin = User.IsInRole("Admin") || User.IsInRole("SuperAdmin");
            var isOwner = request.UserId == _userManager.GetUserId(User);

            if (!isAdmin && !(isOwner && request.Status == "Pending"))
                return Forbid();

            _context.SupplyRequests.Remove(request);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
