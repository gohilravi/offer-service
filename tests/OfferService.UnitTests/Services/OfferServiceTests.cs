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
        var sellerId = Guid.NewGuid();
        var createDto = new CreateOfferDto
        {
            SellerId = sellerId,
            SellerNetworkId = "NETWORK001",
            SellerName = "Test Seller"
        };

        var seller = new Seller { SellerId = sellerId, Name = "Test Seller" };
        var offer = new Offer { OfferId = Guid.NewGuid(), SellerId = sellerId };
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
        var sellerId = Guid.NewGuid();
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
        var offerId = Guid.NewGuid();
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
        var offerId = Guid.NewGuid();

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
        var offerId = Guid.NewGuid();
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
        var offerId = Guid.NewGuid();
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
        var offerId = Guid.NewGuid();
        var purchaseId = Guid.NewGuid();
        var transportId = Guid.NewGuid();
        var offer = new Offer { OfferId = offerId, Status = "offered" };
        var offerDto = new OfferDto { OfferId = offerId };
        var offerAssigned = new OfferAssigned { OfferId = offerId };

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync(offer);
        
        _mockPurchaseApiService.Setup(s => s.CreatePurchaseAsync(offerId))
            .ReturnsAsync(purchaseId);
        
        _mockTransportApiService.Setup(s => s.CreateTransportAsync(offerId))
            .ReturnsAsync(transportId);
        
        _mockOfferRepository.Setup(r => r.UpdateAsync(offer))
            .ReturnsAsync(offer);
        
        _mockMapper.Setup(m => m.Map<OfferDto>(offer))
            .Returns(offerDto);
        
        _mockMapper.Setup(m => m.Map<OfferAssigned>(offer))
            .Returns(offerAssigned);

        // Act
        var result = await _offerService.AssignOfferAsync(offerId);

        // Assert
        result.Should().BeEquivalentTo(offerDto);
        offer.Status.Should().Be("assigned");
        offer.PurchaseId.Should().Be(purchaseId);
        offer.TransportId.Should().Be(transportId);
        _mockEventPublisher.Verify(p => p.PublishAsync(offerAssigned), Times.Once);
    }

    [Fact]
    public async Task AssignOfferAsync_InvalidTransition_ThrowsInvalidOfferStateTransitionException()
    {
        // Arrange
        var offerId = Guid.NewGuid();
        var offer = new Offer { OfferId = offerId, Status = "canceled" };

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync(offer);

        // Act & Assert
        await _offerService.Invoking(s => s.AssignOfferAsync(offerId))
            .Should().ThrowAsync<InvalidOfferStateTransitionException>();
    }

    #endregion

    #region CancelOfferAsync Tests

    [Fact]
    public async Task CancelOfferAsync_ValidTransition_ReturnsCanceledOffer()
    {
        // Arrange
        var offerId = Guid.NewGuid();
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
        var offerId = Guid.NewGuid();
        var offer = new Offer { OfferId = offerId, Status = "canceled" };

        _mockOfferRepository.Setup(r => r.GetByIdAsync(offerId))
            .ReturnsAsync(offer);

        // Act & Assert
        await _offerService.Invoking(s => s.CancelOfferAsync(offerId))
            .Should().ThrowAsync<InvalidOfferStateTransitionException>();
    }

    #endregion
}