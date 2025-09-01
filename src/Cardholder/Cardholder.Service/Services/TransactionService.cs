using CardManagement.Data;
using CardManagement.Data.Entities;
using CardManagement.Data.Repositories;
using CardManagement.Shared.DTOs;
using CardManagement.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Cardholder.Service.Services;

public class TransactionService : ITransactionService
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<Card> _cardRepository;
    private readonly IRepository<Merchant> _merchantRepository;
    private readonly IRepository<QRCode> _qrCodeRepository;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(
        IRepository<Transaction> transactionRepository,
        IRepository<Card> cardRepository,
        IRepository<Merchant> merchantRepository,
        IRepository<QRCode> qrCodeRepository,
        ILogger<TransactionService> logger)
    {
        _transactionRepository = transactionRepository;
        _cardRepository = cardRepository;
        _merchantRepository = merchantRepository;
        _qrCodeRepository = qrCodeRepository;
        _logger = logger;
    }

    public async Task<TransactionConfirmationResponse> ConfirmTransactionAsync(ConfirmTransactionRequest request)
    {
        try
        {
            var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId);
            if (transaction == null)
            {
                throw new NotFoundException("Transaction not found");
            }

            if (transaction.Status != "Pending")
            {
                throw new ValidationException($"Transaction is not in pending status. Current status: {transaction.Status}");
            }

            if (request.Confirm)
            {
                // Validate PIN if provided
                if (!string.IsNullOrEmpty(request.Pin))
                {
                    var card = await _cardRepository.GetByIdAsync(transaction.CardId);
                    if (card == null)
                    {
                        throw new NotFoundException("Card not found");
                    }

                    if (card.Pin != request.Pin)
                    {
                        throw new ValidationException("Invalid PIN");
                    }
                }

                // Process successful payment
                return await ProcessSuccessfulPaymentAsync(request.TransactionId);
            }
            else
            {
                // Cancel transaction
                if (string.IsNullOrEmpty(request.CancellationReason))
                {
                    throw new ValidationException("Cancellation reason is required");
                }

                transaction.Status = "Cancelled";
                transaction.UpdatedAt = DateTime.UtcNow;
                await _transactionRepository.UpdateAsync(transaction);

                // Update QR code status
                var qrCode = await _qrCodeRepository.FindAsync(q => q.TransactionId == request.TransactionId);
                if (qrCode.Any())
                {
                    var qr = qrCode.First();
                    qr.Status = "Cancelled";
                    await _qrCodeRepository.UpdateAsync(qr);
                }

                return new TransactionConfirmationResponse
                {
                    TransactionId = transaction.TransactionId,
                    Status = "Cancelled",
                    Message = "Transaction cancelled successfully",
                    Amount = transaction.Amount,
                    MerchantName = (await _merchantRepository.GetByIdAsync(transaction.MerchantId))?.MerchantName ?? "Unknown",
                    ConfirmedAt = DateTime.UtcNow
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming transaction {TransactionId}", request.TransactionId);
            throw;
        }
    }

    public async Task<TransactionResponse> UpdateTransactionStatusAsync(UpdateTransactionStatusRequest request)
    {
        try
        {
            var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId);
            if (transaction == null)
            {
                throw new NotFoundException("Transaction not found");
            }

            transaction.Status = request.Status;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _transactionRepository.UpdateAsync(transaction);

            var merchant = await _merchantRepository.GetByIdAsync(transaction.MerchantId);

            return new TransactionResponse
            {
                TransactionId = transaction.TransactionId,
                CardId = transaction.CardId,
                MerchantId = transaction.MerchantId,
                MerchantName = merchant?.MerchantName ?? "Unknown",
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                Status = transaction.Status,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating transaction status for {TransactionId}", request.TransactionId);
            throw;
        }
    }

    public async Task<TransactionResponse> GetTransactionAsync(int transactionId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId);
        if (transaction == null)
        {
            throw new NotFoundException("Transaction not found");
        }

        var merchant = await _merchantRepository.GetByIdAsync(transaction.MerchantId);

        return new TransactionResponse
        {
            TransactionId = transaction.TransactionId,
            CardId = transaction.CardId,
            MerchantId = transaction.MerchantId,
            MerchantName = merchant?.MerchantName ?? "Unknown",
            Amount = transaction.Amount,
            TransactionType = transaction.TransactionType,
            Status = transaction.Status,
            CreatedAt = transaction.CreatedAt,
            UpdatedAt = transaction.UpdatedAt
        };
    }

    public async Task<TransactionConfirmationResponse> ProcessSuccessfulPaymentAsync(int transactionId)
    {
        try
        {
            var transaction = await _transactionRepository.GetByIdAsync(transactionId);
            if (transaction == null)
            {
                throw new NotFoundException("Transaction not found");
            }

            // Get card and update balance
            var card = await _cardRepository.GetByIdAsync(transaction.CardId);
            if (card == null)
            {
                throw new NotFoundException("Card not found");
            }

            // Check if balance is sufficient
            if (card.Balance < transaction.Amount)
            {
                // Process as failed payment
                return await ProcessFailedPaymentAsync(transactionId, "Insufficient balance");
            }

            // Deduct amount from card balance
            card.Balance -= transaction.Amount;
            card.UpdatedAt = DateTime.UtcNow;
            await _cardRepository.UpdateAsync(card);

            // Update transaction status
            transaction.Status = "Success";
            transaction.UpdatedAt = DateTime.UtcNow;
            await _transactionRepository.UpdateAsync(transaction);

            // Update QR code status
            var qrCode = await _qrCodeRepository.FindAsync(q => q.TransactionId == transactionId);
            if (qrCode.Any())
            {
                var qr = qrCode.First();
                qr.Status = "Completed";
                qr.CompletedAt = DateTime.UtcNow;
                await _qrCodeRepository.UpdateAsync(qr);
            }

            var merchant = await _merchantRepository.GetByIdAsync(transaction.MerchantId);

            return new TransactionConfirmationResponse
            {
                TransactionId = transaction.TransactionId,
                Status = "Success",
                Message = "Payment completed successfully",
                Amount = transaction.Amount,
                MerchantName = merchant?.MerchantName ?? "Unknown",
                ConfirmedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing successful payment for transaction {TransactionId}", transactionId);
            throw;
        }
    }

    public async Task<TransactionConfirmationResponse> ProcessFailedPaymentAsync(int transactionId, string failureReason)
    {
        try
        {
            var transaction = await _transactionRepository.GetByIdAsync(transactionId);
            if (transaction == null)
            {
                throw new NotFoundException("Transaction not found");
            }

            // Update transaction status
            transaction.Status = "Failed";
            transaction.UpdatedAt = DateTime.UtcNow;
            await _transactionRepository.UpdateAsync(transaction);

            // Update QR code status
            var qrCode = await _qrCodeRepository.FindAsync(q => q.TransactionId == transactionId);
            if (qrCode.Any())
            {
                var qr = qrCode.First();
                qr.Status = "Failed";
                await _qrCodeRepository.UpdateAsync(qr);
            }

            var merchant = await _merchantRepository.GetByIdAsync(transaction.MerchantId);

            return new TransactionConfirmationResponse
            {
                TransactionId = transaction.TransactionId,
                Status = "Failed",
                Message = $"Payment failed: {failureReason}",
                Amount = transaction.Amount,
                MerchantName = merchant?.MerchantName ?? "Unknown",
                ConfirmedAt = DateTime.UtcNow,
                ErrorCode = "PAYMENT_FAILED",
                ErrorMessage = failureReason
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing failed payment for transaction {TransactionId}", transactionId);
            throw;
        }
    }
} 