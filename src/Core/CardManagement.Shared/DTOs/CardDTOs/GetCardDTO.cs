namespace CardManagement.Shared.DTOs.CardDTOs
{
    public class GetCardDTO
    {
        public int CardId { get; set; }
        public string CardNumber { get; set; } = null!;
        public decimal Balance { get; set; }
    }
}
