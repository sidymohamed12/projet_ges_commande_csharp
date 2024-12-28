using ges_commande.Models;

namespace ges_commande.Services
{
    public interface IPayementService
    {
        Task<IEnumerable<Payement>> GetAllPayement();
        Task<IEnumerable<Payement>> GetAllPayementPagination(int page = 1, int limit = 4);
        Task<int> GetCountPayement();
        Task<Payement> Create(string typePayement, string RefPayement, string commandeId);
    }
}