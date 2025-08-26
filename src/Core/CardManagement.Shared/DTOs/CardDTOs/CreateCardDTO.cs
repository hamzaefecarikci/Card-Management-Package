namespace CardManagement.Shared.DTOs.CardDTOs
{
    public class CreateCardDTO
    {
        public int CardholderId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
