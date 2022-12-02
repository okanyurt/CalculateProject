using Calculate.Core;
using Calculate.Data.Enums;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Calculate.Controllers
{
    public class AccountDetailController : BaseController
    {
        private readonly IAccountDetailService _accountDetailService;

        public AccountDetailController(IAccountDetailService accountDetailService)
        {
            _accountDetailService = accountDetailService;
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

            var accounts = await _accountDetailService.GetAllAccountAsync();
            ViewBag.accounts = new SelectList(accounts, "Id", "Name");

            var banks = await _accountDetailService.GetAllBankAsync();
            ViewBag.banks = new SelectList(banks, "Id", "Name");

            var list = await _accountDetailService.GetAllAsync();
            return View(list);
        }

        public async Task<AccountDetailGet> GetByIdAsync(int id)
        {

            var account = await _accountDetailService.GetByIdAsync(id);
            return account;
        }

        [HttpPost]
        public async Task<JsonResult> AccountDetailCreate([FromBody] AccountDetail AccountDetailCreate)
        {
            try
            {
                bool checkError = false;
                if (AccountDetailCreate.Id == null)
                {
                    Error("Hesap boş gönderilemez");
                    checkError = true;
                }

                if (AccountDetailCreate.BankId == null)
                {
                    Error("Banka boş gönderilemez");
                    checkError = true;
                }

                if (AccountDetailCreate.IbanNumber == null)
                {
                    Error("Iban boş gönderilemez");
                    checkError = true;
                }

                if (AccountDetailCreate.BankAccountNumber == null)
                {
                    Error("Hesap numarası boş gönderilemez");
                    checkError = true;
                }

                if (checkError)
                {
                    return Json(new { redirectToUrl = Url.Action("Index", "AccountDetail"), isSuccess = false });
                }

                string userId = Request.Cookies["AuthenticationKey"];
                await _accountDetailService.AddAsync(AccountDetailCreate, userId);
                Success("İşlem başarılı.");
            }
            catch (Exception ex)
            {
                Info(ex.ToString());
                return Json(new { redirectToUrl = Url.Action("Index", "AccountDetail"), isSuccess = false });
            }

            return Json(new { redirectToUrl = Url.Action("Index", "AccountDetail"), isSuccess = true });
        }

        [HttpPost]
        public async Task<JsonResult> AccountDetailEdit([FromBody] AccountDetail AccountDetailUpdate)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _accountDetailService.UpdateAsync(AccountDetailUpdate, userId);
                Success("İşlem başarılı.");
                return Json(new { redirectToUrl = Url.Action("Index", "AccountDetail"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "AccountDetail"), isSuccess = false });
            }
        }

        public async Task<JsonResult> AccountDetailDelete(int id)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _accountDetailService.RemoveAsync(id, userId);
                return Json(new { redirectToUrl = Url.Action("Index", "AccountDetail"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "AccountDetail"), isSuccess = false });
            }
        }
    }
}
