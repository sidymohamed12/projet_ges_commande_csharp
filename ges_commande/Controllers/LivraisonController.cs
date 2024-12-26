using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ges_commande.Data;
using ges_commande.Models;

namespace ges_commande.Controllers
{
    public class LivraisonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LivraisonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: Livraison/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                var commande = await _context.Commandes.Include(c => c.Client).FirstOrDefaultAsync(c => c.Id == int.Parse(commandeId));
                var livreur = await _context.Livreurs.FirstOrDefaultAsync(c => c.Id == int.Parse(LivreurId));

                var livraison = new Livraison
                {
                    LivreurId = int.Parse(LivreurId),
                    CommandeId = int.Parse(commandeId),
                    Date = DateOnly.Parse(DateLivraison),
                };

                if (string.IsNullOrEmpty(AdresseLivraison))
                {
                    livraison.Adresse = commande!.Client.Adresse;
                }
                else
                {
                    livraison.Adresse = AdresseLivraison;
                }

                livraison.OnPrePersist();
                _context.Add(livraison);
                commande!.IsLivrer = true;
                livreur!.IsDisponibile = false;
                await _context.SaveChangesAsync();

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
