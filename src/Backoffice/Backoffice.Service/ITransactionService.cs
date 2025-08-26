using CardManagement.Shared.DTOs.TransactionDTOs;

namespace Backoffice.Service
{
    public interface ITransactionService
    {
        Task<List<GetTransactionDTO>> GetAllTransactionsAsync();
        Task<GetTransactionDTO?> GetLatestTransactionAsync();
    }
}
