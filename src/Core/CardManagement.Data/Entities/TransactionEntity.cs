using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardManagement.Data.Entities
{
    public class TransactionEntity
    {
        [Key]
        public int TransactionId { get; set; }

        public int MerchantId { get; set; }

        [ForeignKey("MerchantId")]
        public MerchantEntity Merchant { get; set; } = null!;

        public int CardId { get; set; }

        [ForeignKey("CardId")]
        public Card Card { get; set; } = null!;

        [Column(TypeName = "nvarchar(100)")]
        public required string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
    }
}
