using Cardholder.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cardholder.API.DbContexts;

public class CardHolderContext : DbContext
{
    public DbSet<CardHolder> CardHolders { get; set; }
    public DbSet<Card> Cards { get; set; }

    public CardHolderContext(DbContextOptions<CardHolderContext> options)
    : base(options)
    {
    }
}