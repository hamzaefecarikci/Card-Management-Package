using System.ComponentModel.DataAnnotations;

namespace CardManagement.Shared.DTOs.MerchantDTOs
{
    public class CreateMerchantDto
    {
        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
