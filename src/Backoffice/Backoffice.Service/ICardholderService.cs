using CardManagement.Data.Entities;
using CardManagement.Shared.DTOs.CardholderDTOs;

namespace Backoffice.Service
{
    public interface ICardholderService
    {
        Task<Cardholder> CreateCardholder(CreateCardholderDto createCardholderDto);
        Task<GetCardholderDTO?> GetCardholderById(int id);
        Task<List<GetCardholderDTO>> GetAllCardholders();
        Task<bool> DeleteCardholder(int id);
        Task<GetCardholderDTO?> UpdateCardholder(int id, UpdateCardholderDTO updateCardholderDTO);
    }
}
