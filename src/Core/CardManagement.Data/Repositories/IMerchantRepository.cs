using CardManagement.Data.Entities;

namespace CardManagement.Data.Repositories
{
    public interface IMerchantRepository
    {
        Task<Merchant> AddMerchant(Merchant merchant);
        Task<Merchant?> GetMerchantById(int id);
        Task<List<Merchant>> GetAllMerchant();
        Task<bool> DeleteMerchant(int id);
        Task SaveChanges();
    }
}
