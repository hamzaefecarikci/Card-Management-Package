using CardManagement.Shared.DTOs;

namespace Cardholder.Service.Services;

public interface ITransactionService
{
    /// <summary>
    /// Confirm or cancel a pending transaction
    /// </summary>
    /// <param name="request">Transaction confirmation request</param>
    /// <returns>Transaction confirmation result</returns>
    Task<TransactionConfirmationResponse> ConfirmTransactionAsync(ConfirmTransactionRequest request);
    
    /// <summary>
    /// Update transaction status
    /// </summary>
    /// <param name="request">Status update request</param>
    /// <returns>Updated transaction</returns>
    Task<TransactionResponse> UpdateTransactionStatusAsync(UpdateTransactionStatusRequest request);
    
    /// <summary>
    /// Get transaction details
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <returns>Transaction details</returns>
    Task<TransactionResponse> GetTransactionAsync(int transactionId);
    
    /// <summary>
    /// Process successful payment
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <returns>Success result</returns>
    Task<TransactionConfirmationResponse> ProcessSuccessfulPaymentAsync(int transactionId);
    
    /// <summary>
    /// Process failed payment
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <param name="failureReason">Reason for failure</param>
    /// <returns>Failure result</returns>
    Task<TransactionConfirmationResponse> ProcessFailedPaymentAsync(int transactionId, string failureReason);
} 