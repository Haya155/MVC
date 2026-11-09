
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityCafeteria7.Models;

namespace UniversityCafeteria7.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyDbContext _context;

        public ProductController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.SupplierId = new SelectList(_context.Suppliers, "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product pro)
        {
            _context.Products.Add(pro);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var products = _context.Products.Find(id);

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.SupplierId = new SelectList(_context.Suppliers, "Id", "Name");

            return View(products);
        }

        [HttpPost]
        public IActionResult Edit(Product pro)
        {
            _context.Products.Update(pro);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var products = _context.Products.Find(id);
            return View(products);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var pro = _context.Products.Find(id);

            if (pro != null)
            {
                _context.Products.Remove(pro);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
