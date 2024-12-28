using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ges_commande.Data;
using ges_commande.Models;
using ges_commande.Models.Enum;
using ges_commande.Services;

namespace ges_commande.Controllers
{
    public class PayementController : Controller
    {
        private readonly IPayementService payementService;

        public PayementController(IPayementService payementService)
        {
            this.payementService = payementService;
        }

        public async Task<IActionResult> Index(int page = 1, int limit = 10)
        {
            var payements = await payementService.GetAllPayementPagination(page, limit);
            int totalPayements = await payementService.GetCountPayement();
            ViewBag.TotalPages = (int)Math.Ceiling(totalPayements / (double)limit);
            ViewBag.CurrentPage = page;
            return View(payements);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string typePayement, string RefPayement, string commandeId)
        {
            if (string.IsNullOrEmpty(typePayement) || string.IsNullOrEmpty(RefPayement))
            {
                return View();
            }

            try
            {
                _ = await payementService.Create(typePayement, RefPayement, commandeId);
                return RedirectToAction("Index", "Commande");
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Empty, $"Une erreur est survenue : {ex.Message}");
                return View();
            }


        }

    }
}
