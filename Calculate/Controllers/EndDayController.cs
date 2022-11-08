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
            var cases = await _endDayService.GetCaseAsync(officeId);
            return cases;
        }

        [HttpGet]
        public async Task<bool> GetAllAsync(int id)
        {
            string userId = Request.Cookies["AuthenticationKey"];
            var list = await _endDayService.GetAllAsync(id);
            await _endDayService.AddOperationArsivAsync(list, userId);
            int result = await _endDayService.AddOperationAsync(id,userId);
            return result > 0 ? true : false;
        }
    }
}
