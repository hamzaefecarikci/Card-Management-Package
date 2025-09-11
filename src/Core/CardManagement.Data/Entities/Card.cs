using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardManagement.Data.Entities;

public class Card
{
    [Key]
    public int CardId { get; set; }
    
    [Required]
    [StringLength(16)]
    public string CardNumber { get; set; } = string.Empty;
    
    [Required]
    [StringLength(4)]
    public string Pin { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Balance { get; set; } = 0;
    
    [Required]
    public DateTime ExpiryDate { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Active";
    
    [StringLength(50)]
    public string? CardType { get; set; }
    
    public int CardholderId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    [ForeignKey("CardholderId")]
    public virtual CardholderEntity Cardholder { get; set; } = null!;
    
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
} 