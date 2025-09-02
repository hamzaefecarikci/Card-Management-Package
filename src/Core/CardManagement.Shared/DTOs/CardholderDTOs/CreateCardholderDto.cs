using System.ComponentModel.DataAnnotations;

namespace CardManagement.Shared.DTOs.CardholderDTOs
{
    public class CreateCardholderDto
    {
        public required string FullName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
