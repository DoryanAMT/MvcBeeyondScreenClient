using MvcBeeyondScreenClient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MvcBeeyondScreenClient.Services;
using NugetBeeyondScreen.Models;

namespace MvcBeeyondScreenClient.Controllers
{
    public class ManagedController : Controller
    {
        private ServiceCine service;
        public ManagedController(ServiceCine service)
        {
            this.service = service;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login
            (string email, string password)
        {
            Usuario usuario = await this.service.LoginUserAsync(email, password);
            if (usuario == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
            else
            {
                ClaimsIdentity identity =
                new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);

                Claim claimId =
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());
                identity.AddClaim(claimId);

                Claim claimName =
                    new Claim(ClaimTypes.Name, usuario.Nombre);
                identity.AddClaim(claimName);

                Claim claimEmail =
                    new Claim(ClaimTypes.Email, usuario.Email);
                identity.AddClaim(claimEmail);

                Claim claimImagen =
                    new Claim("Imagen", usuario.Imagen);
                identity.AddClaim(claimImagen);

                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);

                return RedirectToAction("Index", "Peliculas");
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Peliculas");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
