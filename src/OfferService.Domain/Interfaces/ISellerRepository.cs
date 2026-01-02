using OfferService.Domain.Entities;

namespace OfferService.Domain.Interfaces;

public interface ISellerRepository
{
    Task<IEnumerable<Seller>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    Task<Seller?> GetByIdAsync(int sellerId);
    Task<Seller?> GetByEmailAsync(string email);
    Task<int> GetTotalCountAsync();
}