using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardManagement.Shared.DTOs;
using CardManagement.Shared.Models;
using Cardholder.Service.Services;

namespace Cardholder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }

    /// <summary>
    /// Confirm or cancel a pending transaction
    /// </summary>
    /// <param name="request">Transaction confirmation request</param>
    /// <returns>Transaction confirmation result</returns>
    [HttpPost("confirm")]
    [ProducesResponseType(typeof(ApiResponse<TransactionConfirmationResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> ConfirmTransaction([FromBody] ConfirmTransactionRequest request)
    {
        try
        {
            var result = await _transactionService.ConfirmTransactionAsync(request);
            return Ok(ApiResponse<TransactionConfirmationResponse>.SuccessResult(result, "Transaction processed successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming transaction {TransactionId}", request.TransactionId);
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Get transaction details
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <returns>Transaction details</returns>
    [HttpGet("{transactionId}")]
    [ProducesResponseType(typeof(ApiResponse<TransactionResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetTransaction(int transactionId)
    {
        try
        {
            var result = await _transactionService.GetTransactionAsync(transactionId);
            return Ok(ApiResponse<TransactionResponse>.SuccessResult(result, "Transaction retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transaction {TransactionId}", transactionId);
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Update transaction status
    /// </summary>
    /// <param name="request">Status update request</param>
    /// <returns>Updated transaction</returns>
    [HttpPut("status")]
    [ProducesResponseType(typeof(ApiResponse<TransactionResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> UpdateTransactionStatus([FromBody] UpdateTransactionStatusRequest request)
    {
        try
        {
            var result = await _transactionService.UpdateTransactionStatusAsync(request);
            return Ok(ApiResponse<TransactionResponse>.SuccessResult(result, "Transaction status updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating transaction status for {TransactionId}", request.TransactionId);
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Process successful payment
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <returns>Success result</returns>
    [HttpPost("{transactionId}/success")]
    [ProducesResponseType(typeof(ApiResponse<TransactionConfirmationResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> ProcessSuccessfulPayment(int transactionId)
    {
        try
        {
            var result = await _transactionService.ProcessSuccessfulPaymentAsync(transactionId);
            return Ok(ApiResponse<TransactionConfirmationResponse>.SuccessResult(result, "Payment processed successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing successful payment for transaction {TransactionId}", transactionId);
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Process failed payment
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <param name="failureReason">Reason for failure</param>
    /// <returns>Failure result</returns>
    [HttpPost("{transactionId}/failed")]
    [ProducesResponseType(typeof(ApiResponse<TransactionConfirmationResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> ProcessFailedPayment(int transactionId, [FromBody] string failureReason)
    {
        try
        {
            var result = await _transactionService.ProcessFailedPaymentAsync(transactionId, failureReason);
            return Ok(ApiResponse<TransactionConfirmationResponse>.SuccessResult(result, "Payment failure processed"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing failed payment for transaction {TransactionId}", transactionId);
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }
} 