using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ges_commande.Models
{
    public class Produit : AbstractEntity
    {
        [Required(ErrorMessage = "Le libellé est obligatoire.")]
        public required string Libelle { get; set; }

        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La quantité en stock doit être un nombre positif.")]
        public int QteSock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La quantité seuil doit être un nombre positif.")]
        public int QteSeuil { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Le prix unitaire doit être un nombre positif.")]
        public double PrixUnit { get; set; }

        public List<Detail>? Details { get; set; } = new List<Detail>();

    }
}