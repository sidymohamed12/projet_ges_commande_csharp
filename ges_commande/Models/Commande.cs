using System.ComponentModel.DataAnnotations.Schema;
using ges_commande.Models.Enum;

namespace ges_commande.Models
{
    public class Commande : AbstractEntity
    {
        public DateTime Date { get; set; }
        public double Montant { get; set; }
        public bool IsLivrer { get; set; }
        public bool IsPayer { get; set; }
        public EtatCommande Etat { get; set; } = EtatCommande.ACCEPTER;
        [NotMapped]
        public Livraison? Livraison { get; set; }
        [NotMapped]
        public int? LivraisonId { get; set; }
        public required Client Client { get; set; }
        public int ClientId { get; set; }
        [NotMapped]
        public Payement? Payement { get; set; }
        [NotMapped]
        public int? PayementId { get; set; }
        public List<Detail>? Details { get; set; } = new List<Detail>();
    }
}