using ges_commande.Models;

namespace ges_commande.Services
{
    public interface ILivraisonService
    {
        Task<IEnumerable<Livraison>> GetAllLivraison();
        Task<IEnumerable<Livraison>> GetAllLivraisonPagination(int page = 1, int limit = 4);
        Task<int> GetCountLivraison();
        Task<Livraison> Create(string LivreurId, string DateLivraison, string AdresseLivraison, string commandeId);
    }
}