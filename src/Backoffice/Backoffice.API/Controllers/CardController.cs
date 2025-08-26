using Backoffice.Service;
using CardManagement.Shared.DTOs.CardDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backoffice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCardDTO createCardDTO)
        {
            var createdCard = await _cardService.CreateCard(createCardDTO);
            return CreatedAtAction(null, new { id = createdCard.CardId }, createdCard);
        }
    }
}
