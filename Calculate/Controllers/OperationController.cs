using Calculate.Data;
using Calculate.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Controllers
{
    public class OperationController : Controller
    {
        private readonly DataContext _context;

        public OperationController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Request.Cookies["AuthenticationKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var operation = _context.Operations.Where(x => x.IsEnable == true).ToList();
            return View(operation);

        }

        [HttpPost]
        public IActionResult Index(int ProcessNumber, int AccountId, int AccountDetailId, int ProcessTypeId, decimal Price, decimal ProcessPrice)
        {
            try
            {
                Operation o = new Operation();
                var date = DateTime.UtcNow;
                o.ProcessNumber = ProcessNumber;
                o.AccountId = AccountId;
                o.AccountDetailId = AccountDetailId;
                o.ProcessTypeId = ProcessTypeId;
                o.Price = Price;
                o.ProcessPrice = ProcessPrice;
                o.CreatedBy = _context.Users.FirstOrDefault(x => x.UserId == Request.Cookies["AuthenticationKey"]).Id;
                o.CreatedDate = date;
                o.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == Request.Cookies["AuthenticationKey"]).Id;
                o.UpdatedDate = date;
                o.IsEnable = true;
                _context.Operations.Add(o);
                _context.SaveChanges();
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
                var ope = _context.Operations.Find(id);
                if (ope != null)
                {
                    var date = DateTime.UtcNow;
                    ope.IsEnable = false;
                    ope.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == Request.Cookies["AuthenticationKey"]).Id;
                    ope.UpdatedDate = date;
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public Operation OperationEdit(int id)
        {
            var ope = _context.Operations.Find(id);

            return ope;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult OperationEdit([FromBody] OperationUpdate OperationUpdate)
        {
            try
            {

                var ope = _context.Operations.Find(OperationUpdate.Id);
                var date = DateTime.UtcNow;
                ope.ProcessNumber = OperationUpdate.ProcessNumber;
                ope.AccountId = OperationUpdate.AccountId;
                ope.AccountDetailId = OperationUpdate.AccountDetailId;
                ope.ProcessTypeId = OperationUpdate.ProcessTypeId;
                ope.Price = OperationUpdate.Price;
                ope.ProcessPrice = OperationUpdate.ProcessPrice;
                ope.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == Request.Cookies["AuthenticationKey"]).Id;
                ope.UpdatedDate = date;
                ope.IsEnable = true;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
