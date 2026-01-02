using Microsoft.EntityFrameworkCore;
using OfferService.Domain.Entities;

namespace OfferService.Infrastructure.Data;

public class OfferDbContext : DbContext
{
    public OfferDbContext(DbContextOptions<OfferDbContext> options) : base(options)
    {
    }

    public DbSet<Seller> Sellers { get; set; } = null!;
    public DbSet<Offer> Offers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seller configuration
        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.SellerId);
            
            // Configure SellerId as identity column
            entity.Property(e => e.SellerId)
                .ValueGeneratedOnAdd();
            
            entity.HasIndex(e => e.Email).IsUnique();
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
                
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
                
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
            entity.Property(e => e.LastModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Offer configuration
        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.OfferId);
            
            // Configure OfferId as identity column
            entity.Property(e => e.OfferId)
                .ValueGeneratedOnAdd();
            
            entity.HasOne(e => e.Seller)
                .WithMany(s => s.Offers)
                .HasForeignKey(e => e.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.Property(e => e.SellerNetworkId)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.SellerName)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Vin)
                .IsRequired()
                .HasMaxLength(17);
                
            // Vehicle properties
            entity.Property(e => e.VehicleYear)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.VehicleMake)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.VehicleModel)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.VehicleTrim)
                .HasMaxLength(100);
                
            entity.Property(e => e.VehicleBodyType)
                .HasMaxLength(100);
                
            entity.Property(e => e.VehicleCabType)
                .HasMaxLength(100);
                
            entity.Property(e => e.VehicleFuelType)
                .HasMaxLength(100);
                
            entity.Property(e => e.VehicleBodyStyle)
                .HasMaxLength(100);
                
            entity.Property(e => e.VehicleUsage)
                .HasMaxLength(100);
                
            entity.Property(e => e.VehicleZipCode)
                .IsRequired()
                .HasMaxLength(20);
                
            // Ownership properties
            entity.Property(e => e.OwnershipType)
                .HasMaxLength(100);
                
            entity.Property(e => e.OwnershipTitleType)
                .HasMaxLength(100);
                
            // Condition properties
            entity.Property(e => e.DrivetrainCondition)
                .HasMaxLength(100);
                
            entity.Property(e => e.KeyOrFobAvailable)
                .HasMaxLength(100);
                
            entity.Property(e => e.WorkingBatteryInstalled)
                .HasMaxLength(100);
                
            entity.Property(e => e.AllTiresInflated)
                .HasMaxLength(100);
                
            entity.Property(e => e.WheelsRemoved)
                .HasMaxLength(100);
                
            entity.Property(e => e.BodyPanelsIntact)
                .HasMaxLength(100);
                
            entity.Property(e => e.BodyDamageFree)
                .HasMaxLength(100);
                
            entity.Property(e => e.MirrorsLightsGlassIntact)
                .HasMaxLength(100);
                
            entity.Property(e => e.InteriorIntact)
                .HasMaxLength(100);
                
            entity.Property(e => e.FloodFireDamageFree)
                .HasMaxLength(100);
                
            entity.Property(e => e.EngineTransmissionCondition)
                .HasMaxLength(100);
                
            entity.Property(e => e.AirbagsDeployed)
                .HasMaxLength(100);
                
            // Status and meta properties
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("offered");
                
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
            entity.Property(e => e.LastModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Indexes for better query performance
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => new { e.SellerId, e.Status });
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var sellers = new[]
        {
            new Seller
            {
                SellerId = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            },
            new Seller
            {
                SellerId = 2,
                Name = "Jane Smith",
                Email = "jane.smith@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            },
            new Seller
            {
                SellerId = 3,
                Name = "Bob Johnson",
                Email = "bob.johnson@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            },
            new Seller
            {
                SellerId = 4,
                Name = "Alice Williams",
                Email = "alice.williams@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            },
            new Seller
            {
                SellerId = 5,
                Name = "Charlie Brown",
                Email = "charlie.brown@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            },
            new Seller
            {
                SellerId = 6,
                Name = "Diana Davis",
                Email = "diana.davis@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            },
            new Seller
            {
                SellerId = 7,
                Name = "Edward Miller",
                Email = "edward.miller@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            },
            new Seller
            {
                SellerId = 8,
                Name = "Fiona Wilson",
                Email = "fiona.wilson@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            },
            new Seller
            {
                SellerId = 9,
                Name = "George Moore",
                Email = "george.moore@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            },
            new Seller
            {
                SellerId = 10,
                Name = "Helen Taylor",
                Email = "helen.taylor@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            }
        };

        modelBuilder.Entity<Seller>().HasData(sellers);
    }
}