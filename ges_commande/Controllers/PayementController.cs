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


        // POST: Payement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
