using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardManagement.Data.Entities;

public class Merchant
{
    [Key]
    public int MerchantId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string MerchantName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Address { get; set; }
    
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    
    public virtual ICollection<QRCode> QRCodes { get; set; } = new List<QRCode>();
} 