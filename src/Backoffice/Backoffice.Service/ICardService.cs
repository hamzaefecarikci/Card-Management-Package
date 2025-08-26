using CardManagement.Data.Entities;
using CardManagement.Shared.DTOs.CardDTOs;

namespace Backoffice.Service
{
    public interface ICardService
    {
        Task<Card> CreateCard(CreateCardDTO createCardDTO);
    }
}