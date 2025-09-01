using System.ComponentModel.DataAnnotations;

namespace CardManagement.Shared.DTOs;

/// <summary>
/// Request to generate a new QR code for payment
/// </summary>
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
    [Range(1, 1440)] // 1 minute to 24 hours
    public int ExpiryMinutes { get; set; } = 30;
}

/// <summary>
/// Response containing QR code details
/// </summary>
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

/// <summary>
/// QR code status and transaction details
/// </summary>
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

/// <summary>
/// Request to process QR code payment
/// </summary>
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

/// <summary>
/// Response for QR payment processing
/// </summary>
public class QRPaymentResponse
{
    public int TransactionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string MerchantName { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
}

/// <summary>
/// QR code display information for frontend
/// </summary>
public class QRCodeDisplayResponse
{
    public string QRCodeId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string MerchantName { get; set; } = string.Empty;
    public DateTime ExpiryTime { get; set; }
    public string QRCodeData { get; set; } = string.Empty;
} 