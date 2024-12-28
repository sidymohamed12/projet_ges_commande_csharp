using ges_commande.Data;
using ges_commande.Models;
using Microsoft.EntityFrameworkCore;

namespace ges_commande.Services.Implement
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext context;

        public ClientService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Client>> GetAllClient()
        {
            return await context.Clients.Include(c => c.User).ToArrayAsync();
        }

        public async Task<IEnumerable<Client>> GetAllClientPagination(int page = 1, int limit = 4)
        {
            var result = context.Clients.Skip((page - 1) * limit).Take(limit);
            return await result.OrderBy(p => p.Id).Include(p => p.User).ToListAsync();
        }

        public async Task<int> GetCountClient()
        {
            return await context.Clients.CountAsync();
        }
    }
}