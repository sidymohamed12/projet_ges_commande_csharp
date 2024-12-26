namespace ges_commande.Models
{
    public class Livraison : AbstractEntity
    {
        public Commande Commande { get; set; }
        public int CommandeId { get; set; }
        public Livreur Livreur { get; set; }
        public int? LivreurId { get; set; }
        public DateOnly Date { get; set; }
        public string? Adresse { get; set; }

    }
}