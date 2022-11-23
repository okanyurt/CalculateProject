using Calculate.Core;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Calculate.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Request.Cookies["UserRoleIdKey"] != "2")
            {
                return View("~/Views/Shared/DeniedAccess.cshtml");
            }

            var cases = await _accountService.GetAllCaseAsync();
            ViewBag.cases = new SelectList(cases, "Id", "Name");

            var banks = await _accountService.GetAllBankAsync();
            ViewBag.banks = new SelectList(banks, "Id", "Name");

            var list = await _accountService.GetAllAsync();
            return View(list);
        }

        public async Task<AccountGet> GetByIdAsync(int id)
        {

            var account = await _accountService.GetByIdAsync(id);
            return account;
        }

        [HttpPost]
        public async Task<JsonResult> AccountCreate([FromBody] AccountGet AccountCreate)
        {
            try
            {
                bool checkError = false;
                if (AccountCreate.Name == null)
                {
                    Error("Hesap adı boş gönderilemez");
                    checkError = true;
                }

                if (AccountCreate.PhoneNumber == null)
                {
                    Error("Telefon numarası boş gönderilemez");
                    checkError = true;
                }

                if (AccountCreate.CaseId == null)
                {
                    Error("Kasa boş gönderilemez");
                    checkError = true;
                }

                if (AccountCreate.BankId == null)
                {
                    Error("Banka boş gönderilemez");
                    checkError = true;
                }

                if (AccountCreate.IbanNumber == null)
                {
                    Error("Iban boş gönderilemez");
                    checkError = true;
                }

                if (AccountCreate.BankAccountNumber == null)
                {
                    Error("Hesap numarası boş gönderilemez");
                    checkError = true;
                }

                if (checkError)
                {
                    return Json(new { redirectToUrl = Url.Action("Index", "Case"), isSuccess = false });
                }

                string userId = Request.Cookies["AuthenticationKey"];
                await _accountService.AddAsync(AccountCreate, userId);
                Success("İşlem başarılı.");
            }
            catch (Exception ex)
            {
                Info(ex.ToString());
                return Json(new { redirectToUrl = Url.Action("Index", "Account"), isSuccess = false });
            }

            return Json(new { redirectToUrl = Url.Action("Index", "Account"), isSuccess = true });
        }

        [HttpPost]
        public async Task<JsonResult> AccountEdit([FromBody] AccountGet AccountUpdate)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _accountService.UpdateAsync(AccountUpdate, userId);
                Success("İşlem başarılı.");
                return Json(new { redirectToUrl = Url.Action("Index", "Account"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "Account"), isSuccess = false });
            }
        }

        public async Task<JsonResult> AccountDelete(int id)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _accountService.RemoveAsync(id, userId);
                return Json(new { redirectToUrl = Url.Action("Index", "Account"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "Account"), isSuccess = false });
            }
        }
    }
}
