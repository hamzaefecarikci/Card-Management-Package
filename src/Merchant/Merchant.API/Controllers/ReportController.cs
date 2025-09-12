using CardManagement.Data.Db; // ✅ Make sure this is here
using Merchant.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Merchant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly CardManagementContext _db; // <-- Use the correct DbContext name

        public ReportController(CardManagementContext db) => _db = db; // <-- Correct type here

        [HttpPost("add")]
        public async Task<ActionResult<TransactionDto>> AddTransaction(
            [FromQuery] decimal amount,
            [FromQuery] string status = "Pending",
            [FromQuery] int merchantId = 1,
            [FromQuery] int cardId = 1)
        {
            var tx = new CardManagement.Data.Entities.TransactionEntity
            {
                MerchantId = merchantId,
                CardId = cardId,
                TotalAmount = amount,
                Status = status,
                CreatedAt = DateTime.UtcNow
            };

            _db.Transactions.Add(tx);
            await _db.SaveChangesAsync();

            return Ok(new TransactionDto
            {
                TransactionId = tx.TransactionId,
                TotalAmount = tx.TotalAmount,
                Status = tx.Status,
                CreatedAt = tx.CreatedAt
            });
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll(
            [FromQuery] int merchantId = 1,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] string? status = null)
        {
            var q = _db.Transactions.AsNoTracking().Where(t => t.MerchantId == merchantId);

            if (from.HasValue) q = q.Where(t => t.CreatedAt >= from.Value);
            if (to.HasValue) q = q.Where(t => t.CreatedAt < to.Value);
            if (!string.IsNullOrWhiteSpace(status)) q = q.Where(t => t.Status == status);

            var list = await q
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TransactionDto
                {
                    TransactionId = t.TransactionId,
                    TotalAmount = t.TotalAmount,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        [HttpPost("transactions/filter")]
        public async Task<IActionResult> GetFilteredTransactions([FromBody] TransactionFilter filter)
        {
            if (filter == null)
            {
                return BadRequest(new { success = false, message = "Filter data is required" });
            }

            var q = _db.Transactions.AsNoTracking().Where(t => t.MerchantId == filter.MerchantId);

            if (filter.From.HasValue) q = q.Where(t => t.CreatedAt >= filter.From.Value);
            if (filter.To.HasValue) q = q.Where(t => t.CreatedAt <= filter.To.Value);

            var list = await q
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TransactionDto
                {
                    TransactionId = t.TransactionId,
                    TotalAmount = t.TotalAmount,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            if (!list.Any())
            {
                return NotFound(new
                {
                    success = false,
                    message = "No transactions found for the given filter"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Transactions retrieved successfully",
                transactions = list
            });
        }

        [HttpGet("summary")]
        public async Task<ActionResult<TransactionSummaryDto>> GetSummary(
            [FromQuery] int merchantId = 1,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null)
        {
            var q = _db.Transactions.AsNoTracking().Where(t => t.MerchantId == merchantId);

            if (from.HasValue) q = q.Where(t => t.CreatedAt >= from.Value);
            if (to.HasValue) q = q.Where(t => t.CreatedAt < to.Value);

            var summary = new TransactionSummaryDto
            {
                TotalCount = await q.CountAsync(),
                TotalAmount = await q.SumAsync(t => (decimal?)t.TotalAmount) ?? 0m,
                SuccessCount = await q.CountAsync(t => t.Status == TxStatus.Success),
                FailedCount = await q.CountAsync(t => t.Status == TxStatus.Failed),
                TimeoutCount = await q.CountAsync(t => t.Status == TxStatus.Timeout)
            };

            return Ok(summary);
        }
    }
}
