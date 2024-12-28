using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ges_commande.Data;
using ges_commande.Models;
using Microsoft.AspNetCore.Authorization;
using ges_commande.Services;

namespace ges_commande.Controllers
{
    [Authorize(Roles = "Comptable")]
    public class ClientController : Controller
    {
        private readonly IClientService clientService;

        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }


        // GET: Client
        public async Task<IActionResult> Index(int page = 1, int limit = 10)
        {
            var clients = await clientService.GetAllClientPagination(page, limit);
            int totalClients = await clientService.GetCountClient();
            ViewBag.TotalPages = (int)Math.Ceiling(totalClients / (double)limit);
            ViewBag.CurrentPage = page; return View(clients);
        }

        // GET: Client/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var client = await _context.Clients
        //         .Include(c => c.User)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (client == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(client);
        // }

        // GET: Client/Create
        // public IActionResult Create()
        // {
        //     ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        //     return View();
        // }


        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Adresse,Solde,UserId,Id,CreatedAt,UpdatedAt")] Client client)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(client);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", client.UserId);
        //     return View(client);
        // }

        // GET: Client/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var client = await _context.Clients.FindAsync(id);
        //     if (client == null)
        //     {
        //         return NotFound();
        //     }
        //     ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", client.UserId);
        //     return View(client);
        // }


        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Adresse,Solde,UserId,Id,CreatedAt,UpdatedAt")] Client client)
        // {
        //     if (id != client.Id)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(client);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!ClientExists(client.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", client.UserId);
        //     return View(client);
        // }

    }
}
