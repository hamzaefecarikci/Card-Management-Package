using CardManagement.Data.Db;
using CardManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardManagement.Data.Repositories.Implementation
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly CardManagementContext _context;

        public MerchantRepository(CardManagementContext context)
        {
            _context = context;
        }

        public async Task<Merchant> AddMerchant(Merchant merchant)
        {
            _context.Merchants.Add(merchant);
            await _context.SaveChangesAsync();
            return merchant;
        }

        public async Task<Merchant?> GetMerchantById(int id)
        {
            return await _context.Merchants.FindAsync(id);
        }

        public async Task<List<Merchant>> GetAllMerchant()
        {
            return await _context.Merchants.ToListAsync();
        }

        public async Task<bool> DeleteMerchant(int id)
        {
            var merchant = await _context.Merchants.FindAsync(id);
            if (merchant == null) return false;

            _context.Merchants.Remove(merchant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

    }
}
