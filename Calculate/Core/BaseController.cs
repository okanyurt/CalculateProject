using Calculate.Data.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Calculate.Core
{
    //[Authorize]
    public abstract class BaseController : Controller
    {
        private UserManager<User> _userManager;
        protected void Success(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        protected void Info(string message)
        {
            TempData["InfoMessage"] = message;
        }

        protected void Warning(string message)
        {
            TempData["WarningMessage"] = message;
        }

        protected void Error(string message)
        {
            TempData["ErrorMessage"] = message;
        }

        private (User user, bool initialized) _userData;
       
        protected User CurrentUser
        {
            get
            {
                if (!_userData.initialized)
                {
                    _userData.user = _userManager.Users.FirstOrDefault(x => x.Id == CurrentUserId);
                }

                return _userData.user;
            }
        }

        protected virtual int CurrentUserId => int.Parse(User.Identities.FirstOrDefault(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "ID"))?.FindFirst("ID")?.Value ?? "-1");

        public static class CommonStrings
        {
            public const string AuthCookieName = "calculate-v1.0";
        }
    }
}
