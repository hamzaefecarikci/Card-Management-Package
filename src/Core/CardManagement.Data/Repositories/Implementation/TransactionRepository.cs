using CardManagement.Data.Db;
using CardManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardManagement.Data.Repositories.Implementation
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly CardManagementContext _context;

        public TransactionRepository(CardManagementContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _context.Transactions
                .Include(t => t.Merchant)
                .Include(t => t.Card)
                    .ThenInclude(c => c.Cardholder)
                .ToListAsync();
        }

        public async Task<Transaction?> GetLatestTransaction()
        {
            return await _context.Transactions
                .Include(t => t.Merchant)
                .Include(t => t.Card)
                    .ThenInclude(c => c.Cardholder)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
