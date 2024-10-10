using BeltReview.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeltReview.Controllers;

public class UserController : Controller
{
    private readonly MyContext _context;

    public UserController(MyContext context)
    {
        _context = context;
    }


    [HttpGet("")]
    public ViewResult Index()
    {
        return View();
    }

    // [SessionCheck]
    [HttpGet("success")]
    public ViewResult Success()
    {
        return View("Success");
    }

    [HttpPost("register")]
    public IActionResult Register(User newUser)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        PasswordHasher<User> passwordHasher = new();
        newUser.Password = passwordHasher.HashPassword(newUser, newUser.Password);

        _context.Users.Add(newUser);
        _context.SaveChanges();

        HttpContext.Session.SetInt32("UserId", newUser.Id);
        HttpContext.Session.SetString("UserName", newUser.UserName);

        return RedirectToAction("Dashboard", "Sighting");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginUser newLogin)
    {
        User? dbUser = _context.Users.FirstOrDefault(u => u.Email == newLogin.LoginEmail);
        if (dbUser == null)
        {
            ModelState.AddModelError("LoginEmail", "Invalid Email");
            return View("Index");
        }
        PasswordHasher<LoginUser> passwordHasher = new();

        var result = passwordHasher.VerifyHashedPassword(newLogin, dbUser.Password, newLogin.LoginPassword);

        if (result == 0)
        {
            ModelState.AddModelError("LoginPassword", "Invalid Password");
            return View("Index");
        }

        HttpContext.Session.SetInt32("UserId", dbUser.Id);
        HttpContext.Session.SetString("UserName", dbUser.UserName);

        return RedirectToAction("Dashboard", "Sighting");
    }

    [HttpPost("logout")]
    public RedirectToActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "User");
    }
}
