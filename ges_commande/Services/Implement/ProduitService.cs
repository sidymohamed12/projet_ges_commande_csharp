using ges_commande.Data;
using ges_commande.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ges_commande.Services.Implement
{
    public class ProduitService : IProduitService
    {
        private readonly ApplicationDbContext context;
        public ProduitService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Produit>> GetAllProduit()
        {
            return await context.Produits.ToArrayAsync();
        }

        public async Task<IEnumerable<Produit>> GetProduitAsync(int page = 1, int limit = 4)
        {
            var result = context.Produits.Skip((page - 1) * limit).Take(limit);
            return await result.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<Produit> Create(Produit produit)
        {
            context.Produits.Add(produit);
            await context.SaveChangesAsync();
            return produit;
        }

        public async Task<Produit?> FindProduitById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            try
            {
                return await context.Produits.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(" error occurred while fetching the product.", ex);
            }
        }


        public async Task<Produit> Update(Produit produit)
        {
            produit.OnPreUpdated();
            context.Update(produit);
            await context.SaveChangesAsync();
            return produit;
        }

        public async Task<int> GetCountProduit()
        {
            return await context.Produits.CountAsync();
        }
    }
}
