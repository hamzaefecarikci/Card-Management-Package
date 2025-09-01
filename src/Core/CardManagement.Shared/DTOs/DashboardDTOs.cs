namespace CardManagement.Shared.DTOs;

public class DashboardResponse
{
    public CardholderStats Stats { get; set; } = new();
    public List<RecentTransaction> RecentTransactions { get; set; } = new();
    public List<CardBalance> CardBalances { get; set; } = new();
}

public class CardholderStats
{
    public int TotalCards { get; set; }
    public int ActiveCards { get; set; }
    public decimal TotalBalance { get; set; }
    public int TotalTransactions { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal TotalReceived { get; set; }
}

public class RecentTransaction
{
    public int TransactionId { get; set; }
    public string MerchantName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CardBalance
{
    public int CardId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime LastTransactionDate { get; set; }
} 