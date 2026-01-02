using AutoMapper;
using OfferService.Application.DTOs;
using OfferService.Application.Interfaces;
using OfferService.Domain.Interfaces;

namespace OfferService.Application.Services;

public class SellerService : ISellerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SellerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedSellersDto> GetAllSellersAsync(int pageNumber = 1, int pageSize = 10)
    {
        var sellers = await _unitOfWork.Sellers.GetAllAsync(pageNumber, pageSize);
        var totalCount = await _unitOfWork.Sellers.GetTotalCountAsync();
        
        var sellerDtos = _mapper.Map<IEnumerable<SellerDto>>(sellers);
        
        return new PaginatedSellersDto
        {
            Sellers = sellerDtos,
            TotalCount = totalCount,
            Page = pageNumber,
            PageSize = pageSize
        };
    }
}