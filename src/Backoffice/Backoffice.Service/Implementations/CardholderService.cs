using CardManagement.Data.Entities;
using CardManagement.Data.Repositories.Implementation;
using CardManagement.Shared.DTOs.CardDTOs;
using CardManagement.Shared.DTOs.CardholderDTOs;

namespace Backoffice.Service.Implementations
{
    public class CardholderService : ICardholderService
    {
        private readonly CardholderRepository _repository;

        public CardholderService(CardholderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Cardholder> CreateCardholder(CreateCardholderDto createCardholderDto)
        {
            var PasswordHash = BCrypt.Net.BCrypt.HashPassword(createCardholderDto.Password);

            var cardholder = new Cardholder
            {
                FullName = createCardholderDto.FullName,
                Email = createCardholderDto.Email,
                PasswordHash = PasswordHash,
                CreatedAt = DateTime.Now
            };

            return await _repository.AddCardholder(cardholder);
        }

        public async Task<GetCardholderDTO?> GetCardholderById(int id)
        {
            var cardholder = await _repository.GetCardholderById(id);
            if (cardholder == null) return null;

            return MapToDTO(cardholder);
        }

        public async Task<List<GetCardholderDTO>> GetAllCardholders()
        {
            var cardholders = await _repository.GetAllCardholders();
            return cardholders.Select(MapToDTO).ToList();
        }

        public async Task<bool> DeleteCardholder(int id)
        {
            return await _repository.DeleteCardholder(id);
        }

        public async Task<GetCardholderDTO?> UpdateCardholder(int id, UpdateCardholderDTO updateCardholderDTO)
        {
            var cardholder = await _repository.GetCardholderById(id);
            if (cardholder == null) return null;

            if (!string.IsNullOrEmpty(updateCardholderDTO.FullName)) cardholder.FullName = updateCardholderDTO.FullName;
            if (!string.IsNullOrEmpty(updateCardholderDTO.Email)) cardholder.Email = updateCardholderDTO.Email;

            await _repository.SaveChangesAsync();
            return MapToDTO(cardholder);
        }

        private static GetCardholderDTO MapToDTO(Cardholder cardholder)
        {
            return new GetCardholderDTO
            {
                FullName = cardholder.FullName,
                Email = cardholder.Email,
                Cards = cardholder.Cards.Select(c => new GetCardDTO
                {
                    CardId = c.CardId,
                    CardNumber = c.CardNumber,
                    Balance = c.Balance
                }).ToList()
            };
        }
    }
}
