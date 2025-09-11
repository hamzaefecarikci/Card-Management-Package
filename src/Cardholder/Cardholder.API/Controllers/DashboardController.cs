using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardManagement.Data.Entities;
using CardManagement.Data.Repositories;
using CardManagement.Shared.DTOs;
using CardManagement.Shared.Models;
using System.Security.Claims;

namespace Cardholder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IRepository<Card> _cardRepository;
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<Merchant> _merchantRepository;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IRepository<Card> cardRepository,
        IRepository<Transaction> transactionRepository,
        IRepository<Merchant> merchantRepository,
        ILogger<DashboardController> logger)
    {
        _cardRepository = cardRepository;
        _transactionRepository = transactionRepository;
        _merchantRepository = merchantRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get dashboard overview for the current user
    /// </summary>
    /// <returns>Dashboard data including stats, recent transactions, and card balances</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<DashboardResponse>), 200)]
    public async Task<IActionResult> GetDashboard()
    {
        try
        {
            var cardholderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (cardholderId == 0)
            {
                return Unauthorized(ApiResponse<object>.ErrorResult("Invalid token"));
            }

            // Get user's cards
            var cards = await _cardRepository.FindAsync(c => c.CardholderId == cardholderId);
            var activeCards = cards.Where(c => c.Status == "Active").ToList();

            // Get user's transactions
            var transactions = await _transactionRepository.FindAsync(t => cards.Any(c => c.CardId == t.CardId));

            // Calculate stats
            var stats = new CardholderStats
            {
                TotalCards = cards.Count(),
                ActiveCards = activeCards.Count(),
                TotalBalance = activeCards.Sum(c => c.Balance),
                TotalTransactions = transactions.Count(),
                TotalSpent = transactions.Where(t => t.TransactionType == "Payment" && t.Status == "Success").Sum(t => t.Amount),
                TotalReceived = transactions.Where(t => t.TransactionType == "Deposit" && t.Status == "Success").Sum(t => t.Amount)
            };

            // Get recent transactions
            var recentTransactions = transactions
                .OrderByDescending(t => t.CreatedAt)
                .Take(10)
                .Select(t => new RecentTransaction
                {
                    TransactionId = t.TransactionId,
                    MerchantName = GetMerchantName(t.MerchantId).Result,
                    Amount = t.Amount,
                    TransactionType = t.TransactionType,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt
                })
                .ToList();

            // Get card balances
            var cardBalances = activeCards
                .Select(c => new CardBalance
                {
                    CardId = c.CardId,
                    CardNumber = c.CardNumber,
                    Balance = c.Balance,
                    Status = c.Status,
                    LastTransactionDate = transactions
                        .Where(t => t.CardId == c.CardId)
                        .OrderByDescending(t => t.CreatedAt)
                        .Select(t => t.CreatedAt)
                        .FirstOrDefault()
                })
                .ToList();

            var dashboard = new DashboardResponse
            {
                Stats = stats,
                RecentTransactions = recentTransactions,
                CardBalances = cardBalances
            };

            return Ok(ApiResponse<DashboardResponse>.SuccessResult(dashboard, "Dashboard data retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dashboard data");
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Get transaction history for the current user
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 20)</param>
    /// <returns>Paginated transaction history</returns>
    [HttpGet("transaction-history")]
    [ProducesResponseType(typeof(ApiResponse<List<TransactionResponse>>), 200)]
    public async Task<IActionResult> GetTransactionHistory(int page = 1, int pageSize = 20)
    {
        try
        {
            var cardholderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (cardholderId == 0)
            {
                return Unauthorized(ApiResponse<object>.ErrorResult("Invalid token"));
            }

            // Get user's cards
            var cards = await _cardRepository.FindAsync(c => c.CardholderId == cardholderId);
            var cardIds = cards.Select(c => c.CardId).ToList();

            // Get user's transactions
            var allTransactions = await _transactionRepository.FindAsync(t => cardIds.Contains(t.CardId));
            
            // Apply pagination
            var transactions = allTransactions
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var transactionResponses = new List<TransactionResponse>();
            foreach (var transaction in transactions)
            {
                var merchantName = await GetMerchantName(transaction.MerchantId);
                transactionResponses.Add(new TransactionResponse
                {
                    TransactionId = transaction.TransactionId,
                    CardId = transaction.CardId,
                    MerchantId = transaction.MerchantId,
                    MerchantName = merchantName,
                    Amount = transaction.Amount,
                    TransactionType = transaction.TransactionType,
                    Status = transaction.Status,
                    Description = transaction.Description,
                    CreatedAt = transaction.CreatedAt,
                    UpdatedAt = transaction.UpdatedAt
                });
            }

            return Ok(ApiResponse<List<TransactionResponse>>.SuccessResult(transactionResponses, "Transaction history retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transaction history");
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    private async Task<string> GetMerchantName(int merchantId)
    {
        try
        {
            var merchant = await _merchantRepository.GetByIdAsync(merchantId);
            return merchant?.MerchantName ?? "Unknown";
        }
        catch
        {
            return "Unknown";
        }
    }
} 