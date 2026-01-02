using OfferService.Domain.Entities;

namespace OfferService.Domain.Interfaces;

public interface IOfferRepository
{
    Task<Offer?> GetByIdAsync(long offerId);
    Task<IEnumerable<Offer>> GetAllAsync(
        string? status = null,
        DateTime? createdAfter = null,
        DateTime? createdBefore = null,
        string? sortBy = null,
        bool sortDescending = false,
        int pageNumber = 1,
        int pageSize = 10);
    Task<int> GetTotalCountAsync(
        string? status = null,
        DateTime? createdAfter = null,
        DateTime? createdBefore = null);
    Task<Offer> AddAsync(Offer offer);
    Task<Offer> UpdateAsync(Offer offer);
    Task DeleteAsync(Guid offerId);
}