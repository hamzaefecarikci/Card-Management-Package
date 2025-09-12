using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CardManagement.Data.Entities;
using Merchant.API.Models;
using CardManagement.Data.Db;

namespace Merchant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantAuthController : ControllerBase
    {
        private readonly CardManagementContext _db;

        public MerchantAuthController(CardManagementContext db) => _db = db;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterMerchantRequest request)
        {
            if (request is null)
                return BadRequest(new { success = false, message = "Lütfen tüm alanları doldurun." });

            var exists = await _db.Merchants.AnyAsync(m => m.Email == request.Email);
            if (exists)
                return Conflict(new { success = false, message = "Bu email zaten kayıtlı." });

            var entity = new MerchantEntity
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = request.Password, // no hashing
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Merchants.Add(entity);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Kayıt başarılı",
                merchant = new MerchantDto
                {
                    MerchantId = entity.MerchantId,
                    Name = entity.Name,
                    Email = entity.Email,
                    CreatedAt = entity.CreatedAt
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request is null)
                return BadRequest(new { success = false, message = "Lütfen alanları doldurunuz." });

            var merchant = await _db.Merchants.FirstOrDefaultAsync(m => m.Email == request.Email);
            if (merchant == null)
                return NotFound(new { success = false, message = "Bu E-Mail ile kullanıcı yok." });

            if (merchant.PasswordHash != request.Password)
                return BadRequest(new { success = false, message = "Hatalı şifre girdiniz." });

            return Ok(new
            {
                success = true,
                message = "Giriş başarılı",
                merchant = new MerchantDto
                {
                    MerchantId = merchant.MerchantId,
                    Name = merchant.Name,
                    Email = merchant.Email,
                    CreatedAt = merchant.CreatedAt
                }
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMerchant(int id, [FromBody] UpdateMerchantDto updateMerchantDto)
        {
            var merchant = await _db.Merchants.FindAsync(id);
            if (merchant == null)
                return NotFound(new { success = false, message = "Merchant bulunamadı." });

            if (!string.IsNullOrEmpty(updateMerchantDto.Name))
                merchant.Name = updateMerchantDto.Name;

            if (!string.IsNullOrEmpty(updateMerchantDto.Email))
                merchant.Email = updateMerchantDto.Email;

            if (!string.IsNullOrEmpty(updateMerchantDto.Password))
                merchant.PasswordHash = updateMerchantDto.Password; // no hashing

            merchant.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Merchant bilgileri güncellendi",
                merchant = new MerchantDto
                {
                    MerchantId = merchant.MerchantId,
                    Name = merchant.Name,
                    Email = merchant.Email,
                    CreatedAt = merchant.CreatedAt
                }
            });
        }
    }
}
