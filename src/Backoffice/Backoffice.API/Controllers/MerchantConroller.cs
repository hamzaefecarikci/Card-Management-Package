using Backoffice.Service;
using Backoffice.Service.Implementations;
using CardManagement.Shared.DTOs.MerchantDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backoffice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMerchant(CreateMerchantDto createMerchantDto)
        {
            var merchant = await _merchantService.CreateMerchant(createMerchantDto);
            return Ok(merchant);
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var merchant = await _merchantService.GetMerchantById(id);
            if (merchant == null)
                return NotFound();

            return Ok(merchant);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var merchants = await _merchantService.GetAllMerchants();
            return Ok(merchants);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _merchantService.DeleteMerchant(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMerchantDto dto)
        {
            var updatedMerchant = await _merchantService.UpdateMerchant(id, dto);
            if (updatedMerchant == null) return NotFound();
            return Ok(updatedMerchant);
        }
    }
}
