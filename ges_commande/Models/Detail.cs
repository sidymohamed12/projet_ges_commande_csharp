namespace ges_commande.Models
{
    public class Detail : AbstractEntity
    {
        public int QteCommande { get; set; }
        public double Montant { get; set; }
        public Commande Commande { get; set; }
        public int CommandeId { get; set; }
        public Produit Produit { get; set; }
        public int ProduitId { get; set; }
    }
}