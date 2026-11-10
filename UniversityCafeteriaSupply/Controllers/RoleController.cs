using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace UniversityCafeteriaSupply.Controllers
{

    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }

        [HttpGet]
        public IActionResult addRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                ViewBag.Error = "Role name is required";
                return View();
            }

            bool isExist = await _roleManager.RoleExistsAsync(roleName);
            if (isExist)
            {
                ViewBag.Error = "This role already exists";
                return View();
            }

            var obj = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(obj);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = string.Join(" ", result.Errors.Select(e => e.Description));
            return View();
        }
    }
}
