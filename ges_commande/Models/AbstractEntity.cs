using System.ComponentModel.DataAnnotations.Schema;

namespace ges_commande.Models
{
    public class AbstractEntity
    {


        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // public User? CreatedBy { get; set; }
        // public int? CreatedById { get; set; }
        // public User? UpdatedBy { get; set; }
        // public int? UpdatedById { get; set; }

        public void OnPrePersist()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            // CreatedBy = UserConnect.UserConnecte;
            // UpdatedBy = UserConnect.UserConnecte;
        }

        public void OnPreUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
            // UpdatedBy = UserConnect.UserConnecte;
        }


    }
}