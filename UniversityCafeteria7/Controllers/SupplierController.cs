using Microsoft.AspNetCore.Mvc;
using UniversityCafeteria7.Models;

namespace UniversityCafeteria7.Controllers
{
    public class SupplierController : Controller
    {
        private readonly MyDbContext _context;
        public SupplierController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var suppliers = _context.Suppliers.ToList();

            return View(suppliers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(Supplier sup)
        {
            _context.Suppliers.Add(sup);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var suppliers = _context.Suppliers.Find(id);
            return View(suppliers);


        }
        [HttpPost]
        public IActionResult Edit(Supplier sup)
        {
            _context.Suppliers.Update(sup);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult Details(int id)
        {
            var suppliers = _context.Suppliers.Find(id);
            return View(suppliers);


        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var sup = _context.Suppliers.Find(id);

            if (sup != null)
            {
                _context.Suppliers.Remove(sup);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
