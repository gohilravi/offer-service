using AutoMapper;
using FluentAssertions;
using Moq;
using OfferService.Application.DTOs;
using OfferService.Application.Events;
using OfferService.Application.Interfaces;
using OfferService.Application.Services;
using OfferService.Domain.Entities;
using OfferService.Domain.Exceptions;
using OfferService.Domain.Interfaces;

namespace OfferService.UnitTests.Services;

public class OfferServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IEventPublisher> _mockEventPublisher;
    private readonly Mock<IPurchaseApiService> _mockPurchaseApiService;
    private readonly Mock<ITransportApiService> _mockTransportApiService;
    private readonly Mock<ISellerRepository> _mockSellerRepository;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    private readonly OfferService.Application.Services.OfferService _offerService;

    public OfferServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockEventPublisher = new Mock<IEventPublisher>();
        _mockPurchaseApiService = new Mock<IPurchaseApiService>();
        _mockTransportApiService = new Mock<ITransportApiService>();
        _mockSellerRepository = new Mock<ISellerRepository>();
        _mockOfferRepository = new Mock<IOfferRepository>();

        _mockUnitOfWork.Setup(u => u.Sellers).Returns(_mockSellerRepository.Object);
        _mockUnitOfWork.Setup(u => u.Offers).Returns(_mockOfferRepository.Object);

        _offerService = new OfferService.Application.Services.OfferService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockEventPublisher.Object,
            _mockPurchaseApiService.Object,
            _mockTransportApiService.Object
        );
    }

    #region CreateOfferAsync Tests

    [Fact]
    public async Task CreateOfferAsync_ValidOffer_ReturnsOfferDto()
    {
        // Arrange
        var sellerId = 1;
        var createDto = new CreateOfferDto
        {
            SellerId = sellerId,
            Vin = "1HGCM82633A123456",
            VehicleYear = "2020",
            VehicleMake = "Toyota",
            VehicleModel = "Camry",
            VehicleZipCode = "12345"
        };

        var seller = new Seller { SellerId = sellerId, Name = "Test Seller" };
        var offer = new Offer { OfferId = 1L, SellerId = sellerId };
        var offerDto = new OfferDto { OfferId = offer.OfferId };
        var offerCreated = new OfferCreated { OfferId = offer.OfferId };

        _mockSellerRepository.Setup(r => r.GetByIdAsync(sellerId))
            .ReturnsAsync(seller);
        
        _mockMapper.Setup(m => m.Map<Offer>(createDto))
            .Returns(offer);
        
        _mockOfferRepository.Setup(r => r.AddAsync(offer))
            .ReturnsAsync(offer);
        
        _mockMapper.Setup(m => m.Map<OfferDto>(offer))
            .Returns(offerDto);
        
        _mockMapper.Setup(m => m.Map<OfferCreated>(offer))
            .Returns(offerCreated);

        // Act
        var result = await _offerService.CreateOfferAsync(createDto);

        // Assert
        result.Should().BeEquivalentTo(offerDto);
        _mockEventPublisher.Verify(p => p.PublishAsync(offerCreated), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateOfferAsync_SellerNotFound_ThrowsSellerNotFoundException()
    {
        // Arrange
        var sellerId = 1;
        var createDto = new CreateOfferDto { SellerId = sellerId };

        _mockSellerRepository.Setup(r => r.GetByIdAsync(sellerId))
            .ReturnsAsync((Seller?)null);

        // Act & Assert
        await _offerService.Invoking(s => s.CreateOfferAsync(createDto))
            .Should().ThrowAsync<SellerNotFoundException>();
    }

    #endregion

    #region GetOfferByIdAsync Tests

    [Fact]
    public async Task GetOfferByIdAsync_ExistingOffer_ReturnsOfferDto()
    {
        // Arrange
        var offerId = 1L;
        var offer = new Offer { OfferId = offerId };
        var offerDto = new OfferDto { OfferId = offerId };

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync(offer);
        
        _mockMapper.Setup(m => m.Map<OfferDto>(offer))
            .Returns(offerDto);

        // Act
        var result = await _offerService.GetOfferByIdAsync(offerId);

        // Assert
        result.Should().BeEquivalentTo(offerDto);
    }

    [Fact]
    public async Task GetOfferByIdAsync_NonExistingOffer_ThrowsOfferNotFoundException()
    {
        // Arrange
        var offerId = 1L;

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync((Offer?)null);

        // Act & Assert
        await _offerService.Invoking(s => s.GetOfferByIdAsync(offerId))
            .Should().ThrowAsync<OfferNotFoundException>();
    }

    #endregion

    #region UpdateOfferAsync Tests

    [Fact]
    public async Task UpdateOfferAsync_OfferedStatus_ReturnsUpdatedOffer()
    {
        // Arrange
        var offerId = 1L;
        var updateDto = new UpdateOfferDto { VehicleYear = "2021" };
        var offer = new Offer { OfferId = offerId, Status = "offered" };
        var offerDto = new OfferDto { OfferId = offerId };
        var offerUpdated = new OfferUpdated { OfferId = offerId };

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync(offer);
        
        _mockOfferRepository.Setup(r => r.UpdateAsync(offer))
            .ReturnsAsync(offer);
        
        _mockMapper.Setup(m => m.Map<OfferDto>(offer))
            .Returns(offerDto);
        
        _mockMapper.Setup(m => m.Map<OfferUpdated>(offer))
            .Returns(offerUpdated);

        // Act
        var result = await _offerService.UpdateOfferAsync(offerId, updateDto);

        // Assert
        result.Should().BeEquivalentTo(offerDto);
        _mockEventPublisher.Verify(p => p.PublishAsync(offerUpdated), Times.Once);
    }

    [Fact]
    public async Task UpdateOfferAsync_AssignedStatus_ThrowsOfferCannotBeUpdatedException()
    {
        // Arrange
        var offerId = 1L;
        var updateDto = new UpdateOfferDto();
        var offer = new Offer { OfferId = offerId, Status = "assigned" };

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync(offer);

        // Act & Assert
        await _offerService.Invoking(s => s.UpdateOfferAsync(offerId, updateDto))
            .Should().ThrowAsync<OfferCannotBeUpdatedException>();
    }

    #endregion

    #region AssignOfferAsync Tests

    [Fact]
    public async Task AssignOfferAsync_ValidTransition_ReturnsAssignedOffer()
    {
        // Arrange
        var offerId = 1L;
        var buyerId = 123;
        var carrierId = 456;
        var buyerZipCode = "67890";
        var sellerZipCode = "12345";
        var assignOfferDto = new AssignOfferDto { BuyerId = buyerId, CarrierId = carrierId, BuyerZipCode = buyerZipCode };
        var purchaseId = Guid.NewGuid();
        var transportId = Guid.NewGuid();
        var offer = new Offer { OfferId = offerId, Status = "offered", SellerId = 1, VehicleZipCode = sellerZipCode };
        var offerDto = new OfferDto { OfferId = offerId, BuyerId = buyerId, CarrierId = carrierId, BuyerZipCode = buyerZipCode };
        var offerAssigned = new OfferAssigned { OfferId = offerId };

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync(offer);
        
        _mockPurchaseApiService.Setup(s => s.CreatePurchaseAsync(offerId, buyerId))
            .ReturnsAsync(purchaseId);
        
        _mockTransportApiService.Setup(s => s.CreateTransportAsync(It.IsAny<CreateTransportDto>()))
            .ReturnsAsync(transportId);
        
        _mockOfferRepository.Setup(r => r.UpdateAsync(offer))
            .ReturnsAsync(offer);
        
        _mockMapper.Setup(m => m.Map<OfferDto>(offer))
            .Returns(offerDto);
        
        _mockMapper.Setup(m => m.Map<OfferAssigned>(offer))
            .Returns(offerAssigned);

        // Act
        var result = await _offerService.AssignOfferAsync(offerId, assignOfferDto);

        // Assert
        result.Should().BeEquivalentTo(offerDto);
        offer.Status.Should().Be("assigned");
        offer.PurchaseId.Should().Be(purchaseId);
        offer.TransportId.Should().Be(transportId);
        offer.BuyerId.Should().Be(buyerId);
        offer.CarrierId.Should().Be(carrierId);
        offer.BuyerZipCode.Should().Be(buyerZipCode);
        
        // Verify transport API was called with correct structure
        _mockTransportApiService.Verify(s => s.CreateTransportAsync(It.Is<CreateTransportDto>(dto =>
            dto.OfferId == offerId &&
            dto.PurchaseId == purchaseId &&
            dto.SellerId == offer.SellerId &&
            dto.BuyerId == buyerId &&
            dto.CarrierId == carrierId &&
            dto.SellerZipCode == sellerZipCode &&
            dto.BuyerZipCode == buyerZipCode &&
            dto.ScheduleWindow != null &&
            !string.IsNullOrEmpty(dto.ElasticSearchId)
        )), Times.Once);
        
        _mockEventPublisher.Verify(p => p.PublishAsync(offerAssigned), Times.Once);
    }

    [Fact]
    public async Task AssignOfferAsync_InvalidTransition_ThrowsInvalidOfferStateTransitionException()
    {
        // Arrange
        var offerId = 1L;
        var assignOfferDto = new AssignOfferDto { BuyerId = 123, CarrierId = 456, BuyerZipCode = "67890" };
        var offer = new Offer { OfferId = offerId, Status = "canceled" };

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync(offer);

        // Act & Assert
        await _offerService.Invoking(s => s.AssignOfferAsync(offerId, assignOfferDto))
            .Should().ThrowAsync<InvalidOfferStateTransitionException>();
    }

    [Fact]
    public async Task AssignOfferAsync_ValidTransition_CallsTransportApiWithCorrectStructure()
    {
        // Arrange
        var offerId = 1L;
        var buyerId = 123;
        var carrierId = 456;
        var buyerZipCode = "67890";
        var sellerZipCode = "12345";
        var sellerId = 100;
        var assignOfferDto = new AssignOfferDto { BuyerId = buyerId, CarrierId = carrierId, BuyerZipCode = buyerZipCode };
        var purchaseId = Guid.NewGuid();
        var transportId = Guid.NewGuid();
        var offer = new Offer 
        { 
            OfferId = offerId, 
            Status = "offered", 
            SellerId = sellerId,
            VehicleZipCode = sellerZipCode 
        };
        var offerDto = new OfferDto { OfferId = offerId };

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync(offer);
        
        _mockPurchaseApiService.Setup(s => s.CreatePurchaseAsync(offerId, buyerId))
            .ReturnsAsync(purchaseId);
        
        _mockTransportApiService.Setup(s => s.CreateTransportAsync(It.IsAny<CreateTransportDto>()))
            .ReturnsAsync(transportId);
        
        _mockOfferRepository.Setup(r => r.UpdateAsync(offer))
            .ReturnsAsync(offer);
        
        _mockMapper.Setup(m => m.Map<OfferDto>(offer))
            .Returns(offerDto);

        // Act
        await _offerService.AssignOfferAsync(offerId, assignOfferDto);

        // Assert
        _mockTransportApiService.Verify(s => s.CreateTransportAsync(It.Is<CreateTransportDto>(dto =>
            dto.OfferId == offerId &&
            dto.PurchaseId == purchaseId &&
            dto.SellerId == sellerId &&
            dto.BuyerId == buyerId &&
            dto.CarrierId == carrierId &&
            dto.SellerZipCode == sellerZipCode &&
            dto.BuyerZipCode == buyerZipCode &&
            dto.ScheduleWindow != null &&
            !string.IsNullOrEmpty(dto.ElasticSearchId)
        )), Times.Once);
    }

    #endregion

    #region CancelOfferAsync Tests

    [Fact]
    public async Task CancelOfferAsync_ValidTransition_ReturnsCanceledOffer()
    {
        // Arrange
        var offerId = 1L;
        var offer = new Offer { OfferId = offerId, Status = "offered" };
        var offerDto = new OfferDto { OfferId = offerId };
        var offerCanceled = new OfferCanceled { OfferId = offerId };

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync(offer);
        
        _mockOfferRepository.Setup(r => r.UpdateAsync(offer))
            .ReturnsAsync(offer);
        
        _mockMapper.Setup(m => m.Map<OfferDto>(offer))
            .Returns(offerDto);
        
        _mockMapper.Setup(m => m.Map<OfferCanceled>(offer))
            .Returns(offerCanceled);

        // Act
        var result = await _offerService.CancelOfferAsync(offerId);

        // Assert
        result.Should().BeEquivalentTo(offerDto);
        offer.Status.Should().Be("canceled");
        _mockEventPublisher.Verify(p => p.PublishAsync(offerCanceled), Times.Once);
    }

    [Fact]
    public async Task CancelOfferAsync_InvalidTransition_ThrowsInvalidOfferStateTransitionException()
    {
        // Arrange
        var offerId = 1L;
        var offer = new Offer { OfferId = offerId, Status = "canceled" };

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync(offer);

        // Act & Assert
        await _offerService.Invoking(s => s.CancelOfferAsync(offerId))
            .Should().ThrowAsync<InvalidOfferStateTransitionException>();
    }

    #endregion
}