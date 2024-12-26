using ges_commande.Models;
using Microsoft.AspNetCore.Mvc;

namespace ges_commande.Services
{
    public interface IProduitService
    {
        Task<IEnumerable<Produit>> GetAllProduit();
        Task<int> GetCountProduit();
        Task<IEnumerable<Produit>> GetProduitAsync(int page = 1, int limit = 4);
        Task<Produit> Create(Produit produit);
        Task<Produit> Update(Produit produit);
        Task<Produit?> FindProduitById(int? id);

    }
}