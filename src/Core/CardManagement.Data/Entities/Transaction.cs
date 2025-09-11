using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardManagement.Data.Entities;

public class Transaction
{
    [Key]
    public int TransactionId { get; set; }
    
    public int CardId { get; set; }
    
    public int MerchantId { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }
    
    [Required]
    [StringLength(50)]
    public string TransactionType { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Pending";
    
    [StringLength(200)]
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    [ForeignKey("CardId")]
    public virtual Card Card { get; set; } = null!;
    
    [ForeignKey("MerchantId")]
    public virtual Merchant Merchant { get; set; } = null!;
} 