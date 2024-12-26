using ges_commande.Data;
using ges_commande.Models;
using ges_commande.Models.Enum;

namespace gesDetteWebCS.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, ApplicationDbContext context)
        {

            if (context.Users.Any())
            {
                return;
            }

            // LIVREURS
            for (int i = 1; i <= 5; i++)
            {
                var livreur = new Livreur
                {
                    Nom = $"LivreurNom{i}",
                    Prenom = $"LivreurPrenom{i}",
                    Telephone = $"+221 77 987 65 4{i}",
                    IsDisponibile = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                context.Add(livreur);
            }
            context.SaveChanges();

            // Créer des produits
            var produits = new List<Produit>();
            for (int p = 1; p <= 10; p++)
            {
                var produit = new Produit
                {
                    Libelle = $"Produit{p}",
                    Image = $"/image/produits/image-{p}.jpg",
                    QteSock = 100,
                    QteSeuil = 10,
                    PrixUnit = p * 100.0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                produits.Add(produit);
                context.Produits.Add(produit);
            }

            context.SaveChanges();

            // Créer des utilisateurs et leurs clients associés avec commandes et détails
            for (int i = 1; i <= 5; i++)
            {
                var user = new User
                {
                    Nom = $"Nom{i}",
                    Prenom = $"Prenom{i}",
                    Telephone = $"+221 123 45 6{i}",
                    Login = $"user{i}@",
                    Password = $"password{i}",
                    Role = (i % 2 == 0) ? Role.Client : Role.RS,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                for (int j = 1; j <= 1; j++) // 2 clients par utilisateur
                {
                    var client = new Client
                    {
                        Adresse = $"Adresse{j}",
                        Solde = i * 1000.0,
                        User = user,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    for (int k = 1; k <= 3; k++) // 3 commandes par client
                    {
                        var commande = new Commande
                        {
                            Date = DateTime.UtcNow,
                            Etat = EtatCommande.ATTENTE,
                            IsLivrer = false,
                            IsPayer = false,
                            Montant = k * 300, // Calculé dynamiquement
                            Client = client,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        double totalCommande = 0;
                        for (int d = 1; d <= 2; d++) // 2 détails par commande
                        {
                            var produit = produits[(d + k + j) % produits.Count]; // Sélectionner un produit
                            var qteCommande = d + k;
                            var montant = qteCommande * produit.PrixUnit;
                            totalCommande += montant;

                            var detail = new Detail
                            {
                                QteCommande = qteCommande,
                                Montant = montant,
                                Commande = commande,
                                Produit = produit,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };
                            commande.Details.Add(detail);
                            context.Details.Add(detail);
                        }

                        commande.Montant = totalCommande;
                        client.Commandes.Add(commande);
                        context.Commandes.Add(commande);
                    }

                    context.Clients.Add(client);
                }

                context.Users.Add(user);
            }

            context.SaveChanges();
        }
    }
}
