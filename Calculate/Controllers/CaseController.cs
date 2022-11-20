using Calculate.Core;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Calculate.Controllers
{
    public class CaseController : BaseController
    {
        private readonly ICaseService _caseService;

        public CaseController(ICaseService caseService)
        {
            _caseService = caseService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var offices = await _caseService.GetAllOfficeAsync();
            ViewBag.offices = new SelectList(offices, "Id", "Name");

            var cases = await GetCaseAsync();
            return View(cases);
        }

        public async Task<List<Case>> GetCaseAsync()
        {

            var cases = await _caseService.GetAllAsync();
            return cases;
        }

        public async Task<Case> GetByIdAsync(int id)
        {

            var cases = await _caseService.GetByIdAsync(id);
            return cases;
        }

        [HttpPost]
        public async Task<JsonResult> CaseCreate([FromBody] Case CaseCreate)
        {
            try
            {
                bool checkError = false;
                if (CaseCreate.officeId == null)
                {
                    Error("Ofis adı boş gönderilemez");
                    checkError = true;
                }

                if (CaseCreate.Name == null)
                {
                    Error("Kasa adı boş gönderilemez");
                    checkError = true;
                }

                if (checkError)
                {
                    return Json(new { redirectToUrl = Url.Action("Index", "Case"), isSuccess = false });
                }

                string userId = Request.Cookies["AuthenticationKey"];
                await _caseService.AddAsync(CaseCreate, userId);
                Success("İşlem başarılı.");
            }
            catch (Exception ex)
            {
                Info(ex.ToString());
                return Json(new { redirectToUrl = Url.Action("Index", "Case"), isSuccess = false });
            }

            return Json(new { redirectToUrl = Url.Action("Index", "Case"), isSuccess = true });
        }

        [HttpPost]
        public async Task<JsonResult> CaseEdit([FromBody] Case CaseUpdate)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _caseService.UpdateAsync(CaseUpdate, userId);
                Success("İşlem başarılı.");
                return Json(new { redirectToUrl = Url.Action("Index", "Case"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "Case"), isSuccess = false });
            }
        }

        public async Task<JsonResult> CaseDelete(int id)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _caseService.RemoveAsync(id, userId);
                return Json(new { redirectToUrl = Url.Action("Index", "Case"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "Case"), isSuccess = false });
            }
        }
    }
}
