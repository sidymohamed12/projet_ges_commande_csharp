using ges_commande.Models;
using Microsoft.EntityFrameworkCore;

namespace ges_commande.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Livreur> Livreurs { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Payement> Payements { get; set; }
        public DbSet<Livraison> Livraisons { get; set; }

    }
}