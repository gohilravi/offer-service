using Microsoft.AspNetCore.Mvc;
using OfferService.Application.DTOs;
using OfferService.Application.Interfaces;

namespace OfferService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SellersController : ControllerBase
{
    private readonly ISellerService _sellerService;
    private readonly ILogger<SellersController> _logger;

    public SellersController(ISellerService sellerService, ILogger<SellersController> logger)
    {
        _sellerService = sellerService;
        _logger = logger;
    }

    /// <summary>
    /// Get all sellers with optional pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <returns>Paginated list of sellers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedSellersDto), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PaginatedSellersDto>> GetSellers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageNumber < 1)
                return BadRequest("Page number must be greater than 0");

            if (pageSize < 1 || pageSize > 100)
                return BadRequest("Page size must be between 1 and 100");

            _logger.LogInformation("Getting sellers - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

            var result = await _sellerService.GetAllSellersAsync(pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sellers");
            return StatusCode(500, "An error occurred while retrieving sellers");
        }
    }
}