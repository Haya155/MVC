
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityCafeteria7.Models;

namespace UniversityCafeteria7.Controllers
{
    public class OrderController : Controller
    {
        private readonly MyDbContext _context;

        public OrderController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FullName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Order ord)
        {
            _context.Orders.Add(ord);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);

            ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FullName");

            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order ord)
        {
            _context.Orders.Update(ord);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var order = _context.Orders.Find(id);
            return View(order);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);

            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
