using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project2.Models;

namespace UniversityCafeteria.Controllers
{
    public class StudentController : Controller
    {
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RequestForm()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SaveUsingRequest()
        {

            string userName = Request.Form["txt_name"];
            string userAge = Request.Form["txt_age"];

            return Content("Name: " + userName + " Age: " + userAge);
        }


        [HttpGet]
        public IActionResult Collection()
        {
            return View();
        }

     
        [HttpPost]
        public IActionResult SaveUsingCollection(IFormCollection form)
        {
            string userName = form["txt_name"];
            string userAge = form["txt_age"];

            return Content("Name: " + userName + " Age: " + userAge);
        }

        [HttpGet]
        public IActionResult ModelBinding()
        {
            return View();
        }

      
        [HttpPost]
        public IActionResult SaveUsingModel(Student model)
        {
            string userName = model.Name;
            int userAge = model.Age;

            return Content("Name: " + userName + " Age: " + userAge);
        }
    }
}