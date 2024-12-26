using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ges_commande.Data;
using ges_commande.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ges_commande.Controllers
{
    public class SecuriteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SecuriteController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            // Recherchez l'utilisateur dans la base de données
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == user.Login);

            if (dbUser != null && dbUser.Password == user.Password) // Remplacez par une vérification de hash si nécessaire
            {
                // Créez les revendications pour l'utilisateur connecté
                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, dbUser.Login),
                new Claim(ClaimTypes.Name, dbUser.Login),
                new Claim(ClaimTypes.Role, dbUser.Role.ToString()),
                new Claim("Prenom", dbUser.Prenom),
                new Claim("Nom", dbUser.Nom)
            };

                // Identité et schéma d'authentification
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Propriétés d'authentification
                AuthenticationProperties properties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true // Permet une session persistante
                };

                // Authentification de l'utilisateur
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }
            ViewData["ValidateMessage"] = "user not found";
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Securite");
        }

        public IActionResult Error404()
        {
            return View();
        }

    }
}
