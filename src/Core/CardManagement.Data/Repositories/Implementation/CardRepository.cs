using CardManagement.Data.Db;
using CardManagement.Data.Entities;
using System.Threading.Tasks;

namespace CardManagement.Data.Repositories.Implementation
{
    public class CardRepository : ICardRepository
    {
        private readonly CardManagementContext _context;

        public CardRepository(CardManagementContext context)
        {
            _context = context;
        }

        public async Task<Card> AddCard(Card card)
        {
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            return card;
        }
    }
}