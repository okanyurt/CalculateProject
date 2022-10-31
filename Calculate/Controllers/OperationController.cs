using Calculate.Core;
using Calculate.Data;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Controllers
{
    public class OperationController : BaseController
    {

        private readonly IOperationService _operationService;

        public OperationController(IOperationService operationService)
        {
            _operationService = operationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {          
            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var der = Request.Cookies["OfficeIdListKey"];

            var operation = await _operationService.GetAllAsync();

            ViewBag.accounts = new SelectList(await GetAccountAsync(), "Id", "Name");

            ViewBag.processTypes = new SelectList(await GetProcessTypeAsync(), "Id", "Name");

            ViewBag.cases = new SelectList(await GetCaseAsync(), "Id", "Name");

            return View(operation);

        }

        [HttpPost]
        public async Task<IActionResult> Index(int caseId, int ProcessNumber, int AccountId, int AccountDetailId, int ProcessTypeId, decimal Price, decimal ProcessPrice)
        {
            try
            {
                bool checkError = false;

                if (caseId == null || caseId == 0)
                {
                    Error("Kasa adı boş gönderilemez");
                    checkError = true;
                }
                else if (ProcessNumber == null || ProcessNumber == 0)
                {
                    Error("İşlem Numarası boş gönderilemez");
                    checkError = true;
                }
                else if (AccountId == null || AccountId == 0)
                {
                    Error("Hesap adı boş gönderilemez");
                    checkError = true;
                }
                else if (AccountDetailId == null || AccountDetailId == 0)
                {
                    Error("Banka adı boş gönderilemez");
                    checkError = true;
                }
                else if (ProcessTypeId == null || ProcessTypeId == 0)
                {
                    Error("İşlem tipi boş gönderilemez");
                    checkError = true;
                }

                if (checkError)
                {
                    return RedirectToAction(nameof(Index));
                }

                string userId = Request.Cookies["AuthenticationKey"];
                await _operationService.AddAsync(caseId, ProcessNumber, AccountId, AccountDetailId, ProcessTypeId, Price, ProcessPrice, userId);
                Success("İşlem başarılı.");
            }
            catch (Exception ex)
            {
                Info(ex.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OperationDelete(int id)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _operationService.RemoveAsync(id, userId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<Operation> OperationEdit(int id)
        {
            var ope = await _operationService.GetByIdAsync(id);

            return ope;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> OperationEdit([FromBody] OperationUpdate OperationUpdate)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _operationService.UpdateAsync(OperationUpdate, userId);
                Success("İşlem başarılı.");
                return Json(new { redirectToUrl = Url.Action("Index", "Operation"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "Operation"), isSuccess = false });
            }
        }

        public async Task<List<AccountGetName>> GetAccountAsync()
        {
            var accounts = await _operationService.GetAccountAsync();
            return accounts;
        }

        [HttpGet]
        public async Task<JsonResult> GetBankAsync(int id)
        {
            var banks = await _operationService.GetBankAsync(id);
            return Json(new SelectList(banks.ToArray(), "Id", "Name"));
        }

        public async Task<List<ProcessType>> GetProcessTypeAsync()
        {
            var processType = await _operationService.GetProcessTypeAsync();
            return processType;
        }

        public async Task<List<Case>> GetCaseAsync()
        {
            string officeId = Request.Cookies["OfficeIdListKey"];
            var cases = await _operationService.GetCaseAsync(officeId);
            return cases;
        }
    }
}
