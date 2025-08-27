using System.ComponentModel.DataAnnotations.Schema;

namespace CardManagement.Data.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public int MerchantId { get; set; }

        [ForeignKey("MerchantId")]
        public Merchant Merchant { get; set; } = null!;

        [Column(TypeName = "char(100)")]
        public required string Name { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
