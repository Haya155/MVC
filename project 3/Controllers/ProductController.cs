using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityCafeteriaSupply.Data;
using UniversityCafeteriaSupply.Models;

namespace UniversityCafeteriaSupply.Controllers
{
    // any logged-in user can view products
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // ---- only Admin and SuperAdmin below ----

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public IActionResult Create(Product pro)
        {
            if (!ModelState.IsValid)
                return View(pro);

            _context.Products.Add(pro);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public IActionResult Edit(Product pro)
        {
            if (!ModelState.IsValid)
                return View(pro);

            _context.Products.Update(pro);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var pro = _context.Products.Find(id);
            if (pro == null)
                return NotFound();

            // deleting a used product would also delete its request items
            if (_context.RequestItems.Any(r => r.ProductId == id))
            {
                TempData["Error"] = "This product is used in supply requests and cannot be deleted.";
                return RedirectToAction("Index");
            }

            _context.Products.Remove(pro);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
