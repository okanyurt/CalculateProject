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

            var operation = await _operationService.GetAllAsync();
       
            ViewBag.accounts = new SelectList(await GetAccountAsync(), "Id", "Name");

            ViewBag.processTypes = new SelectList(await GetProcessTypeAsync(), "Id", "Name");

            return View(operation);

        }

        [HttpPost]
        public async Task<IActionResult> Index(int ProcessNumber, int AccountId, int AccountDetailId, int ProcessTypeId, decimal Price, decimal ProcessPrice)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _operationService.AddAsync(ProcessNumber, AccountId, AccountDetailId, ProcessTypeId, Price, ProcessPrice, userId);
                Success("İşlem başarılı.");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
    }
}
