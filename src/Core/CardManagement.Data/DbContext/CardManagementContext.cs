using Microsoft.EntityFrameworkCore;
using CardManagement.Data.Entities;

namespace CardManagement.Data;

public class CardManagementContext : DbContext
{
    public CardManagementContext(DbContextOptions<CardManagementContext> options) : base(options)
    {
    }

    public DbSet<CardholderEntity> Cardholders { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<QRCode> QRCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // CardholderEntity configuration
        modelBuilder.Entity<CardholderEntity>(entity =>
        {
            entity.HasKey(e => e.CardholderId);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Card configuration
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.CardId);
            entity.Property(e => e.CardNumber).IsRequired().HasMaxLength(16);
            entity.Property(e => e.Pin).IsRequired().HasMaxLength(4);
            entity.Property(e => e.Balance).HasColumnType("decimal(10,2)");
            entity.HasIndex(e => e.CardNumber).IsUnique();
        });

        // Merchant configuration
        modelBuilder.Entity<Merchant>(entity =>
        {
            entity.HasKey(e => e.MerchantId);
            entity.Property(e => e.MerchantName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Transaction configuration
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId);
            entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
        });

        // QRCode configuration
        modelBuilder.Entity<QRCode>(entity =>
        {
            entity.HasKey(e => e.QRCodeId);
            entity.Property(e => e.QRCodeIdString).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.QRCodeIdString).IsUnique();
        });

        // Relationships
        modelBuilder.Entity<Card>()
            .HasOne(c => c.Cardholder)
            .WithMany(ch => ch.Cards)
            .HasForeignKey(c => c.CardholderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Card)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Merchant)
            .WithMany(m => m.Transactions)
            .HasForeignKey(t => t.MerchantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QRCode>()
            .HasOne(q => q.Merchant)
            .WithMany(m => m.QRCodes)
            .HasForeignKey(q => q.MerchantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QRCode>()
            .HasOne(q => q.Transaction)
            .WithMany()
            .HasForeignKey(q => q.TransactionId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<QRCode>()
            .HasOne(q => q.Card)
            .WithMany()
            .HasForeignKey(q => q.CardId)
            .OnDelete(DeleteBehavior.NoAction);
    }
} 