using Calculate.Core;
using Calculate.Data.Enums;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Calculate.Controllers
{
    public class OfficeController : BaseController
    {
        private readonly IOfficeService _officeService;
        private readonly IGenericRemoveService _genericRemoveService;

        public OfficeController(IOfficeService officeService, IGenericRemoveService genericRemoveService)
        {
            _officeService = officeService;
            _genericRemoveService = genericRemoveService;
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

            var offices = await GetOfficeAsync();
            ViewBag.offices = new SelectList(offices, "Id", "Name");

            return View();
        }

        public async Task<List<Office>> GetOfficeAsync()
        {
            
            var offices = await _officeService.GetAllAsync();
            return offices;
        }

        public async Task<Office> GetByIdAsync(int id)
        {

            var offices = await _officeService.GetByIdAsync(id);
            return offices;
        }

        [HttpPost]
        public async Task<JsonResult> OfficeCreate([FromBody] Office OfficeCreate)
        {
            try
            {
                bool checkError = false;

                if (OfficeCreate.Name == null)
                {
                    Error("Ofis adı boş gönderilemez");
                    checkError = true;
                }

                if (checkError)
                {
                    return Json(new { redirectToUrl = Url.Action("Index", "Office"), isSuccess = false });
                }

                string userId = Request.Cookies["AuthenticationKey"];
                await _officeService.AddAsync(OfficeCreate, userId);
                Success("İşlem başarılı.");
            }
            catch (Exception ex)
            {
                Info(ex.ToString());
                return Json(new { redirectToUrl = Url.Action("Index", "Office"), isSuccess = false });
            }

            return Json(new { redirectToUrl = Url.Action("Index", "Office"), isSuccess = true });
        }

        [HttpPost]
        public async Task<JsonResult> OfficeEdit([FromBody] Office OfficeUpdate)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _officeService.UpdateAsync(OfficeUpdate, userId);
                Success("İşlem başarılı.");
                return Json(new { redirectToUrl = Url.Action("Index", "Office"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "Office"), isSuccess = false });
            }
        }

        public async Task<JsonResult> OfficeDelete(int id)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                //await _officeService.RemoveAsync(id, userId);
                int result = await _genericRemoveService.RemoveAsync(id,(int)EnumIsMaster.OFFICE,userId);
                if (result == 0)
                {
                    return Json(new { redirectToUrl = Url.Action("Index", "Office"), isSuccess = false });
                }

                return Json(new { redirectToUrl = Url.Action("Index", "Office"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "Office"), isSuccess = false });
            }
        }
    }
}
