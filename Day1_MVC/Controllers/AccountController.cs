using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Day1_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data.Data;
using Project.Data.Model;
using Project.Data.Repo;
//using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace Day1_MVC.Controllers
{
    public class AccountController : Controller
    {
     
        public UserManager<ApplicationUser> _userManager;
        public SignInManager<ApplicationUser> _signInManager;
        public RoleManager<IdentityRole> _roleManager;
        public IUnitOfWork _unitOfWork;
        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {

            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        #region Auth
        //public IActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginVm log)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = _context.Users
        //            .Include(u => u.Roles)
        //            .FirstOrDefault(u => u.UserName == log.UserName && u.Password == log.Password);
        //        if (user != null)
        //        {
        //            var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.UserName),
        //        new Claim("UserId", user.Id.ToString())
        //    };


        //            if (user.Roles != null)
        //            {
        //                foreach (var role in user.Roles)
        //                {
        //                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
        //                }
        //            }

        //            var ci = new ClaimsIdentity(claims, "Cookies");
        //            var cp = new ClaimsPrincipal(ci);

        //            await HttpContext.SignInAsync("Cookies", cp);
        //            if (user.Roles != null && user.Roles.Any(r => r.Name == "Student"))
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //            return RedirectToAction("Error", "Home");
        //        }
        //        ModelState.AddModelError("", "Invalid Username or Password.");
        //    }

        //    return View(log);
        //}
        //[HttpPost]

        //public async Task<IActionResult> Logout()
        //{

        //    await HttpContext.SignOutAsync("Cookies");


        //    return RedirectToAction("Index", "Home");
        //}
        #endregion
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    
                    await _userManager.AddToRoleAsync(user, model.Role); 
                   
                        var admin = new Admin
                        {
                            UserId = user.Id,
                            Name = model.UserName
                        };
                        _unitOfWork.AdminRepo.Add(admin);
                    

                    _unitOfWork.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVm.UserName, loginVm.Password, false, false);

                if (result.Succeeded)
                {
                    var appUser = await _userManager.FindByNameAsync(loginVm.UserName);

                    if (appUser != null && await _userManager.IsInRoleAsync(appUser, "Student"))
                    {
                        return RedirectToAction("Index", "Student");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid Username or Password.");
            }
            return View(loginVm);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
