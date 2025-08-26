using System.ComponentModel.DataAnnotations.Schema;

namespace CardManagement.Data.Entities
{
    public class Card
    {
        public int CardId { get; set; }

        public int CardholderId { get; set; }

        [ForeignKey("CardholderId")]
        public Cardholder Cardholder { get; set; } = null!;

        [Column(TypeName = "char(16)")]
        public required string CardNumber { get; set; }

        [Column(TypeName = "char(5)")]
        public required string ExpiryDate { get; set; }

        [Column(TypeName = "char(3)")]
        public required string CVV { get; set; }

        [Column(TypeName = "char(4)")]
        public required string Pin { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Balance { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
