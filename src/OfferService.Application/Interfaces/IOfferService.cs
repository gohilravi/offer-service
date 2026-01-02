using OfferService.Application.DTOs;

namespace OfferService.Application.Interfaces;

public interface IOfferService
{
    Task<OfferDto> CreateOfferAsync(CreateOfferDto createOfferDto);
    Task<OfferDto?> GetOfferByIdAsync(long offerId);
    Task<PaginatedOffersDto> GetOffersAsync(
        string? status = null,
        DateTime? createdAfter = null,
        DateTime? createdBefore = null,
        string? sortBy = null,
        bool sortDescending = false,
        int pageNumber = 1,
        int pageSize = 10);
    Task<OfferDto> UpdateOfferAsync(long offerId, UpdateOfferDto updateOfferDto);
    Task<OfferDto> AssignOfferAsync(long offerId, AssignOfferDto assignOfferDto);
    Task<OfferDto> CancelOfferAsync(long offerId);
}

public interface IPurchaseApiService
{
    Task<Guid> CreatePurchaseAsync(long offerId, int buyerId);
}

public interface ITransportApiService
{
    Task<Guid> CreateTransportAsync(CreateTransportDto createTransportDto);
}