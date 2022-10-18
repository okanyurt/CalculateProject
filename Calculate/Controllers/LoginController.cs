using Calculate.Data;
using Calculate.Data.Models;
using Calculate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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

            //private readonly CookieOptions _cookieOptionsForAuthentication = new() { Path = "/", Expires = DateTime.Now.AddDays(1) };

            //Response.Cookies.Append("AuthenticationKey", $"{user.Id}", _cookieOptionsForAuthentication);

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(1);
            Response.Cookies.Append("AuthenticationKey", $"{User.UserId}", options);

            return RedirectToAction("Login", "Login", new { userId = User.UserId });
        }

        [HttpGet]
        public IActionResult Login()
        {
            //if (String.IsNullOrEmpty(model.UserId))
            //{
            //    model.Message = "Kullanıcı bulunamadı.";
            //    return View(model);
            //}

            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInViewModel model)
        {
            if (String.IsNullOrEmpty(model.MobilePhone) || String.IsNullOrEmpty(model.Password))
            {
                model.Message = "Kullanıcı bulunamadı.";
                return View(model);
            }

            var User = _context.Users.FirstOrDefault(x => x.IsEnabled && x.PhoneNumber == model.MobilePhone && x.PasswordHash == model.Password);

            if (User == null)
            {
                model.Message = "Uygulama bulunamadı.";
                return View(model);
            }

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("AuthenticationKey", $"{User.UserId}", options);

            return RedirectToAction("Index", "Home");
        }
    }
}
