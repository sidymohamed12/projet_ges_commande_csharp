using ges_commande.Models;

namespace ges_commande.Services
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAllClient();
        Task<IEnumerable<Client>> GetAllClientPagination(int page = 1, int limit = 4);
        Task<int> GetCountClient();

    }
}