using CardManagement.Shared.DTOs.CardDTOs;

namespace CardManagement.Shared.DTOs.CardholderDTOs
{
    public class GetCardholderDTO
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }

        public List<GetCardDTO> Cards { get; set; } = new();
    }
}