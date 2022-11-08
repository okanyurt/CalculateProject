using Calculate.Data.Models;
using Calculate.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace Calculate.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly ILoginService _login;
       
        public LoginController(ILoginService login)
        {
            _login = login;
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

            var User = await _login.GetUserIndex(model.Token);
            if (User == null)
            {
                model.Message = "Uygulama bulunamadı.";
                return View(model);
            }

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(1);
            Response.Cookies.Append("AuthenticationKey", $"{User.UserId}", options);

            return RedirectToAction("Login", "Login", new { userId = User.UserId });
        }

        [HttpGet]
        public IActionResult Login(string UserId)
        {
            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (UserId == null)
            {
                TokenViewModel tokenModel = new TokenViewModel();
                tokenModel.Message = "Uygulama bulunamadı.";
                return RedirectToAction("Index", tokenModel);
            }

            SignInViewModel signInModel = new SignInViewModel();
            signInModel.UserId = UserId;
            return View(signInModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInViewModel model)
        {          
            if (String.IsNullOrEmpty(model.MobilePhone) || String.IsNullOrEmpty(model.Password))
            {
                model.Message = "Kullanıcı bulunamadı.";
                return RedirectToAction("Login", "Login", new { userId = Request.Cookies["AuthenticationKey"] });
            }

            var User = await _login.GetUserLogin(model.MobilePhone, model.Password);
            if (User == null)
            {
                model.Message = "Uygulama bulunamadı.";            
                return RedirectToAction("Login", "Login", new { userId = Request.Cookies["AuthenticationKey"] });
            }

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("AuthenticationKey", $"{User.UserId}", options);
            Response.Cookies.Append("OfficeIdListKey", $"{User.officeIdList}", options);

            return RedirectToAction("Index", "Operation");
        }
    }
}
