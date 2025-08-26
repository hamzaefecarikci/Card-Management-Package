namespace CardManagement.Shared.DTOs.CardholderDTOs
{
    public class CreateCardholderDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
