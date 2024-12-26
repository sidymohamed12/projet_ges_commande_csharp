namespace ges_commande.Models
{
    public class Panier
    {
        public List<Detail> CommandeProduit { get; set; } = new List<Detail>();
        public double Total => CommandeProduit.Sum(a => a.Montant);

        public void AddInPanier(Produit produit, int qteVendu)
        {
            var detail = CommandeProduit.FirstOrDefault(p => p.ProduitId == produit.Id);
            if (detail == null)
            {
                CommandeProduit.Add(new Detail
                {
                    Produit = produit,
                    ProduitId = produit.Id,
                    QteCommande = qteVendu,
                    Montant = produit.PrixUnit * qteVendu,
                });
            }
            else
            {
                detail.QteCommande += qteVendu;
                detail.Montant = produit.PrixUnit * detail.QteCommande;
            }
        }

        public Panier DeleteProduit(int produitId)
        {
            var produit = CommandeProduit.FirstOrDefault(a => a.Produit.Id == produitId);
            if (produit != null)
            {
                CommandeProduit.Remove(produit);
            }
            return this;
        }
    }
}