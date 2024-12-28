using ges_commande.Models;

namespace ges_commande.Services
{
    public interface ICommandeService
    {
        Task<IEnumerable<Commande>> GetCommande(int? IsPayer = null, int? isLivrer = null, string? login = null, string? role = null);
        Task<IEnumerable<Commande>> GetCommandePagination(int? IsPayer = null, int? isLivrer = null, string? login = null, string? role = null, int page = 1, int limit = 4);
        Task<int> GetCountCommande(string? login = null, string? role = null);
        IQueryable<Commande> FilterByPaymentStatus(IQueryable<Commande> commandes, int? isPayer);
        IQueryable<Commande> FilterByLivrerStatus(IQueryable<Commande> commandes, int? isLivrer);
        IQueryable<Commande> FilterBySearch(IQueryable<Commande> commandes, string? search);
        IQueryable<Commande> FilterClientConnecter(IQueryable<Commande> commandes, string? login);
        Task<IEnumerable<Detail>> GetProduitsCommande(int? id);
    }
}