using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteria5.Data;
using UniversityCafeteria5.Models;

namespace UniversityCafeteria5.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var Supplie = _context.Suppliers.ToList();

            return View(Supplie);
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Supplier supply)
        {
            _context.Suppliers.Add(supply);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
