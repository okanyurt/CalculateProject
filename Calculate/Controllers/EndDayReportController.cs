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
          
            return View();
        }

        [HttpGet]
        public async Task<List<EndDayReport>> GetAllAsync()
        {
            string officeId = Request.Cookies["OfficeIdListKey"];
            var operation = await _endDayReportService.GetAllAsync(officeId);

            return operation;
        }
    }
}
