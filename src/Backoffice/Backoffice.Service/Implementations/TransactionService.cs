using CardManagement.Data.Entities;
using CardManagement.Shared.DTOs.TransactionDTOs;
using CardManagement.Data.Repositories;

namespace Backoffice.Service.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;

        public TransactionService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetTransactionDTO>> GetAllTransactionsAsync()
        {
            var transactions = await _repository.GetAllTransactions();
            return transactions.Select(MapToDTO).ToList();
        }

        public async Task<GetTransactionDTO?> GetLatestTransactionAsync()
        {
            var transaction = await _repository.GetLatestTransaction();
            return MapToDTO(transaction);
        }

        private static GetTransactionDTO MapToDTO(Transaction transaction)
        {
            return new GetTransactionDTO
            {
                MerchantName = transaction.Merchant.Name,
                CardholderName = transaction.Card.Cardholder.FullName,
                Status = transaction.Status,
                TotalAmount = transaction.TotalAmount,
                CreatedAt = transaction.CreatedAt
            };
        }
    }
}
