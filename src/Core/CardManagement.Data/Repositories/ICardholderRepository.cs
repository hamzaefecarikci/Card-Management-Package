using CardManagement.Data.Entities;

namespace CardManagement.Data.Repositories
{
    public interface ICardholderRepository
    {
        Task<Cardholder> AddCardholder(Cardholder cardholder);
        Task<Cardholder?> GetCardholderById(int id);
        Task<List<Cardholder>> GetAllCardholders();
        Task<bool> DeleteCardholder(int id);
        Task SaveChangesAsync();
    }
}
