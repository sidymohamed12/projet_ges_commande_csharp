using ges_commande.Data;
using ges_commande.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ges_commande.Services.Implement
{
    public class CommandeService : ICommandeService
    {
        private readonly ApplicationDbContext context;

        public CommandeService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<IEnumerable<Commande>> GetCommande(int? IsPayer = null, int? isLivrer = null, string? login = null, string? role = null)
        {
            var commandes = context.Commandes
                    .Include(c => c.Client)
                    .Include(c => c.Client.User)
                    .AsQueryable();

            if (role == "Client")
            {
                commandes = FilterClientConnecter(commandes, login);
            }

            commandes = FilterByLivrerStatus(commandes, isLivrer);
            commandes = FilterByPaymentStatus(commandes, IsPayer);

            return Task.FromResult((IEnumerable<Commande>)commandes);
        }

        public IQueryable<Commande> FilterClientConnecter(IQueryable<Commande> commandes, string? login)
        {
            if (!string.IsNullOrEmpty(login))
            {
                commandes = commandes.Where(c => c.Client.User.Login == login);
            }
            return commandes;
        }

        public IQueryable<Commande> FilterBySearch(IQueryable<Commande> commandes, string? search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                commandes = commandes.Where(c => c.Client.User.Nom.Contains(search.Trim()));
            }
            return commandes;
        }

        public IQueryable<Commande> FilterByPaymentStatus(IQueryable<Commande> commandes, int? isPayer)
        {
            if (isPayer.HasValue)
            {
                commandes = commandes.Where(c => c.IsPayer == (isPayer.Value == 1));
            }
            return commandes;
        }

        public IQueryable<Commande> FilterByLivrerStatus(IQueryable<Commande> commandes, int? isLivrer)
        {
            if (isLivrer.HasValue)
            {
                commandes = commandes.Where(c => c.IsLivrer == (isLivrer.Value == 1));
            }
            return commandes;
        }

        public async Task<IEnumerable<Detail>> GetProduitsCommande(int? id)
        {
            return await context.Details.Where(d => d.CommandeId == id).Include(d => d.Produit).ToListAsync();
        }

        public async Task<IEnumerable<Commande>> GetCommandePagination(int? IsPayer = null, int? isLivrer = null, string? login = null, string? role = null, int page = 1, int limit = 4)
        {
            var commandes = context.Commandes
                    .Include(c => c.Client)
                    .Include(c => c.Client.User)
                    .AsQueryable();

            if (role == "Client")
            {
                commandes = FilterClientConnecter(commandes, login);
            }

            commandes = FilterByLivrerStatus(commandes, isLivrer);
            commandes = FilterByPaymentStatus(commandes, IsPayer);

            var result = commandes.Skip((page - 1) * limit).Take(limit);
            return await result.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<int> GetCountCommande(string? login = null, string? role = null)
        {
            var commandes = context.Commandes
                    .AsQueryable();

            if (role == "Client")
            {
                commandes = FilterClientConnecter(commandes, login);
            }
            return await commandes.CountAsync();
        }
    }
}