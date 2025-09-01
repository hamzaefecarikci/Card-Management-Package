using CardManagement.Data.Entities;
using CardManagement.Data.Repositories;
using CardManagement.Shared.DTOs;
using CardManagement.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Cardholder.Service.Services;

public class QRService : IQRService
{
    private readonly IRepository<QRCode> _qrCodeRepository;
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<Card> _cardRepository;
    private readonly IRepository<Merchant> _merchantRepository;
    private readonly ILogger<QRService> _logger;

    public QRService(
        IRepository<QRCode> qrCodeRepository,
        IRepository<Transaction> transactionRepository,
        IRepository<Card> cardRepository,
        IRepository<Merchant> merchantRepository,
        ILogger<QRService> logger)
    {
        _qrCodeRepository = qrCodeRepository;
        _transactionRepository = transactionRepository;
        _cardRepository = cardRepository;
        _merchantRepository = merchantRepository;
        _logger = logger;
    }

    public async Task<QRCodeResponse> GenerateQRCodeAsync(GenerateQRRequest request)
    {
        try
        {
            var merchant = await _merchantRepository.GetByIdAsync(request.MerchantId);
            if (merchant == null)
            {
                throw new NotFoundException("Merchant not found");
            }

            var qrCodeId = GenerateUniqueQRCodeId();
            var expiryTime = DateTime.UtcNow.AddMinutes(request.ExpiryMinutes);

            var qrCode = new QRCode
            {
                QRCodeIdString = qrCodeId,
                MerchantId = request.MerchantId,
                Amount = request.Amount,
                Description = request.Description,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                ExpiryTime = expiryTime
            };

            var createdQRCode = await _qrCodeRepository.AddAsync(qrCode);

            var qrCodeData = GenerateQRCodeData(createdQRCode);

            return new QRCodeResponse
            {
                QRCodeId = createdQRCode.QRCodeIdString,
                MerchantId = createdQRCode.MerchantId,
                MerchantName = merchant.MerchantName,
                Amount = createdQRCode.Amount,
                Description = createdQRCode.Description,
                Status = createdQRCode.Status,
                CreatedAt = createdQRCode.CreatedAt,
                ExpiryTime = createdQRCode.ExpiryTime,
                QRCodeData = qrCodeData
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating QR code for merchant {MerchantId}", request.MerchantId);
            throw;
        }
    }

    public async Task<QRPaymentResponse> ProcessQRPaymentAsync(ProcessQRPaymentRequest request)
    {
        try
        {
            var qrCodes = await _qrCodeRepository.FindAsync(q => q.QRCodeIdString == request.QRCodeId);
            var qrCode = qrCodes.FirstOrDefault();

            if (qrCode == null)
            {
                throw new NotFoundException("QR code not found");
            }

            if (qrCode.Status != "Active")
            {
                throw new ValidationException($"QR code is not active. Current status: {qrCode.Status}");
            }

            if (DateTime.UtcNow > qrCode.ExpiryTime)
            {
                throw new ValidationException("QR code has expired");
            }

            var card = await _cardRepository.GetByIdAsync(request.CardId);
            if (card == null)
            {
                throw new NotFoundException("Card not found");
            }

            if (card.Pin != request.Pin)
            {
                throw new ValidationException("Invalid PIN");
            }

            if (card.Balance < qrCode.Amount)
            {
                throw new InsufficientBalanceException("Insufficient balance");
            }

            var merchant = await _merchantRepository.GetByIdAsync(qrCode.MerchantId);
            if (merchant == null)
            {
                throw new NotFoundException("Merchant not found");
            }

            // Create pending transaction
            var transaction = new Transaction
            {
                CardId = request.CardId,
                MerchantId = qrCode.MerchantId,
                Amount = qrCode.Amount,
                TransactionType = "QR Payment",
                Status = "Pending",
                Description = qrCode.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdTransaction = await _transactionRepository.AddAsync(transaction);

            // Update QR code status
            qrCode.Status = "Pending";
            qrCode.TransactionId = createdTransaction.TransactionId;
            qrCode.CardId = request.CardId;
            await _qrCodeRepository.UpdateAsync(qrCode);

            return new QRPaymentResponse
            {
                TransactionId = createdTransaction.TransactionId,
                Status = "Pending",
                Message = "Payment initiated successfully. Please confirm the transaction.",
                Amount = qrCode.Amount,
                MerchantName = merchant.MerchantName,
                ProcessedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing QR payment for QR code {QRCodeId}", request.QRCodeId);
            throw;
        }
    }

    public async Task<QRCodeStatusResponse> GetQRCodeStatusAsync(string qrCodeId)
    {
        try
        {
            var qrCodes = await _qrCodeRepository.FindAsync(q => q.QRCodeIdString == qrCodeId);
            var qrCode = qrCodes.FirstOrDefault();

            if (qrCode == null)
            {
                throw new NotFoundException("QR code not found");
            }

            var merchant = await _merchantRepository.GetByIdAsync(qrCode.MerchantId);
            TransactionResponse? transaction = null;

            if (qrCode.TransactionId.HasValue)
            {
                var transactionEntity = await _transactionRepository.GetByIdAsync(qrCode.TransactionId.Value);
                if (transactionEntity != null)
                {
                    transaction = new TransactionResponse
                    {
                        TransactionId = transactionEntity.TransactionId,
                        CardId = transactionEntity.CardId,
                        MerchantId = transactionEntity.MerchantId,
                        MerchantName = merchant?.MerchantName ?? "Unknown",
                        Amount = transactionEntity.Amount,
                        TransactionType = transactionEntity.TransactionType,
                        Status = transactionEntity.Status,
                        Description = transactionEntity.Description,
                        CreatedAt = transactionEntity.CreatedAt,
                        UpdatedAt = transactionEntity.UpdatedAt
                    };
                }
            }

            return new QRCodeStatusResponse
            {
                QRCodeId = qrCode.QRCodeIdString,
                Status = qrCode.Status,
                Amount = qrCode.Amount,
                Description = qrCode.Description,
                CreatedAt = qrCode.CreatedAt,
                ExpiryTime = qrCode.ExpiryTime,
                CompletedAt = qrCode.CompletedAt,
                Transaction = transaction
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting QR code status for {QRCodeId}", qrCodeId);
            throw;
        }
    }

    private string GenerateUniqueQRCodeId()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var random = new Random();
        var randomPart = random.Next(1000, 9999);
        return $"QR{timestamp}{randomPart}";
    }

    private string GenerateReferenceNumber()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var random = new Random();
        var randomPart = random.Next(100000, 999999);
        return $"REF{timestamp}{randomPart}";
    }

    private string GenerateQRCodeData(QRCode qrCode)
    {
        var qrData = new
        {
            qrCodeId = qrCode.QRCodeIdString,
            merchantId = qrCode.MerchantId,
            amount = qrCode.Amount,
            description = qrCode.Description,
            expiryTime = qrCode.ExpiryTime,
            referenceNumber = GenerateReferenceNumber()
        };

        return JsonSerializer.Serialize(qrData);
    }
} 