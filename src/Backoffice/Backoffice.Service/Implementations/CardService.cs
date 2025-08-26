using CardManagement.Data.Entities;
using CardManagement.Data.Repositories;
using CardManagement.Shared.DTOs.CardDTOs;

namespace Backoffice.Service.Implementations
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _repository;

        public CardService(ICardRepository repository)
        {
            _repository = repository;
        }

        public async Task<Card> CreateCard(CreateCardDTO createCardDTO)
        {
            var card = new Card
            {
                CardholderId = createCardDTO.CardholderId,
                CardNumber = createCardDTO.CardNumber,
                ExpiryDate = createCardDTO.ExpiryDate,
                CVV = createCardDTO.CVV,
                Pin = createCardDTO.Pin,
                Balance = createCardDTO.Balance
            };

            return await _repository.AddCard(card);
        }
    }
}
