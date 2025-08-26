using System.ComponentModel.DataAnnotations.Schema;

namespace CardManagement.Data.Entities
{
    public class Merchant
    {
        public int MerchantId { get; set; }

        [Column(TypeName = "char(100)")]
        public required string Name { get; set; }

        [Column(TypeName = "char(100)")]
        public required string Email { get; set; }

        [Column(TypeName = "char(100)")]
        public required string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
