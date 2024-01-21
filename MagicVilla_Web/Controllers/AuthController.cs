using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // GET
    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDto obj = new LoginRequestDto();
        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequestDto obj)
    {
        return View();
    }
    
    // GET
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegistrationRequestDto obj)
    {
        ApiResponse result = await _authService.RegisterAsync<ApiResponse>(obj);
        if (result != null && result.IsSuccess)
        {
            return RedirectToAction("Login");
        }
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        return View();
    }
    
    public IActionResult AccessDenied()
    {
        return View();
    }
}