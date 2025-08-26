namespace CardManagement.Shared.DTOs.TransactionDTOs
{
    public class GetTransactionDTO
    {
        public string MerchantName { get; set; } = null!;
        public string CardholderName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
