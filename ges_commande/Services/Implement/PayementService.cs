using ges_commande.Data;
using ges_commande.Models;
using ges_commande.Models.Enum;
using ges_commande.Services;
using Microsoft.EntityFrameworkCore;

namespace ges_commande.Services.Implement
{
    public class PayementService : IPayementService
    {
        private readonly ApplicationDbContext context;
        public PayementService(ApplicationDbContext context)
        {
            this.context = context;
        }


        public async Task<IEnumerable<Payement>> GetAllPayement()
        {
            return await context.Payements.Include(p => p.Commande).ToArrayAsync();
        }

        public async Task<IEnumerable<Payement>> GetAllPayementPagination(int page = 1, int limit = 4)
        {
            var result = context.Payements.Skip((page - 1) * limit).Take(limit);
            return await result.OrderBy(p => p.Id).Include(p => p.Commande).ToListAsync();

        }


        public async Task<Payement> Create(string typePayement, string RefPayement, string commandeId)
        {
            var commande = await context.Commandes.Include(c => c.Client).FirstOrDefaultAsync(c => c.Id == int.Parse(commandeId));

            try
            {
                var payement = new Payement();
                payement.HasReduction = false;
                payement.CommandeId = commande!.Id;
                payement.Montant = commande.Montant;
                payement.Type = (TypePayement)int.Parse(typePayement);
                payement.OnPrePersist();
                context.Add(payement);
                commande.IsPayer = true;
                await context.SaveChangesAsync();
                return payement;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(string.Empty, $"Une erreur est survenue : {ex.Message}");
                return null;
            }
        }

        public async Task<int> GetCountPayement()
        {
            return await context.Payements.CountAsync();
        }
    }
}