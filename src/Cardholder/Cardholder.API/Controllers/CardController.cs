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
public class CardController : ControllerBase
{
    private readonly IRepository<Card> _cardRepository;
    private readonly IRepository<CardholderEntity> _cardholderRepository;
    private readonly ILogger<CardController> _logger;

    public CardController(
        IRepository<Card> cardRepository,
        IRepository<CardholderEntity> cardholderRepository,
        ILogger<CardController> logger)
    {
        _cardRepository = cardRepository;
        _cardholderRepository = cardholderRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all cards for the current user
    /// </summary>
    /// <returns>List of user cards</returns>
    [HttpGet("user-cards")]
    [ProducesResponseType(typeof(ApiResponse<List<CardResponse>>), 200)]
    public async Task<IActionResult> GetUserCards()
    {
        try
        {
            var cardholderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (cardholderId == 0)
            {
                return Unauthorized(ApiResponse<object>.ErrorResult("Invalid token"));
            }

            var cards = await _cardRepository.FindAsync(c => c.CardholderId == cardholderId);
            var cardResponses = cards.Select(c => new CardResponse
            {
                CardId = c.CardId,
                CardNumber = c.CardNumber,
                Balance = c.Balance,
                ExpiryDate = c.ExpiryDate,
                Status = c.Status,
                CardType = c.CardType,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return Ok(ApiResponse<List<CardResponse>>.SuccessResult(cardResponses, "Cards retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user cards");
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Get specific card details
    /// </summary>
    /// <param name="cardId">Card ID</param>
    /// <returns>Card details</returns>
    [HttpGet("{cardId}")]
    [ProducesResponseType(typeof(ApiResponse<CardDetailResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetCard(int cardId)
    {
        try
        {
            var cardholderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (cardholderId == 0)
            {
                return Unauthorized(ApiResponse<object>.ErrorResult("Invalid token"));
            }

            var card = await _cardRepository.GetByIdAsync(cardId);
            if (card == null)
            {
                return NotFound(ApiResponse<object>.ErrorResult("Card not found"));
            }

            if (card.CardholderId != cardholderId)
            {
                return Forbid();
            }

            var cardDetail = new CardDetailResponse
            {
                CardId = card.CardId,
                CardNumber = card.CardNumber,
                Balance = card.Balance,
                ExpiryDate = card.ExpiryDate,
                Status = card.Status,
                CardType = card.CardType,
                CreatedAt = card.CreatedAt,
                UpdatedAt = card.UpdatedAt,
                RecentTransactions = new List<TransactionResponse>()
            };

            return Ok(ApiResponse<CardDetailResponse>.SuccessResult(cardDetail, "Card details retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving card {CardId}", cardId);
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Create a new card
    /// </summary>
    /// <param name="request">Card creation request</param>
    /// <returns>Created card information</returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<CardResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest request)
    {
        try
        {
            var cardholderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (cardholderId == 0)
            {
                return Unauthorized(ApiResponse<object>.ErrorResult("Invalid token"));
            }

            if (!IsValidCardNumber(request.CardNumber))
            {
                return BadRequest(ApiResponse<object>.ErrorResult("Invalid card number"));
            }

            var card = new Card
            {
                CardNumber = request.CardNumber,
                Pin = request.Pin,
                Balance = 0,
                ExpiryDate = request.ExpiryDate,
                Status = "Active",
                CardType = request.CardType,
                CardholderId = cardholderId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdCard = await _cardRepository.AddAsync(card);

            var cardResponse = new CardResponse
            {
                CardId = createdCard.CardId,
                CardNumber = createdCard.CardNumber,
                Balance = createdCard.Balance,
                ExpiryDate = createdCard.ExpiryDate,
                Status = createdCard.Status,
                CardType = createdCard.CardType,
                CreatedAt = createdCard.CreatedAt,
                UpdatedAt = createdCard.UpdatedAt
            };

            return Ok(ApiResponse<CardResponse>.SuccessResult(cardResponse, "Card created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating card");
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Update card status
    /// </summary>
    /// <param name="cardId">Card ID</param>
    /// <param name="status">New status</param>
    /// <returns>Updated card information</returns>
    [HttpPut("{cardId}/status")]
    [ProducesResponseType(typeof(ApiResponse<CardResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> UpdateCardStatus(int cardId, [FromBody] string status)
    {
        try
        {
            var cardholderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (cardholderId == 0)
            {
                return Unauthorized(ApiResponse<object>.ErrorResult("Invalid token"));
            }

            var card = await _cardRepository.GetByIdAsync(cardId);
            if (card == null)
            {
                return NotFound(ApiResponse<object>.ErrorResult("Card not found"));
            }

            if (card.CardholderId != cardholderId)
            {
                return Forbid();
            }

            card.Status = status;
            card.UpdatedAt = DateTime.UtcNow;
            await _cardRepository.UpdateAsync(card);

            var cardResponse = new CardResponse
            {
                CardId = card.CardId,
                CardNumber = card.CardNumber,
                Balance = card.Balance,
                ExpiryDate = card.ExpiryDate,
                Status = card.Status,
                CardType = card.CardType,
                CreatedAt = card.CreatedAt,
                UpdatedAt = card.UpdatedAt
            };

            return Ok(ApiResponse<CardResponse>.SuccessResult(cardResponse, "Card status updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating card status for card {CardId}", cardId);
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    private bool IsValidCardNumber(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 16)
            return false;

        return cardNumber.All(char.IsDigit);
    }
} 