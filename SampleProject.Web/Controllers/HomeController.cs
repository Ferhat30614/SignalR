using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleProject.Web.Models;
using SampleProject.Web.Models.ViewModels;
using System.Diagnostics;
using System.Numerics;

namespace SampleProject.Web.Controllers
{
    public class HomeController(ILogger<HomeController> _logger,UserManager<IdentityUser> _userManager,SignInManager<IdentityUser> _signInManager) : Controller
    {
       

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();      
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var hasUser=await _userManager.FindByEmailAsync(model.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "username or password  is wrong");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(hasUser, model.Password, true, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "username or password  is wrong");
                return View(model);
            }




            return RedirectToAction(nameof(Index));   
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();      
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if(!ModelState.IsValid) return View(model);

            var createUser = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result=await _userManager.CreateAsync(createUser,model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }


            return RedirectToAction(nameof(SignIn));     // normal burda "SignIn" yazılabilirdi tabiki ama bu nameofun artıları var nameof(SignIn) kullandığında:

            //Refactoring(yeniden adlandırma) yaptığında otomatik güncellenir.
            //Yazım hatalarını compile - time’da fark eder.
        }
        public IActionResult ProductList()
        {
            return View();      
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
