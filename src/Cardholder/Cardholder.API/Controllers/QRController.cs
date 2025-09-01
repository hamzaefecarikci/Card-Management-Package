using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardManagement.Shared.DTOs;
using CardManagement.Shared.Models;
using Cardholder.Service.Services;

namespace Cardholder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QRController : ControllerBase
{
    private readonly IQRService _qrService;
    private readonly ILogger<QRController> _logger;

    public QRController(IQRService qrService, ILogger<QRController> logger)
    {
        _qrService = qrService;
        _logger = logger;
    }

    /// <summary>
    /// Generate a new QR code for payment
    /// </summary>
    /// <param name="request">QR generation request</param>
    /// <returns>Generated QR code details</returns>
    [HttpPost("generate")]
    [ProducesResponseType(typeof(ApiResponse<QRCodeResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> GenerateQRCode([FromBody] GenerateQRRequest request)
    {
        try
        {
            var result = await _qrService.GenerateQRCodeAsync(request);
            return Ok(ApiResponse<QRCodeResponse>.SuccessResult(result, "QR code generated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating QR code for merchant {MerchantId}", request.MerchantId);
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Process QR code payment
    /// </summary>
    /// <param name="request">QR payment request</param>
    /// <returns>Payment processing result</returns>
    [HttpPost("process")]
    [ProducesResponseType(typeof(ApiResponse<QRPaymentResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> ProcessQRPayment([FromBody] ProcessQRPaymentRequest request)
    {
        try
        {
            var result = await _qrService.ProcessQRPaymentAsync(request);
            return Ok(ApiResponse<QRPaymentResponse>.SuccessResult(result, "QR payment processed successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing QR payment for QR code {QRCodeId}", request.QRCodeId);
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Get QR code status and details
    /// </summary>
    /// <param name="qrCodeId">QR code ID</param>
    /// <returns>QR code status and transaction details</returns>
    [HttpGet("status/{qrCodeId}")]
    [ProducesResponseType(typeof(ApiResponse<QRCodeStatusResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetQRCodeStatus(string qrCodeId)
    {
        try
        {
            var result = await _qrService.GetQRCodeStatusAsync(qrCodeId);
            return Ok(ApiResponse<QRCodeStatusResponse>.SuccessResult(result, "QR code status retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting QR code status for {QRCodeId}", qrCodeId);
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }
} 