using Calculate.Core;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Calculate.Controllers
{
    public class EndDayController : BaseController
    {
        private readonly IEndDayService _endDayService;
        
        public EndDayController(IEndDayService endDayService)
        {
            _endDayService = endDayService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            string officeId = Request.Cookies["OfficeIdListKey"];
            var caseList = await _endDayService.GetCaseAsync(officeId);
          
            ViewBag.cases = new SelectList(caseList, "Id", "Name");

            var operation = await _endDayService.GetAllAsync(officeId);

            return View(operation);
        }

        [HttpGet]
        public async Task<bool> CalculateEndDayAsync(int id, bool isCheckDay)
        {
            string userId = Request.Cookies["AuthenticationKey"];                  
            bool result = await _endDayService.CalculateEndDayAsync(id, userId, isCheckDay);
            return result;
        }

        [HttpGet]
        public async Task<List<OperationGet>> GetAllAsync()
        {
            string officeId = Request.Cookies["OfficeIdListKey"];
            var result = await _endDayService.GetAllAsync(officeId);
            return result;
        }
    }
}
