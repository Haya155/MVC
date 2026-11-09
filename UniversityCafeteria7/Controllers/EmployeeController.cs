using Microsoft.AspNetCore.Mvc;
using UniversityCafeteria7.Models;

namespace UniversityCafeteria7.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MyDbContext _context;
        public EmployeeController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();

            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            _context.Employees.Add(emp);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employees = _context.Employees.Find(id);
            return View(employees);


        }
        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
            _context.Employees.Update(emp);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult Details(int id)
        {
            var employees = _context.Employees.Find(id);
            return View(employees);


        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employees = _context.Employees.Find(id);

            if (employees != null)
            {
                _context.Employees.Remove(employees);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
