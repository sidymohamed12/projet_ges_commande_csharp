namespace ges_commande.Models
{
    public class Personne : AbstractEntity
    {
        public required string Nom { get; set; }
        public required string Prenom { get; set; }
        public required string Telephone { get; set; }
    }
}