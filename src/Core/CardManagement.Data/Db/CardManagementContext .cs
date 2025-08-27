using CardManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardManagement.Data.Db
{
    public class CardManagementContext : DbContext
    {
        public CardManagementContext(DbContextOptions<CardManagementContext> options) : base(options)
        {
        }

        public DbSet<Cardholder> Cardholders { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;
        public DbSet<Merchant> Merchants { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<TransactionProductDetail> TransactionProductDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TransactionProductDetail>()
                .HasOne(tp => tp.Transaction)
                .WithMany()
                .HasForeignKey(tp => tp.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionProductDetail>()
                .HasOne(tp => tp.Product)
                .WithMany()
                .HasForeignKey(tp => tp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
