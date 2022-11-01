using Microsoft.AspNetCore.Mvc;

namespace Calculate.Controllers
{
    public class LogoutController : Controller
    {
        public bool Index()
        {
            //Response.Cookies.Delete("AuthenticationKey");

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(-1);
            Response.Cookies.Append("AuthenticationKey","", options);

            return true;
        }
    }
}
