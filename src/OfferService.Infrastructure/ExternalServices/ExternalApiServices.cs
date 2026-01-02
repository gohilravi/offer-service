using Microsoft.Extensions.Logging;
using OfferService.Application.Interfaces;
using OfferService.Application.DTOs;
using System.Text;
using System.Text.Json;

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

    public async Task<Guid> CreatePurchaseAsync(long offerId, int buyerId)
    {
        try
        {
            _logger.LogInformation("Creating purchase for offer {OfferId} with buyer {BuyerId}", offerId, buyerId);
            
            var request = new CreatePurchaseRequestDto
            {
                OfferId = offerId,
                BuyerId = buyerId
            };
            
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("http://localhost:5000/purchases", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var purchaseResponse = JsonSerializer.Deserialize<CreatePurchaseResponseDto>(responseContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                var purchaseId = purchaseResponse?.PurchaseId ?? Guid.NewGuid();
                _logger.LogInformation("Created purchase {PurchaseId} for offer {OfferId}", purchaseId, offerId);
                
                return purchaseId;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to create purchase for offer {OfferId}. Status: {StatusCode}, Response: {Response}", 
                    offerId, response.StatusCode, errorContent);
                
                throw new HttpRequestException($"Purchase API returned {response.StatusCode}: {errorContent}");
            }
        }
        catch (HttpRequestException)
        {
            throw;
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

    public async Task<Guid> CreateTransportAsync(CreateTransportDto createTransportDto)
    {
        try
        {
            _logger.LogInformation("Creating transport for offer {OfferId} with carrier {CarrierId}, buyer {BuyerId}", 
                createTransportDto.OfferId, createTransportDto.CarrierId, createTransportDto.BuyerId);
            
            var json = JsonSerializer.Serialize(createTransportDto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("http://localhost:5002/api/transports", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var transportResponse = JsonSerializer.Deserialize<CreateTransportResponseDto>(responseContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                var transportId = transportResponse?.TransportId ?? Guid.NewGuid();
                _logger.LogInformation("Created transport {TransportId} for offer {OfferId} with purchase {PurchaseId}", 
                    transportId, createTransportDto.OfferId, createTransportDto.PurchaseId);
                
                return transportId;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to create transport for offer {OfferId}. Status: {StatusCode}, Response: {Response}", 
                    createTransportDto.OfferId, response.StatusCode, errorContent);
                
                throw new HttpRequestException($"Transport API returned {response.StatusCode}: {errorContent}");
            }
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create transport for offer {OfferId}", createTransportDto.OfferId);
            throw;
        }
    }
}