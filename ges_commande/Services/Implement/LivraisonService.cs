using ges_commande.Data;
using ges_commande.Models;
using ges_commande.Services;
using Microsoft.EntityFrameworkCore;

public class LivraisonService : ILivraisonService
{
    private readonly ApplicationDbContext _context;

    public LivraisonService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Livraison>> GetAllLivraison()
    {
        return await _context.Livraisons.Include(p => p.Livreur).Include(p => p.Commande).ToListAsync();
    }

    public async Task<IEnumerable<Livraison>> GetAllLivraisonPagination(int page = 1, int limit = 4)
    {
        var result = _context.Livraisons.Skip((page - 1) * limit).Take(limit);
        return await result.OrderBy(p => p.Id).Include(p => p.Livreur).Include(p => p.Commande).ToListAsync();

    }

    public async Task<Livraison> Create(string LivreurId, string DateLivraison, string AdresseLivraison, string commandeId)
    {
        try
        {
            var commande = await _context.Commandes.Include(c => c.Client).FirstOrDefaultAsync(c => c.Id == int.Parse(commandeId));
            var livreur = await _context.Livreurs.FirstOrDefaultAsync(c => c.Id == int.Parse(LivreurId));

            var livraison = new Livraison
            {
                LivreurId = int.Parse(LivreurId),
                CommandeId = int.Parse(commandeId),
                Date = DateOnly.Parse(DateLivraison),
            };

            if (string.IsNullOrEmpty(AdresseLivraison))
            {
                livraison.Adresse = commande!.Client.Adresse;
            }
            else
            {
                livraison.Adresse = AdresseLivraison;
            }

            livraison.OnPrePersist();
            _context.Add(livraison);
            commande!.IsLivrer = true;
            livreur!.IsDisponibile = false;
            await _context.SaveChangesAsync();
            return livraison;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(string.Empty, $"Une erreur est survenue : {ex.Message}");
            return null;
        }
    }

    public async Task<int> GetCountLivraison()
    {
        return await _context.Livraisons.CountAsync();
    }
}