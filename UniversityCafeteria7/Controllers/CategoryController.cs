using Microsoft.AspNetCore.Mvc;
using UniversityCafeteria7.Models;

namespace UniversityCafeteria7.Controllers
{
    public class CategoryController : Controller
    {
        private readonly MyDbContext _context;
        public CategoryController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();

            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(Category cat)
        {
            _context.Categories.Add(cat);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            return View(category);


        }
        [HttpPost]
        public IActionResult Edit(Category cat)
        {
            _context.Categories.Update(cat);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult Details(int id)
        {
            var category = _context.Categories.Find(id);
            return View(category);


        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
