using Calculate.Core;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

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
            var operation = await _operationService.GetAllAsync(officeId);

            ViewBag.processTypes = new SelectList(await GetProcessTypeAsync(), "Id", "Name");

            ViewBag.cases = new SelectList(await GetCaseAsync(), "Id", "Name");

            return View(operation);

        }

        [HttpPost]
        public async Task<JsonResult> OperationCreate([FromBody] OperationCreate OperationCreate)
        {
            try
            {
                bool checkError = false;

                if (OperationCreate.CaseId == null || OperationCreate.CaseId == 0)
                {
                    Error("Kasa adı boş gönderilemez");
                    checkError = true;
                }
                else if (OperationCreate.ProcessNumber == null || OperationCreate.ProcessNumber == 0)
                {
                    Error("İşlem Numarası boş gönderilemez");
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
            var cases = await _operationService.GetCaseAsync(officeId);
            return cases;
        }

        [HttpPost]
        public async Task<JsonResult> uploadData(IFormFile excelFile)
        {
            try
            {
                var list = new List<OperationUploadExcel>();
                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowcount = worksheet.Dimension.Rows;
                        for (int row = 3; row < rowcount; row++)
                        {
                            list.Add(new OperationUploadExcel
                            {
                                CaseName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                ProcessNumber = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                Account = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                BankName = worksheet.Cells[row, 4].Value.ToString().Trim(),
                                ProcessType = worksheet.Cells[row, 5].Value.ToString().Trim(),
                                Price = worksheet.Cells[row, 6].Value.ToString().Trim(),
                                ProcessPrice = worksheet.Cells[row, 7].Value.ToString().Trim(),
                            });
                        }
                    }
                }

                var userId = Request.Cookies["AuthenticationKey"];
                bool result = false;
                if (list == null)
                    result = true;
                else
                    result = await _operationService.SaveUploadExcelAsync(list, userId);

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
    }
}
