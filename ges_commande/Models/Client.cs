namespace ges_commande.Models
{
    public class Client : AbstractEntity
    {
        public required string Adresse { get; set; }
        public double? Solde { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public List<Commande>? Commandes { get; set; } = new List<Commande>();

    }
}