using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleProject.Web.Models;
using SampleProject.Web.Models.ViewModels;
using SampleProject.Web.Services;
using System.Diagnostics;

namespace SampleProject.Web.Controllers
{
    public class HomeController(ILogger<HomeController> _logger,UserManager<IdentityUser> _userManager
        ,SignInManager<IdentityUser> _signInManager
        ,AppDbContext _context
        ,FileService fileService) : Controller
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
        public async Task<IActionResult> ProductList()
        {

            var user = await _userManager.FindByEmailAsync("ferhat123.ftr@gmail.com");

            if (_context.Products.Any(a=>a.UserId==user!.Id))
            {
                var products=_context.Products.Where(a => a.UserId == user!.Id).ToList();
                return View(products);  
                
                
            }


            var productList = new List<Product>() {

                new Product() { Name = "Car 1", Description = "Description 1", Price = 123, UserId = user!.Id },
                new Product() { Name = "Car 2", Description = "Description 2", Price = 123, UserId = user!.Id },
                new Product() { Name = "Car 3", Description = "Description 3", Price = 123, UserId = user!.Id },
                new Product() { Name = "Car 4", Description = "Description 4", Price = 123, UserId = user!.Id },
                new Product() { Name = "Car 5", Description = "Description 5", Price = 123, UserId = user!.Id },

            };

            _context.Products.AddRange(productList);
            _context.SaveChanges(); 
            


            return View(productList);      
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateExcel()
        {
            var response = new
            {
                Status = await fileService.AddMessageToQueue()
            };
            //burda isimsiz bir obje oluşturdum

            //ajaxla çağırdığımızdan burda json var
            return Json(response);  

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
