using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ges_commande.Data;
using ges_commande.Models;
using ges_commande.Services;

namespace ges_commande.Controllers
{
    public class LivraisonController : Controller
    {
        private readonly ILivraisonService livraisonService;

        public LivraisonController(ILivraisonService livraisonService)
        {
            this.livraisonService = livraisonService;
        }

        public async Task<IActionResult> Index(int page = 1, int limit = 10)
        {
            var livraisons = await livraisonService.GetAllLivraisonPagination(page, limit);
            int totalLivraisons = await livraisonService.GetCountLivraison();
            ViewBag.TotalPages = (int)Math.Ceiling(totalLivraisons / (double)limit);
            ViewBag.CurrentPage = page;

            return View(livraisons);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string LivreurId, string DateLivraison, string AdresseLivraison, string commandeId)
        {
            if (string.IsNullOrEmpty(LivreurId) || string.IsNullOrEmpty(DateLivraison) || string.IsNullOrEmpty(commandeId))
            {
                return View();
            }

            try
            {
                _ = await livraisonService.Create(LivreurId, DateLivraison, AdresseLivraison, commandeId);
                return RedirectToAction("Index", "Commande");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Une erreur est survenue : {ex.Message}");
                return View();
            }
        }
    }
}
