using AutoMapper;
using OfferService.Application.DTOs;
using OfferService.Application.Events;
using OfferService.Application.Interfaces;
using OfferService.Domain.Entities;
using OfferService.Domain.Enums;
using OfferService.Domain.Exceptions;
using OfferService.Domain.Interfaces;

namespace OfferService.Application.Services;

public class OfferService : IOfferService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;
    private readonly IPurchaseApiService _purchaseApiService;
    private readonly ITransportApiService _transportApiService;

    public OfferService(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IEventPublisher eventPublisher,
        IPurchaseApiService purchaseApiService,
        ITransportApiService transportApiService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
        _purchaseApiService = purchaseApiService;
        _transportApiService = transportApiService;
    }

    public async Task<OfferDto> CreateOfferAsync(CreateOfferDto createOfferDto)
    {
        // Validate seller exists
        var seller = await _unitOfWork.Sellers.GetByIdAsync(createOfferDto.SellerId);
        if (seller == null)
        {
            throw new SellerNotFoundException(createOfferDto.SellerId);
        }

        var offer = _mapper.Map<Offer>(createOfferDto);
        
        // Set seller information automatically
        offer.SellerNetworkId = seller.SellerId.ToString();
        offer.SellerName = seller.Name;
        
        // Set default status
        offer.Status = "offered";
        
        // Set timestamps
        offer.CreatedAt = DateTime.UtcNow;
        offer.LastModifiedAt = DateTime.UtcNow;
        
        offer = await _unitOfWork.Offers.AddAsync(offer);
        await _unitOfWork.SaveChangesAsync();

        // Publish event
        var offerCreated = _mapper.Map<OfferCreated>(offer);
        await _eventPublisher.PublishAsync(offerCreated);

        return _mapper.Map<OfferDto>(offer);
    }

    public async Task<OfferDto?> GetOfferByIdAsync(long offerId)
    {
        var offer = await _unitOfWork.Offers.GetByIdAsync(offerId);
        if (offer == null)
        {
            throw new OfferNotFoundException(offerId);
        }

        return _mapper.Map<OfferDto>(offer);
    }

    public async Task<PaginatedOffersDto> GetOffersAsync(
        string? status = null,
        DateTime? createdAfter = null,
        DateTime? createdBefore = null,
        string? sortBy = null,
        bool sortDescending = false,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var offers = await _unitOfWork.Offers.GetAllAsync(
            status, createdAfter, createdBefore, sortBy, sortDescending, pageNumber, pageSize);
        var totalCount = await _unitOfWork.Offers.GetTotalCountAsync(status, createdAfter, createdBefore);

        var offerDtos = _mapper.Map<IEnumerable<OfferDto>>(offers);

        return new PaginatedOffersDto
        {
            Offers = offerDtos,
            TotalCount = totalCount,
            Page = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<OfferDto> UpdateOfferAsync(long offerId, UpdateOfferDto updateOfferDto)
    {
        var offer = await _unitOfWork.Offers.GetByIdAsync(offerId);
        if (offer == null)
        {
            throw new OfferNotFoundException(offerId);
        }

        // Only allow updates when status is "offered"
        if (offer.Status != OfferStatus.Offered)
        {
            throw new OfferCannotBeUpdatedException(offer.Status);
        }

        // Map updates to existing offer
        _mapper.Map(updateOfferDto, offer);
        offer.LastModifiedAt = DateTime.UtcNow;
        offer = await _unitOfWork.Offers.UpdateAsync(offer);
        await _unitOfWork.SaveChangesAsync();

        // Publish event
        var offerUpdated = _mapper.Map<OfferUpdated>(offer);
        await _eventPublisher.PublishAsync(offerUpdated);

        return _mapper.Map<OfferDto>(offer);
    }

    public async Task<OfferDto> AssignOfferAsync(long offerId, AssignOfferDto assignOfferDto)
    {
        var offer = await _unitOfWork.Offers.GetByIdAsync(offerId);
        if (offer == null)
        {
            throw new OfferNotFoundException(offerId);
        }

        if (!OfferStatus.CanTransitionTo(offer.Status, OfferStatus.Assigned))
        {
            throw new InvalidOfferStateTransitionException(offer.Status, OfferStatus.Assigned);
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            // Call external APIs with BuyerId and CarrierId
            var purchaseId = await _purchaseApiService.CreatePurchaseAsync(offerId, assignOfferDto.BuyerId);
            
            // Create transport with detailed information
            var createTransportDto = new CreateTransportDto
            {
                OfferId = offerId,
                PurchaseId = purchaseId,
                SellerId = offer.SellerId,
                BuyerId = assignOfferDto.BuyerId,
                CarrierId = assignOfferDto.CarrierId,
                SellerZipCode = offer.VehicleZipCode,
                BuyerZipCode = "10054",
                ScheduleWindow = new ScheduleWindowDto
                {
                    StartDate = DateTime.UtcNow.AddDays(1).Date.AddHours(10), // Next day 10 AM
                    EndDate = DateTime.UtcNow.AddDays(1).Date.AddHours(18),   // Next day 6 PM
                    ScheduledDate = DateTime.UtcNow.AddDays(1).Date.AddHours(14) // Next day 2 PM
                }
            };
            
            var transportId = await _transportApiService.CreateTransportAsync(createTransportDto);

            // Update offer
            offer.Status = OfferStatus.Assigned;
            offer.PurchaseId = purchaseId;
            offer.TransportId = transportId;
            offer.BuyerId = assignOfferDto.BuyerId;
            offer.CarrierId = assignOfferDto.CarrierId;
            offer.LastModifiedAt = DateTime.UtcNow;

            offer = await _unitOfWork.Offers.UpdateAsync(offer);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            // Publish event
            var offerAssigned = _mapper.Map<OfferAssigned>(offer);
            await _eventPublisher.PublishAsync(offerAssigned);

            return _mapper.Map<OfferDto>(offer);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<OfferDto> CancelOfferAsync(long offerId)
    {
        var offer = await _unitOfWork.Offers.GetByIdAsync(offerId);
        if (offer == null)
        {
            throw new OfferNotFoundException(offerId);
        }

        if (!OfferStatus.CanTransitionTo(offer.Status, OfferStatus.Canceled))
        {
            throw new InvalidOfferStateTransitionException(offer.Status, OfferStatus.Canceled);
        }

        offer.Status = OfferStatus.Canceled;
        offer.LastModifiedAt = DateTime.UtcNow;

        offer = await _unitOfWork.Offers.UpdateAsync(offer);
        await _unitOfWork.SaveChangesAsync();

        // Publish event
        var offerCanceled = _mapper.Map<OfferCanceled>(offer);
        await _eventPublisher.PublishAsync(offerCanceled);

        return _mapper.Map<OfferDto>(offer);
    }
}