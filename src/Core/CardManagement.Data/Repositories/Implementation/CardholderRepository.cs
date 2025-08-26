using CardManagement.Data.Db;
using CardManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardManagement.Data.Repositories.Implementation
{
    public class CardholderRepository : ICardholderRepository
    {
        private readonly CardManagementContext _context;

        public CardholderRepository(CardManagementContext context)
        {
            _context = context;
        }

        public async Task<Cardholder> AddCardholder(Cardholder cardholder)
        {
            _context.Cardholders.Add(cardholder);
            await _context.SaveChangesAsync();
            return cardholder;
        }

        public async Task<Cardholder?> GetCardholderById(int id)
        {
            return await _context.Cardholders
                .Include(c => c.Cards)
                .FirstOrDefaultAsync(c => c.CardholderId == id);
        }

        public async Task<List<Cardholder>> GetAllCardholders()
        {
            return await _context.Cardholders
                .Include(c => c.Cards)
                .ToListAsync();
        }

        public async Task<bool> DeleteCardholder(int id)
        {
            var cardholder = await _context.Cardholders.FindAsync(id);
            if (cardholder == null) return false;

            _context.Cardholders.Remove(cardholder);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
