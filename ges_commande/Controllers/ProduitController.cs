using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ges_commande.Data;
using ges_commande.Models;
using ges_commande.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace ges_commande.Controllers
{
    [Authorize(Roles = "RS, Client")]
    public class ProduitController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProduitService produitService;

        public ProduitController(ApplicationDbContext context, IProduitService produitService)
        {
            _context = context;
            this.produitService = produitService;
        }

        // GET: Produit
        public async Task<IActionResult> Index(int page = 1, int limit = 10)
        {
            var produits = await produitService.GetProduitAsync(page, limit);

            int totalProdiuts = await produitService.GetCountProduit();
            ViewBag.TotalPages = (int)Math.Ceiling(totalProdiuts / (double)limit);
            ViewBag.CurrentPage = page;

            return View(produits);
        }

        // GET: Produit/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // GET: Produit/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produit produit)
        {

            if (produit.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Veuillez télécharger une image.");
            }
            else
            {
                var imagePath = Path.Combine("wwwroot/image/produits", produit.ImageFile.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    produit.ImageFile.CopyTo(stream);
                }

                produit.Image = $"/image/produits/{produit.ImageFile.FileName}";
            }

            if (ModelState.IsValid)
            {

                _ = await produitService.Create(produit);
                return RedirectToAction(nameof(Index));
            }
            return View(produit);
        }

        // GET: Produit/Edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            return View(produit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produit produit)
        {
            if (id != produit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = await produitService.Update(produit);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.Id))
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
            return View(produit);
        }

        [HttpPost("api/addproducts")]
        public IActionResult AddProduit(int produitId)
        {
            var produit = _context.Produits.Find(produitId);

            var panier = HttpContext.Session.GetObjectFromJson<Panier>("Panier") ?? new Panier();
            panier.AddInPanier(produit, 1);

            HttpContext.Session.SetObjectAsJson("Panier", panier);
            return Json(new { success = true });
        }

        public IActionResult PanierList()
        {
            var panier = HttpContext.Session.GetObjectFromJson<Panier>("Panier") ?? new Panier();
            ViewData["Panier"] = panier;
            ViewBag.nbr = panier.CommandeProduit.Count();
            ViewBag.Total = panier.Total;
            return View();
        }


        public IActionResult DeleteProduit(int detailId)
        {
            var panier = HttpContext.Session.GetObjectFromJson<Panier>("Panier");
            if (panier != null)
            {
                panier = panier.DeleteProduit(detailId);
                HttpContext.Session.SetObjectAsJson("Panier", panier);
                ViewData["Panier"] = panier;
            }
            return Json(new { success = true });
        }

        [HttpPost("Produit/UpdateQuantity")]
        public IActionResult UpdateQuantity([FromBody] JsonElement data)
        {
            // Extraire les paramètres directement du corps JSON
            int produitId = data.GetProperty("produitId").GetInt32();
            int quantity = data.GetProperty("quantity").GetInt32();

            var panier = HttpContext.Session.GetObjectFromJson<Panier>("Panier");

            if (panier == null)
            {
                return Json(new { success = false });
            }

            var produit = panier.CommandeProduit.FirstOrDefault(p => p.Produit.Id == produitId);
            if (produit == null)
            {
                return Json(new { success = false });
            }

            produit.QteCommande = quantity;

            produit.Montant = produit.Produit.PrixUnit * produit.QteCommande;

            var total = panier.CommandeProduit.Sum(p => p.Montant);

            HttpContext.Session.SetObjectAsJson("Panier", panier);

            return Json(new { success = true, newMontant = produit.Montant, total = total });
        }


        private bool ProduitExists(int id)
        {
            return _context.Produits.Any(e => e.Id == id);
        }
    }
}

