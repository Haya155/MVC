using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace project_3.Controllers
{
    public class DemoController : Controller
    {
     
        public IActionResult Index()
        {
        
            HttpContext.Session.SetString("Username", "HayaAhmed");

         
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("UserTheme", "DarkTheme", options);

            string sessionUser = HttpContext.Session.GetString("Username");


            string cookieTheme = Request.Cookies["UserTheme"];

         
            ViewBag.SessionUser = sessionUser;
            ViewBag.CookieTheme = cookieTheme;

            return View();
        }

   
        [HttpGet]
        public IActionResult Calculator()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculator(double num1, double num2, string operation)
        {
            double result = 0;

            switch (operation)
            {
                case "+":
                    result = num1 + num2;
                    break;
                case "-":
                    result = num1 - num2;
                    break;
                case "*":
                    result = num1 * num2;
                    break;
                case "/":
                    if (num2 != 0)
                        result = num1 / num2;
                    else
                        ViewBag.Error = "Error: Division by zero is not allowed!";
                    break;
            }

            ViewBag.Result = result;
            return View();
        }

        // Another action to test @Url.Action redirection or links
        public IActionResult History()
        {
            return View();
        }
    }
}