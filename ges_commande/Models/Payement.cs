using ges_commande.Models.Enum;

namespace ges_commande.Models
{
    public class Payement : AbstractEntity
    {
        public bool HasReduction { get; set; }
        public double Montant { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public TypePayement Type { get; set; }
        public Commande Commande { get; set; }
        public int CommandeId { get; set; }
    }
}