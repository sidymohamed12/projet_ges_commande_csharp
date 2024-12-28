using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ges_commande.Data;
using ges_commande.Models;
using ges_commande.Models.Enum;
using System.Security.Claims;
using ges_commande.Services;

namespace ges_commande.Controllers
{
    public class CommandeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommandeService commandeService;

        public CommandeController(ApplicationDbContext context, ICommandeService commandeService)
        {
            _context = context;
            this.commandeService = commandeService;
        }

        // GET: Commande
        public async Task<IActionResult> Index(int? IsPayer = null, int? IsLivrer = null, int page = 1, int limit = 5)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var login = User.Identity!.Name;
            var commandes = await commandeService.GetCommandePagination(IsPayer, IsLivrer, login, userRole, page, limit);

            ViewBag.IsPayer = IsPayer;
            ViewBag.Islivrer = IsLivrer;

            int totalCommande = await commandeService.GetCountCommande(login, userRole);
            ViewBag.TotalPages = (int)Math.Ceiling(totalCommande / (double)limit);
            ViewBag.CurrentPage = page;

            return View(commandes);
        }

        // GET: Commande/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.Client.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commande == null)
            {
                return NotFound();
            }
            var typePayement = Enum.GetValues(typeof(TypePayement))
                                .Cast<TypePayement>()
                                .Select(t => (Value: t.ToString(), Text: t.ToString()))
                                .ToList();

            ViewBag.typePayement = typePayement;

            ViewBag.Produits = await commandeService.GetProduitsCommande(commande.Id);
            ViewBag.Livreurs = await _context.Livreurs.ToListAsync();

            return View(commande);
        }

        // GET: Commande/Create
        public IActionResult Create()
        {
            var produits = _context.Produits.Select(p => new { p.Id, p.Libelle }).ToList();
            ViewBag.Produit = new SelectList(produits, "Id", "Libelle");
            var panier = HttpContext.Session.GetObjectFromJson<Panier>("Panier") ?? new Panier();
            ViewData["Panier"] = panier;
            ViewBag.Total = panier.Total;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Commande commande)
        {
            var panier = HttpContext.Session.GetObjectFromJson<Panier>("Panier");
            var client = _context.Clients.Include(c => c.User).Where(c => c.User.Login == User.Identity!.Name).FirstOrDefault();

            if (panier != null && client != null)
            {
                commande.Client = client;
                commande.ClientId = client.Id;
                commande.IsLivrer = false;
                commande.IsPayer = false;
                commande.Date = DateTime.UtcNow;
                commande.Montant = panier.Total;
                commande.Details = panier.CommandeProduit;
                commande.OnPrePersist();

                foreach (var item in panier.CommandeProduit)
                {
                    item.Produit.QteSock = item.Produit.QteSock - item.QteCommande;
                    if (item.Produit.QteSock <= item.QteCommande)
                    {
                        commande.Etat = EtatCommande.ATTENTE;
                    }

                    _context.Attach(item.Produit);
                    item.Commande = commande;
                    item.OnPrePersist();
                    _context.Add(item);
                }
                _context.Add(commande);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("Panier");
                return RedirectToAction(nameof(Index));
            }
            // if (ModelState.IsValid)
            // {
            //     _context.Add(commande);
            //     await _context.SaveChangesAsync();
            //     return RedirectToAction(nameof(Index));
            // }
            var produits = _context.Produits.Select(p => new { p.Id, p.Libelle }).ToList();
            ViewBag.Produit = new SelectList(produits, "Id", "Libelle");
            return View(commande);
        }

        // GET: Commande/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", commande.ClientId);
            // ViewData["LivreurId"] = new SelectList(_context.Livreurs, "Id", "Id", commande.LivreurId);
            return View(commande);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Date,Montant,IsLivrer,IsPayer,Etat,LivreurId,ClientId,Id,CreatedAt,UpdatedAt")] Commande commande)
        {
            if (id != commande.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commande);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommandeExists(commande.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", commande.ClientId);
            // ViewData["LivreurId"] = new SelectList(_context.Livreurs, "Id", "Id", commande.LivreurId);
            return View(commande);
        }

        // GET: Commande/Delete/id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        // POST: Commande/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);
            if (commande != null)
            {
                _context.Commandes.Remove(commande);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommandeExists(int id)
        {
            return _context.Commandes.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult AddProduit(int produitId, int qteVendu)
        {
            var produit = _context.Produits.Find(produitId);

            if (produit == null)
            {
                ModelState.AddModelError("", "Selection un produit !");
                return RedirectToAction("Create");
            }

            if (qteVendu <= 0)
            {
                ModelState.AddModelError("", "Entrer une qte valide !");
                return RedirectToAction("Create");
            }

            var panier = HttpContext.Session.GetObjectFromJson<Panier>("Panier") ?? new Panier();
            panier.AddInPanier(produit, qteVendu);

            HttpContext.Session.SetObjectAsJson("Panier", panier);
            return RedirectToAction("Create");

        }

        public IActionResult DeleteProduit(int produitId)
        {
            var panier = HttpContext.Session.GetObjectFromJson<Panier>("Panier");
            if (panier != null)
            {
                panier = panier.DeleteProduit(produitId);
                HttpContext.Session.SetObjectAsJson("Panier", panier);
                ViewData["Panier"] = panier;
            }
            return RedirectToAction("Create");
        }
    }
}
