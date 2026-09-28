using Microsoft.EntityFrameworkCore;
using PriceGuard.Api.Data.Entities;

namespace PriceGuard.Api.Data;

public sealed class PriceGuardDbContext(
    DbContextOptions<PriceGuardDbContext> options)
    : DbContext(options)
{
    public DbSet<Store> Stores => Set<Store>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var store = modelBuilder.Entity<Store>();

        store.ToTable("Stores");

        store.HasKey(x => x.Id);

        store.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        store.Property(x => x.Domain)
            .HasMaxLength(300)
            .IsRequired();

        store.HasIndex(x => x.Domain)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}