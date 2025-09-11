using System.ComponentModel.DataAnnotations;

namespace CardManagement.Shared.DTOs;

public class CreateCardRequest
{
    [Required]
    [StringLength(16, MinimumLength = 16)]
    public string CardNumber { get; set; } = string.Empty;
    
    [Required]
    [StringLength(4, MinimumLength = 4)]
    public string Pin { get; set; } = string.Empty;
    
    [Required]
    public DateTime ExpiryDate { get; set; }
    
    [StringLength(50)]
    public string? CardType { get; set; }
}

public class CardResponse
{
    public int CardId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CardType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CardDetailResponse
{
    public int CardId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CardType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // TODO: Add transaction service
    public List<TransactionResponse> RecentTransactions { get; set; } = new();
} 