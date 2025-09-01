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

// QR Code related DTOs
public class GenerateQRRequest
{
    [Required]
    public int MerchantId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
    
    [StringLength(200)]
    public string? Description { get; set; }
    
    [Required]
    public int ExpiryMinutes { get; set; } = 30;
}

public class QRCodeResponse
{
    public string QRCodeId { get; set; } = string.Empty;
    public int MerchantId { get; set; }
    public string MerchantName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiryTime { get; set; }
    public string QRCodeData { get; set; } = string.Empty;
}

public class QRCodeStatusResponse
{
    public string QRCodeId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiryTime { get; set; }
    public DateTime? CompletedAt { get; set; }
    public TransactionResponse? Transaction { get; set; }
}

public class ProcessQRPaymentRequest
{
    [Required]
    public string QRCodeId { get; set; } = string.Empty;
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    [StringLength(4, MinimumLength = 4)]
    public string Pin { get; set; } = string.Empty;
}

public class ConfirmQRPaymentRequest
{
    [Required]
    public int TransactionId { get; set; }
    
    [Required]
    public bool Confirm { get; set; }
    
    [StringLength(4, MinimumLength = 4)]
    public string? Pin { get; set; }
    
    [StringLength(200)]
    public string? CancellationReason { get; set; }
}

public class QRPaymentResponse
{
    public int TransactionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string MerchantName { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
}

public class QRCodeDisplayResponse
{
    public string QRCodeId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string MerchantName { get; set; } = string.Empty;
    public DateTime ExpiryTime { get; set; }
    public string QRCodeData { get; set; } = string.Empty;
} 