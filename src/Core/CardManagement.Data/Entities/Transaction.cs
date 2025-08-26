using System.ComponentModel.DataAnnotations.Schema;

namespace CardManagement.Data.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int MerchantId { get; set; }

        [ForeignKey("MerchantId")]
        public Merchant Merchant { get; set; } = null!;

        public int CardId { get; set; }

        [ForeignKey("CardId")]
        public Card Card { get; set; } = null!;

        [Column(TypeName = "char(20)")]
        public required string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
    }
}
