using Calculate.Core;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Calculate.Controllers
{
    public class EndDayReportController : BaseController
    {
        private readonly IEndDayReportService _endDayReportService;

        public EndDayReportController(IEndDayReportService endDayReportService)
        {
            _endDayReportService = endDayReportService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.reportmaxdate = _endDayReportService.GetMaxDate();

            return View();
        }

        [HttpGet]
        public async Task<List<EndDayReport>> GetAllAsync()
        {
            string officeId = Request.Cookies["OfficeIdListKey"];
            var operation = await _endDayReportService.GetAllAsync(officeId);

            return operation;
        }

        [HttpGet]
        public async Task<List<EndDayReport>> GetAllSelectDateAsync(string _date)
        {
            string _officeId = Request.Cookies["OfficeIdListKey"];
            var list = await _endDayReportService.GetAllSelectDateAsync(_officeId, _date);
            return list;
        }
    }
}
