using ges_commande.Models;

namespace ges_commande.Services
{
    public interface IPayementService
    {
        Task<Payement> Create(string typePayement, string RefPayement, string commandeId);
    }
}