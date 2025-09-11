using System.ComponentModel.DataAnnotations;

namespace CardManagement.Shared.DTOs;

public class CreateTransactionRequest
{
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public int MerchantId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
    
    [Required]
    [StringLength(50)]
    public string TransactionType { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string? Description { get; set; }
}

public class TransactionResponse
{
    public int TransactionId { get; set; }
    public int CardId { get; set; }
    public int MerchantId { get; set; }
    public string MerchantName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
} 