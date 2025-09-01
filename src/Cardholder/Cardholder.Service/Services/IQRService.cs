using CardManagement.Shared.DTOs;

namespace Cardholder.Service.Services;

public interface IQRService
{
    /// <summary>
    /// Generate a new QR code for payment
    /// </summary>
    /// <param name="request">QR generation request</param>
    /// <returns>Generated QR code details</returns>
    Task<QRCodeResponse> GenerateQRCodeAsync(GenerateQRRequest request);
    
    /// <summary>
    /// Process QR code payment
    /// </summary>
    /// <param name="request">QR payment request</param>
    /// <returns>Payment processing result</returns>
    Task<QRPaymentResponse> ProcessQRPaymentAsync(ProcessQRPaymentRequest request);
    
    /// <summary>
    /// Get QR code status and details
    /// </summary>
    /// <param name="qrCodeId">QR code ID</param>
    /// <returns>QR code status and transaction details</returns>
    Task<QRCodeStatusResponse> GetQRCodeStatusAsync(string qrCodeId);
} 