using ChapAppSignalR.Models;
using ChapAppSignalR.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChapAppSignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly ChatAppDbContext _context;


        public HomeController(ILogger<HomeController> logger, ChatAppDbContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Oturum açmýþ kullanýcýnýn ID sini al
            var userId = _userManager.GetUserId(User);

            // Kullanýcýyý UserManager ile al
            var user = await _userManager.FindByIdAsync(userId);

            var users = _userManager.Users.ToList();

            // Kullanýcýlarý UserViewModel e map et
            var userVM = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            }).ToList(); //ýndex te list olarak kullanacaðýmýz için 

            return View(userVM);
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
