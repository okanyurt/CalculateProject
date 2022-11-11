using Calculate.Core;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;

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

            var caseList = await GetCaseAsync();
            object[] cases = new object[caseList.Count];
            int index = 0;
            foreach (var item in caseList)
            {
                var total = await _reportService.GetCaseTotalAsync(item.Id);
                string[] c = { item.Id.ToString(), item.Name, total != null ? total.Price.ToString(): "0" };
                cases[index] = c;
                index++;
            }
            ViewBag.cases = cases;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Case()
        {
            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var caseList = await GetCaseAsync();
            object[] cases = new object[caseList.Count];
            int index = 0;
            foreach (var item in caseList)
            {
                var total = await _reportService.GetCaseTotalAsync(item.Id);
                string[] c = { item.Id.ToString(), item.Name, total != null ? total.Price.ToString() : "0" };
                cases[index] = c;
                index++;
            }

            ViewBag.cases = cases;
            return View();
        }

        public async Task<List<Case>> GetCaseAsync()
        {
            string officeId = Request.Cookies["OfficeIdListKey"];
            string userId = Request.Cookies["AuthenticationKey"];
            var cases = await _reportService.GetCaseAsync(officeId, userId);
            return cases;
        }

        /*public async Task<Operation> GetCaseTotalAsync(int id)
        {
            var casesTotal = await _reportService.GetCaseTotalAsync(id);
            return casesTotal;
        } */

        [HttpGet]
        public async Task<List<ReportGet>> GetAllAsync(int Id)
        {
            var list = await _reportService.GetAllAsync(Id);
            return list;
        }

        [HttpGet]
        public async Task<List<ReportGet>> GetAllForCaseAsync(int Id)
        {
            var list = await _reportService.GetAllForCaseAsync(Id);
            return list;
        }
    }
}
