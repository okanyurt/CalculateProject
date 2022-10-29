using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Calculate.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (Request.Cookies["AuthenticationKey"] == null)
        {
            return RedirectToAction("Index", "Login");
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}

