using System.ComponentModel.DataAnnotations.Schema;

namespace ges_commande.Models
{
    public class Livreur : Personne
    {
        public bool IsDisponibile { get; set; }
        public List<Livraison>? Livraisons { get; set; } = new List<Livraison>();
    }
}