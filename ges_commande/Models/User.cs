using ges_commande.Models.Enum;

namespace ges_commande.Models
{
    public class User : Personne
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public Role Role { get; set; }
    }
}