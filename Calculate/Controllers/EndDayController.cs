using Calculate.Core;
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

            return View();
        }

        [HttpGet]
        public async Task<bool> CalculateEndDayAsync(int id, bool isCheckDay)
        {
            string userId = Request.Cookies["AuthenticationKey"];                  
            bool result = await _endDayService.CalculateEndDayAsync(id, userId, isCheckDay);
            return result;
        }
    }
}
