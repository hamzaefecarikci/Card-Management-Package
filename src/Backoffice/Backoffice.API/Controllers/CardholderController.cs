using Backoffice.Service;
using Backoffice.Service.Implementations;
using CardManagement.Shared.DTOs.CardholderDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backoffice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardholderController : ControllerBase
    {
        private readonly ICardholderService _cardholderService;

        public CardholderController(ICardholderService cardholderService)
        {
            _cardholderService = cardholderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cardholders = await _cardholderService.GetAllCardholders();
            return Ok(cardholders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cardholder = await _cardholderService.GetCardholderById(id);
            if (cardholder == null) return NotFound();
            return Ok(cardholder);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCardholderDto createCardholderDto)
        {
            var created = await _cardholderService.CreateCardholder(createCardholderDto);
            return CreatedAtAction(nameof(GetById), new { id = created.CardholderId }, created);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCardholderDTO updateCardholderDTO)
        {
            var updated = await _cardholderService.UpdateCardholder(id, updateCardholderDTO);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _cardholderService.DeleteCardholder(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
