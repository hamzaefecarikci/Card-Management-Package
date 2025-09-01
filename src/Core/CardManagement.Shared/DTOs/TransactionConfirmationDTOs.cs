using System.ComponentModel.DataAnnotations;

namespace CardManagement.Shared.DTOs;

/// <summary>
/// Request to confirm or cancel a pending transaction
/// </summary>
public class ConfirmTransactionRequest
{
    [Required]
    public int TransactionId { get; set; }
    
    [Required]
    public bool Confirm { get; set; }
    
    [StringLength(4, MinimumLength = 4)]
    public string? Pin { get; set; } // Required if confirming payment
    
    [StringLength(200)]
    public string? CancellationReason { get; set; } // Required if cancelling
}

/// <summary>
/// Response for transaction confirmation
/// </summary>
public class TransactionConfirmationResponse
{
    public int TransactionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string MerchantName { get; set; } = string.Empty;
    public DateTime ConfirmedAt { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Transaction status update request
/// </summary>
public class UpdateTransactionStatusRequest
{
    [Required]
    public int TransactionId { get; set; }
    
    [Required]
    public string Status { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string? FailureReason { get; set; }
    
    [StringLength(200)]
    public string? Notes { get; set; }
} 