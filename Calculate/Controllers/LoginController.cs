using Calculate.Data;
using Calculate.Data.Models;
using Calculate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calculate.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> Index(TokenViewModel model)
        {
            if (String.IsNullOrEmpty(model.Token))
            {
                model.Message =  "Kullanıcı bulunamadı.";
                return View(model);
            }

            var User = _context.Users.FirstOrDefault(x => x.IsEnabled && x.AccessToken==model.Token);

            if (User == null)
            {
                model.Message = "Uygulama bulunamadı.";
                return View(model);
            }

            return RedirectToAction("Login", "Login", new { userId = User.UserId });
        }
    }
}
