using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardManagement.Data.Entities;

public class QRCode
{
    [Key]
    public int QRCodeId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string QRCodeIdString { get; set; } = string.Empty;
    
    public int MerchantId { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }
    
    [StringLength(200)]
    public string? Description { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Active";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime ExpiryTime { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    public int? TransactionId { get; set; }
    
    public int? CardId { get; set; }
    
    // Navigation properties
    [ForeignKey("MerchantId")]
    public virtual Merchant Merchant { get; set; } = null!;
    
    [ForeignKey("TransactionId")]
    public virtual Transaction? Transaction { get; set; }
    
    [ForeignKey("CardId")]
    public virtual Card? Card { get; set; }
} 