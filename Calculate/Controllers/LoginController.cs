using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Calculate.Data;
using Microsoft.AspNetCore.Authorization;
using Calculate.Models;
using Calculate.Core;

namespace Calculate.Controllers
{
    public class LoginController : Controller
    {
       // private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            //_context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> Index(TokenViewModel model)
        //{
        //    if (String.IsNullOrEmpty(model.Token))
        //    {
        //        Error("Kullanıcı bulunamadı.");

        //        return View(model);
        //    }

        //    //var User = _context.users.FirstOrDefault(x => x.token == user.token);

        //    //if (User != null)
        //    //{
        //    //    return RedirectToAction("Login", "Login", new { userId = User.userId });
        //    //}

        //    return View();
        //}
    }
}
