using Calculate.Core;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Calculate.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.cases = new SelectList(await GetCaseAsync(), "Id", "Name");

            return View();
        }

        public async Task<List<Case>> GetCaseAsync()
        {
            string officeId = Request.Cookies["OfficeIdListKey"];
            var cases = await _reportService.GetCaseAsync(officeId);
            return cases;
        }

        [HttpGet]
        public async Task<List<ReportGet>> GetAllAsync(int Id)
        {
            var list = await _reportService.GetAllAsync(Id);
            return list;
        }
    }
}
