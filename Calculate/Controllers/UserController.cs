using Calculate.Core;
using Calculate.Data.Enums;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Calculate.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Request.Cookies["UserRoleIdKey"] != Convert.ToInt32(EnumRole.ADMIN).ToString())
            {
                return View("~/Views/Shared/DeniedAccess.cshtml");
            }

            var offices = await _userService.GetAllOfficeAsync();
            ViewBag.offices = new SelectList(offices, "Id", "Name");


            var list = await _userService.GetAllAsync();
            return View(list);
        }

        public async Task<UserGet> GetByIdAsync(int id)
        {

            var user = await _userService.GetByIdAsync(id);
            return user;
        }

        [HttpPost]
        public async Task<JsonResult> UserCreate([FromBody] User UserCreate)
        {
            try
            {
                bool checkError = false;
                if (UserCreate.UserName == null)
                {
                    Error("Kullanıcı adı boş gönderilemez");
                    checkError = true;
                }

                if (UserCreate.RoleID == 0)
                {
                    Error("Rol boş gönderilemez");
                    checkError = true;
                }

                if (UserCreate.IsEnabled != false && UserCreate.IsEnabled != true)
                {
                    Error("Aktif/Pasif boş gönderilemez");
                    checkError = true;
                }

                if (UserCreate.officeIdList == 0)
                {
                    Error("Ofis boş gönderilemez");
                    checkError = true;
                }

                if (UserCreate.PhoneNumber == null)
                {
                    Error("Telefon numarası boş gönderilemez");
                    checkError = true;
                }

                if (UserCreate.PasswordHash == null)
                {
                    Error("Şifre boş gönderilemez");
                    checkError = true;
                }

                if (checkError)
                {
                    return Json(new { redirectToUrl = Url.Action("Index", "User"), isSuccess = false });
                }

                string userId = Request.Cookies["AuthenticationKey"];
                await _userService.AddAsync(UserCreate, userId);
                Success("İşlem başarılı.");
            }
            catch (Exception ex)
            {
                Info(ex.ToString());
                return Json(new { redirectToUrl = Url.Action("Index", "User"), isSuccess = false });
            }

            return Json(new { redirectToUrl = Url.Action("Index", "User"), isSuccess = true });
        }

        [HttpPost]
        public async Task<JsonResult> UserEdit([FromBody] User UserUpdate)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _userService.UpdateAsync(UserUpdate, userId);
                Success("İşlem başarılı.");
                return Json(new { redirectToUrl = Url.Action("Index", "User"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "User"), isSuccess = false });
            }
        }

        public async Task<JsonResult> UserDelete(int id)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _userService.RemoveAsync(id, userId);
                return Json(new { redirectToUrl = Url.Action("Index", "User"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "User"), isSuccess = false });
            }
        }
    }
}
