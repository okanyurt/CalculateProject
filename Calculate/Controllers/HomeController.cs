using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Calculate.Data;
using Calculate.Data.Models;

namespace Calculate.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _context;

    public HomeController(ILogger<HomeController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        if(Request.Cookies["AuthenticationKey"] == null)
        {
            return RedirectToAction("Index", "Login");
        }

        return View(_context.Operations.Where(x => x.IsEnable == true).ToList());
    }

    [HttpGet]
    public IActionResult OperationCreate()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult OperationCreate(Operation o)
    {
        try
        {
            var date = DateTime.UtcNow;
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
            if(ope != null) {
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
    public IActionResult OperationEdit(int id)
    {
        var ope = _context.Operations.Find(id);

        return View("OperationCreate",ope); 
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult OperationEdit(Operation o)
    {
        try
        {
            var ope = _context.Operations.Find(o.Id);
            if(ope != null)
            {
                var date = DateTime.UtcNow;
                ope.ProcessNumber = o.ProcessNumber;
                ope.AccountId = o.AccountId;
                ope.AccountDetailId = o.AccountDetailId;
                ope.ProcessTypeId = o.ProcessTypeId;
                ope.Price = o.Price;
                ope.ProcessPrice = o.ProcessPrice;
                ope.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == Request.Cookies["AuthenticationKey"]).Id;
                ope.UpdatedDate = date;
                ope.IsEnable = true;
                _context.SaveChanges();
            }
          
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }
}

