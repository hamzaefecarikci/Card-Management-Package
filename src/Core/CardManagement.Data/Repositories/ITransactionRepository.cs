using CardManagement.Data.Entities;

namespace CardManagement.Data.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllTransactions();
        Task<Transaction?> GetLatestTransaction();
    }
}
