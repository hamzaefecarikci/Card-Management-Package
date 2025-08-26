using CardManagement.Data.Entities;
using System.Threading.Tasks;

namespace CardManagement.Data.Repositories
{
    public interface ICardRepository
    {
        Task<Card> AddCard(Card card);
    }
}