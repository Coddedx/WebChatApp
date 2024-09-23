using ChapAppSignalR.Models;
using ChapAppSignalR.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChapAppSignalR.Controllers
{
    public class AccountController : Controller
    {
        private readonly ChatAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(ChatAppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            var result = new LoginViewModel();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) { return View(loginVM); }

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                await _signInManager.SignOutAsync();

                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["Error"] = "Wrong credentials. Please try again.";
                    return View(loginVM);
                }
            }
            else
            {
                TempData["Error"] = "Wrong credentials. Please try again.";
            }
            return View(loginVM);

        }

        public IActionResult Register()
        {
            var result = new RegisterViewModel();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use.";
                return View(registerVM);
            }

            var normalizedEmail = _userManager.NormalizeEmail(registerVM.EmailAddress);
            var newUser = new AppUser()
            {
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress,
                NormalizedEmail = normalizedEmail,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Gender = registerVM.Gender,
                PhoneNumber = registerVM.Phone,
                IdentityNumber = registerVM.IdentityNumber,
                Country = registerVM.Country,
            };

            var newUserResponse = await _userManager.CreateAsync(newUser,registerVM.Password);
                _context.SaveChanges();

            // BURAYI YAPAMADIM TEKRAR UĞRAŞ ????????????????????????????!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //if (newUserResponse.Succeeded)
            //{
            //    await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            //}
            //else
            //{
            //    TempData["Error"] = "Couldn't be created. Try again.";
            //    return View(registerVM);
            //}
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult>LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
