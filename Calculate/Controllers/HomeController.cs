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

        return View(_context.Operations.ToList());
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
                _context.Operations.Remove(ope);
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
                ope.IslemNo = o.IslemNo;
                ope.HesapAdi = o.HesapAdi;
                ope.BankaAdi = o.BankaAdi;
                ope.IslemTipi = o.IslemTipi;
                ope.Miktar = o.Miktar;
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

