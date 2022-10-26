using Calculate.Data;
using Calculate.Data.Models;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Controllers
{
    public class OperationController : Controller
    {

        private readonly IOperationService _operationService;

        public OperationController(IOperationService operationService)
        {
            _operationService = operationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var operation = _operationService.GetAll();
            return View(operation);

        }

        [HttpPost]
        public IActionResult Index(int ProcessNumber, int AccountId, int AccountDetailId, int ProcessTypeId, decimal Price, decimal ProcessPrice)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                _operationService.Add(ProcessNumber, AccountId, AccountDetailId, ProcessTypeId, Price, ProcessPrice, userId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult OperationDelete(int id)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                _operationService.Remove(id, userId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public Task<Operation> OperationEdit(int id)
        {
            var ope =  _operationService.GetByIdAsync(id);

            return ope;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult OperationEdit([FromBody] OperationUpdate OperationUpdate)
        {
            try
            {
                string userId = Request.Cookies["AuthenticationKey"];
                _operationService.Update(OperationUpdate, userId);

                return RedirectToAction(nameof(Index));                        
            }
            catch
            {
                return View();
            }
        }
    }
}
