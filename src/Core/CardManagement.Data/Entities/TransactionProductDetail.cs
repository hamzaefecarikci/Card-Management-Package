using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardManagement.Data.Entities
{
    public class TransactionProductDetail
    {
        [Key]
        public int TransactionProductDetailId { get; set; }
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("TransactionId")]
        public TransactionEntity Transaction { get; set; } = null!;

        [ForeignKey("ProductId")]
        public ProductEntity Product { get; set; } = null!;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
    }
}
