using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OfferService.Api.Controllers;
using OfferService.Application.DTOs;
using OfferService.Application.Interfaces;
using OfferService.Domain.Exceptions;

namespace OfferService.UnitTests.Controllers;

public class OffersControllerTests
{
    private readonly Mock<IOfferService> _mockOfferService;
    private readonly Mock<ILogger<OffersController>> _mockLogger;
    private readonly OffersController _controller;

    public OffersControllerTests()
    {
        _mockOfferService = new Mock<IOfferService>();
        _mockLogger = new Mock<ILogger<OffersController>>();
        _controller = new OffersController(_mockOfferService.Object, _mockLogger.Object);
    }

    #region CreateOffer Tests

    [Fact]
    public async Task CreateOffer_ValidOffer_ReturnsCreated()
    {
        // Arrange
        var createDto = new CreateOfferDto
        {
            SellerId = 1,
            Vin = "1HGCM82633A123456",
            VehicleYear = "2020",
            VehicleMake = "Toyota",
            VehicleModel = "Camry",
            VehicleZipCode = "12345"
        };

        var offerDto = new OfferDto
        {
            OfferId = 1,
            SellerId = createDto.SellerId,
            Status = "offered"
        };

        _mockOfferService.Setup(s => s.CreateOfferAsync(createDto))
            .ReturnsAsync(offerDto);

        // Act
        var result = await _controller.CreateOffer(createDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult!.Value.Should().BeEquivalentTo(offerDto);
    }

    [Fact]
    public async Task CreateOffer_SellerNotFound_ReturnsNotFound()
    {
        // Arrange
        var createDto = new CreateOfferDto { SellerId = 1 };
        
        _mockOfferService.Setup(s => s.CreateOfferAsync(createDto))
            .ThrowsAsync(new SellerNotFoundException(createDto.SellerId));

        // Act
        var result = await _controller.CreateOffer(createDto);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task CreateOffer_ServiceException_ReturnsInternalServerError()
    {
        // Arrange
        var createDto = new CreateOfferDto { SellerId = 1 };
        
        _mockOfferService.Setup(s => s.CreateOfferAsync(createDto))
            .ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _controller.CreateOffer(createDto);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);
    }

    #endregion

    #region GetOfferById Tests

    [Fact]
    public async Task GetOfferById_ExistingId_ReturnsOk()
    {
        // Arrange
        var offerId = 1L;
        var offerDto = new OfferDto { OfferId = offerId };

        _mockOfferService.Setup(s => s.GetOfferByIdAsync(offerId))
            .ReturnsAsync(offerDto);

        // Act
        var result = await _controller.GetOfferById(offerId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(offerDto);
    }

    [Fact]
    public async Task GetOfferById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var offerId = 1L;
        
        _mockOfferService.Setup(s => s.GetOfferByIdAsync(offerId))
            .ThrowsAsync(new OfferNotFoundException(offerId));

        // Act
        var result = await _controller.GetOfferById(offerId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    #endregion

    #region GetOffers Tests

    [Fact]
    public async Task GetOffers_ValidParameters_ReturnsOk()
    {
        // Arrange
        var paginatedOffers = new PaginatedOffersDto
        {
            Offers = new List<OfferDto> { new() { OfferId = 1L } },
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };

        _mockOfferService.Setup(s => s.GetOffersAsync(
            null, null, null, null, false, 1, 10))
            .ReturnsAsync(paginatedOffers);

        // Act
        var result = await _controller.GetOffers();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetOffers_InvalidPageNumber_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetOffers(pageNumber: 0);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    #endregion

    #region UpdateOffer Tests

    [Fact]
    public async Task UpdateOffer_ValidUpdate_ReturnsOk()
    {
        // Arrange
        var offerId = 1L;
        var updateDto = new UpdateOfferDto { VehicleYear = "2021" };
        var updatedOffer = new OfferDto { OfferId = offerId };

        _mockOfferService.Setup(s => s.UpdateOfferAsync(offerId, updateDto))
            .ReturnsAsync(updatedOffer);

        // Act
        var result = await _controller.UpdateOffer(offerId, updateDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task UpdateOffer_OfferNotFound_ReturnsNotFound()
    {
        // Arrange
        var offerId = 1L;
        var updateDto = new UpdateOfferDto();

        _mockOfferService.Setup(s => s.UpdateOfferAsync(offerId, updateDto))
            .ThrowsAsync(new OfferNotFoundException(offerId));

        // Act
        var result = await _controller.UpdateOffer(offerId, updateDto);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task UpdateOffer_CannotBeUpdated_ReturnsConflict()
    {
        // Arrange
        var offerId = 1L;
        var updateDto = new UpdateOfferDto();

        _mockOfferService.Setup(s => s.UpdateOfferAsync(offerId, updateDto))
            .ThrowsAsync(new OfferCannotBeUpdatedException("assigned"));

        // Act
        var result = await _controller.UpdateOffer(offerId, updateDto);

        // Assert
        result.Result.Should().BeOfType<ConflictObjectResult>();
    }

    #endregion

    #region AssignOffer Tests

    [Fact]
    public async Task AssignOffer_ValidOffer_ReturnsOk()
    {
        // Arrange
        var offerId = 1L;
        var assignOfferDto = new AssignOfferDto { BuyerId = 123, CarrierId = 456, BuyerZipCode = "67890" };
        var assignedOffer = new OfferDto { OfferId = offerId, Status = "assigned", BuyerId = 123, CarrierId = 456, BuyerZipCode = "67890" };

        _mockOfferService.Setup(s => s.AssignOfferAsync(offerId, assignOfferDto))
            .ReturnsAsync(assignedOffer);

        // Act
        var result = await _controller.AssignOffer(offerId, assignOfferDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var resultOffer = okResult!.Value as OfferDto;
        resultOffer!.Status.Should().Be("assigned");
        resultOffer.BuyerId.Should().Be(123);
        resultOffer.CarrierId.Should().Be(456);
        resultOffer.BuyerZipCode.Should().Be("67890");
    }

    [Fact]
    public async Task AssignOffer_InvalidStateTransition_ReturnsConflict()
    {
        // Arrange
        var offerId = 1L;
        var assignOfferDto = new AssignOfferDto { BuyerId = 123, CarrierId = 456, BuyerZipCode = "67890" };

        _mockOfferService.Setup(s => s.AssignOfferAsync(offerId, assignOfferDto))
            .ThrowsAsync(new InvalidOfferStateTransitionException("canceled", "assigned"));

        // Act
        var result = await _controller.AssignOffer(offerId, assignOfferDto);

        // Assert
        result.Result.Should().BeOfType<ConflictObjectResult>();
    }

    #endregion

    #region CancelOffer Tests

    [Fact]
    public async Task CancelOffer_ValidOffer_ReturnsOk()
    {
        // Arrange
        var offerId = 1L;
        var canceledOffer = new OfferDto { OfferId = offerId, Status = "canceled" };

        _mockOfferService.Setup(s => s.CancelOfferAsync(offerId))
            .ReturnsAsync(canceledOffer);

        // Act
        var result = await _controller.CancelOffer(offerId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var resultOffer = okResult!.Value as OfferDto;
        resultOffer!.Status.Should().Be("canceled");
    }

    [Fact]
    public async Task CancelOffer_AlreadyCanceled_ReturnsConflict()
    {
        // Arrange
        var offerId = 1L;

        _mockOfferService.Setup(s => s.CancelOfferAsync(offerId))
            .ThrowsAsync(new InvalidOfferStateTransitionException("canceled", "canceled"));

        // Act
        var result = await _controller.CancelOffer(offerId);

        // Assert
        result.Result.Should().BeOfType<ConflictObjectResult>();
    }

    #endregion
}