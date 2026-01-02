using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OfferService.Infrastructure.Data;

namespace OfferService.Infrastructure.Design;

public class OfferDbContextFactory : IDesignTimeDbContextFactory<OfferDbContext>
{
    public OfferDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OfferDbContext>();
        
        // Use a temporary connection string for migrations
        // In production, this will be configured from appsettings.json
        optionsBuilder.UseNpgsql("Host=localhost;Database=OfferServiceDb;Username=postgres;Password=postgres");
        
        return new OfferDbContext(optionsBuilder.Options);
    }
}