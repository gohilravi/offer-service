using OfferService.Application.DTOs;

namespace OfferService.Application.Interfaces;

public interface ISellerService
{
    Task<PaginatedSellersDto> GetAllSellersAsync(int pageNumber = 1, int pageSize = 10);
}