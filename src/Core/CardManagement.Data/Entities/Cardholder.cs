using System.ComponentModel.DataAnnotations.Schema;

namespace CardManagement.Data.Entities
{
    public class Cardholder
    {
        public int CardholderId { get; set; }

        [Column(TypeName = "char(100)")]
        public required string FullName { get; set; }

        [Column(TypeName = "char(100)")]
        public required string Email { get; set; }

        [Column(TypeName = "char(100)")]
        public required string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
