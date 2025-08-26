using Backoffice.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backoffice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService service)
        {
            _transactionService = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestTransaction()
        {
            var transaction = await _transactionService.GetLatestTransactionAsync();
            if (transaction == null) return NotFound();
            return Ok(transaction);
        }
    }
}
