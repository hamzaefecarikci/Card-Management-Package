using CardManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardManagement.Data.Db
{
    public class CardManagementContext : DbContext
    {
        public CardManagementContext(DbContextOptions<CardManagementContext> options): base(options)
        {
        }

        public DbSet<Cardholder> Cardholders { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;
        public DbSet<Merchant> Merchants { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
    }
}
