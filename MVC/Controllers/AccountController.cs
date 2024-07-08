using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Context;

namespace MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly WebDatabaseContext _context;

        public AccountController(WebDatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Usuarios
                    .SingleOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Nombre),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    HttpContext.Session.SetInt32("UserId", user.Id);

                    return RedirectToAction("Index", "Tableros");
                }

                ModelState.AddModelError(string.Empty, "Usuario inexistente o contraseña incorrecta.");
            }

            return View(model);
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _context.Usuarios.AnyAsync(u => u.Email == model.Email);
                if (userExists)
                {
                    ModelState.AddModelError(string.Empty, "Email ya registrado.");
                    return View(model);
                }

                var user = new Usuario
                {
                    Email = model.Email,
                    Password = model.Password,
                    Nombre = model.Nombre
                };

                _context.Usuarios.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Remove("UserId");

            return RedirectToAction("Index", "Home");
        }
    }
}
