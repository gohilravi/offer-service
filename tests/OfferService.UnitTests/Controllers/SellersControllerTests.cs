using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OfferService.Api.Controllers;
using OfferService.Application.DTOs;
using OfferService.Application.Interfaces;

namespace OfferService.UnitTests.Controllers;

public class SellersControllerTests
{
    private readonly Mock<ISellerService> _mockSellerService;
    private readonly Mock<ILogger<SellersController>> _mockLogger;
    private readonly SellersController _controller;

    public SellersControllerTests()
    {
        _mockSellerService = new Mock<ISellerService>();
        _mockLogger = new Mock<ILogger<SellersController>>();
        _controller = new SellersController(_mockSellerService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetSellers_ValidParameters_ReturnsOkResult()
    {
        // Arrange
        var sellersDto = new PaginatedSellersDto
        {
            Sellers = new List<SellerDto>
            {
                new() { SellerId = Guid.NewGuid(), Name = "Test Seller", Email = "test@example.com" }
            },
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };

        _mockSellerService.Setup(s => s.GetAllSellersAsync(1, 10))
            .ReturnsAsync(sellersDto);

        // Act
        var result = await _controller.GetSellers(1, 10);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(sellersDto);
    }

    [Fact]
    public async Task GetSellers_InvalidPageNumber_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetSellers(0, 10);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetSellers_InvalidPageSize_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetSellers(1, 0);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetSellers_PageSizeExceedsLimit_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetSellers(1, 150);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetSellers_ServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        _mockSellerService.Setup(s => s.GetAllSellersAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _controller.GetSellers(1, 10);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);
    }
}