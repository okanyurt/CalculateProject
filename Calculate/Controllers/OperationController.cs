using Calculate.Core;
using Calculate.Data.Enums;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System.Text.Json;
using System.Text.Json.Nodes;

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
            string officeId = Request.Cookies["OfficeIdListKey"];
            var operation = await _operationService.GetAllAsync(officeId, Request.Cookies["UserRoleIdKey"] == Convert.ToInt32(EnumRole.ADMIN).ToString() ? true : false);

            var processType = await GetProcessTypeAsync();
            ViewBag.processTypes = new SelectList(processType, "Id", "Name");

            var casess = await GetCaseAsync();
            ViewBag.cases = new SelectList(casess, "Id", "Name");

            ViewBag.maxdate = _operationService.GetMaxDate();

            return View(operation);

        }

        [HttpPost]
        public async Task<JsonResult> OperationCreate([FromBody] OperationCreate OperationCreate)
        {
            try
            {
                bool checkError = false;
               
                if (OperationCreate == null)
                {
                    Error("Bir hata oluştu. Ekranı yenileyerek tekrar deneyiniz.");
                    checkError = true;
                }
                else if (OperationCreate.CaseId == null || OperationCreate.CaseId == 0)
                {
                    Error("Kasa adı boş gönderilemez");
                    checkError = true;
                }
                else if (OperationCreate.AccountId == null || OperationCreate.AccountId == 0)
                {
                    Error("Hesap adı boş gönderilemez");
                    checkError = true;
                }
                else if (OperationCreate.AccountDetailId == null || OperationCreate.AccountDetailId == 0)
                {
                    Error("Banka adı boş gönderilemez");
                    checkError = true;
                }
                else if (OperationCreate.ProcessTypeId == null || OperationCreate.ProcessTypeId == 0)
                {
                    Error("İşlem tipi boş gönderilemez");
                    checkError = true;
                }

                if (checkError)
                {
                    return Json(new { redirectToUrl = Url.Action("Index", "Operation"), isSuccess = false });
                }
                OperationCreate.ProcessNumber = OperationCreate.ProcessNumber == null ? 0 : OperationCreate.ProcessNumber;
                string userId = Request.Cookies["AuthenticationKey"];
                await _operationService.AddAsync(OperationCreate, userId);
                Success("İşlem başarılı.");               
            }
            catch (Exception ex)
            {
                Info(ex.ToString());
                return Json(new { redirectToUrl = Url.Action("Index", "Operation"), isSuccess = false });
            }

            return Json(new { redirectToUrl = Url.Action("Index", "Operation"), isSuccess = true });
        }

        public async Task<JsonResult> OperationDelete(int id)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                await _operationService.RemoveAsync(id, userId);
                return Json(new { redirectToUrl = Url.Action("Index", "Operation"), isSuccess = true });
            }
            catch
            {
                return Json(new { redirectToUrl = Url.Action("Index", "Operation"), isSuccess = false });
            }
        }

        [HttpGet]
        public async Task<Operation> OperationEdit(int id)
        {
            var ope = await _operationService.GetByIdAsync(id);

            return ope;
        }

        [HttpPost]
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

        [HttpGet]
        public async Task<List<AccountGetName>> GetAccountAsync(int id)
        {
            var accounts = await _operationService.GetAccountAsync(id);
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
            string roleId = Request.Cookies["UserRoleIdKey"];
            var cases = await _operationService.GetCaseAsync(officeId, roleId);
            return cases;
        }

        [HttpPost]
        public async Task<JsonResult> uploadData([FromBody] List<OperationUploadExcel> list)
        {
            try
            {
                var _list = new List<OperationUploadExcel>();

                var userId = Request.Cookies["AuthenticationKey"];
                bool result = false;
                if (list == null)
                    result = true;
                else
                {
                   
                    for (int row = 0; row < list.Count; row++)
                    {                      
                        _list.Add(new OperationUploadExcel
                        {
                            CaseName = list[row].CaseName.ToString().Trim().ToUpper(),
                            ProcessNumber = list[row].ProcessNumber.ToString().Trim(),
                            Account = list[row].Account.ToString().Trim().ToUpper(),
                            BankName = list[row].BankName.ToString().Trim().ToUpper(),
                            ProcessType = list[row].ProcessType.ToString().Trim().ToUpper(),
                            Price = list[row].Price.ToString().Trim().Replace("₺", "").Replace(".",","),
                            ProcessPrice = list[row].ProcessPrice.ToString().Trim().Replace("₺", "").Replace(".", ",")
                        });
                    }
                    if (_list != null)
                    {
                        result = await _operationService.SaveUploadExcelAsync(_list, userId);
                    }                 
                }
                  

                return Json(new
                {
                    Success = result,
                    Message = result ? "İşlem başarılı" : "Dosyayı kontrol ediniz."
                });
            }
            catch
            {
                return Json(new
                {
                    Success = false,
                    Message = "Dosyayı kontrol ediniz."
                });

            }
        }

        [HttpGet]
        public async Task<List<OperationGet>> GetAllSelectDateAsync(string _date)
        {
            string _officeId = Request.Cookies["OfficeIdListKey"];
            var list = await _operationService.GetAllSelectDateAsync(_officeId, _date, Request.Cookies["UserRoleIdKey"] == Convert.ToInt32(EnumRole.ADMIN).ToString() ? true : false);
            return list;
        }
    }
}
