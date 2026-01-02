using Microsoft.Extensions.Logging;
using OfferService.Application.Interfaces;

namespace OfferService.Infrastructure.ExternalServices;

public class PurchaseApiService : IPurchaseApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PurchaseApiService> _logger;

    public PurchaseApiService(HttpClient httpClient, ILogger<PurchaseApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Guid> CreatePurchaseAsync(long offerId)
    {
        try
        {
            // Mock implementation - In real scenario, this would call an external API
            _logger.LogInformation("Creating purchase for offer {OfferId}", offerId);
            
            // Simulate API call delay
            await Task.Delay(100);
            
            // Return a mock purchase ID
            var purchaseId = Guid.NewGuid();
            _logger.LogInformation("Created purchase {PurchaseId} for offer {OfferId}", purchaseId, offerId);
            
            return purchaseId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create purchase for offer {OfferId}", offerId);
            throw;
        }
    }
}

public class TransportApiService : ITransportApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TransportApiService> _logger;

    public TransportApiService(HttpClient httpClient, ILogger<TransportApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Guid> CreateTransportAsync(long offerId)
    {
        try
        {
            // Mock implementation - In real scenario, this would call an external API
            _logger.LogInformation("Creating transport for offer {OfferId}", offerId);
            
            // Simulate API call delay
            await Task.Delay(100);
            
            // Return a mock transport ID
            var transportId = Guid.NewGuid();
            _logger.LogInformation("Created transport {TransportId} for offer {OfferId}", transportId, offerId);
            
            return transportId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create transport for offer {OfferId}", offerId);
            throw;
        }
    }
}