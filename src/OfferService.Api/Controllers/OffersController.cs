using Microsoft.AspNetCore.Mvc;
using OfferService.Application.DTOs;
using OfferService.Application.Interfaces;
using OfferService.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace OfferService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OffersController : ControllerBase
{
    private readonly IOfferService _offerService;
    private readonly ILogger<OffersController> _logger;

    public OffersController(IOfferService offerService, ILogger<OffersController> logger)
    {
        _offerService = offerService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new offer
    /// </summary>
    /// <param name="createOfferDto">Offer creation data</param>
    /// <returns>Created offer</returns>
    [HttpPost]
    [ProducesResponseType(typeof(OfferDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<OfferDto>> CreateOffer([FromBody] CreateOfferDto createOfferDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Creating offer for seller {SellerId}", createOfferDto.SellerId);

            var result = await _offerService.CreateOfferAsync(createOfferDto);
            return CreatedAtAction(nameof(GetOfferById), new { id = result.OfferId }, result);
        }
        catch (SellerNotFoundException ex)
        {
            _logger.LogWarning(ex, "Seller not found when creating offer");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating offer");
            return StatusCode(500, "An error occurred while creating the offer");
        }
    }

    /// <summary>
    /// Get offer by ID
    /// </summary>
    /// <param name="id">Offer ID</param>
    /// <returns>Offer details</returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(OfferDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<OfferDto>> GetOfferById(long id)
    {
        try
        {
            _logger.LogInformation("Getting offer {OfferId}", id);

            var result = await _offerService.GetOfferByIdAsync(id);
            return Ok(result);
        }
        catch (OfferNotFoundException ex)
        {
            _logger.LogWarning(ex, "Offer not found");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting offer {OfferId}", id);
            return StatusCode(500, "An error occurred while retrieving the offer");
        }
    }

    /// <summary>
    /// Get all offers with filtering, sorting, and pagination
    /// </summary>
    /// <param name="status">Filter by status (offered, assigned, canceled)</param>
    /// <param name="createdAfter">Filter offers created after this date</param>
    /// <param name="createdBefore">Filter offers created before this date</param>
    /// <param name="sortBy">Sort field (createdAt, status, vehicleMake, vehicleModel)</param>
    /// <param name="sortDescending">Sort direction (default: false)</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <returns>Paginated list of offers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedOffersDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PaginatedOffersDto>> GetOffers(
        [FromQuery] string? status = null,
        [FromQuery] DateTime? createdAfter = null,
        [FromQuery] DateTime? createdBefore = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageNumber < 1)
                return BadRequest("Page number must be greater than 0");

            if (pageSize < 1 || pageSize > 100)
                return BadRequest("Page size must be between 1 and 100");

            _logger.LogInformation("Getting offers - Status: {Status}, Page: {PageNumber}, Size: {PageSize}", 
                status, pageNumber, pageSize);

            var result = await _offerService.GetOffersAsync(
                status, createdAfter, createdBefore, sortBy, sortDescending, pageNumber, pageSize);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting offers");
            return StatusCode(500, "An error occurred while retrieving offers");
        }
    }

    /// <summary>
    /// Update an existing offer
    /// </summary>
    /// <param name="id">Offer ID</param>
    /// <param name="updateOfferDto">Offer update data</param>
    /// <returns>Updated offer</returns>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(OfferDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<OfferDto>> UpdateOffer(long id, [FromBody] UpdateOfferDto updateOfferDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Updating offer {OfferId}", id);

            var result = await _offerService.UpdateOfferAsync(id, updateOfferDto);
            return Ok(result);
        }
        catch (OfferNotFoundException ex)
        {
            _logger.LogWarning(ex, "Offer not found when updating");
            return NotFound(ex.Message);
        }
        catch (OfferCannotBeUpdatedException ex)
        {
            _logger.LogWarning(ex, "Offer cannot be updated");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating offer {OfferId}", id);
            return StatusCode(500, "An error occurred while updating the offer");
        }
    }

    /// <summary>
    /// Assign an offer (creates purchase and transport, changes status to assigned)
    /// </summary>
    /// <param name="id">Offer ID</param>
    /// <returns>Assigned offer</returns>
    [HttpPost("{id:long}/assign")]
    [ProducesResponseType(typeof(OfferDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<OfferDto>> AssignOffer(long id)
    {
        try
        {
            _logger.LogInformation("Assigning offer {OfferId}", id);

            var result = await _offerService.AssignOfferAsync(id);
            return Ok(result);
        }
        catch (OfferNotFoundException ex)
        {
            _logger.LogWarning(ex, "Offer not found when assigning");
            return NotFound(ex.Message);
        }
        catch (InvalidOfferStateTransitionException ex)
        {
            _logger.LogWarning(ex, "Invalid state transition when assigning offer");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning offer {OfferId}", id);
            return StatusCode(500, "An error occurred while assigning the offer");
        }
    }

    /// <summary>
    /// Cancel an offer (changes status to canceled)
    /// </summary>
    /// <param name="id">Offer ID</param>
    /// <returns>Canceled offer</returns>
    [HttpPost("{id:long}/cancel")]
    [ProducesResponseType(typeof(OfferDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<OfferDto>> CancelOffer(long id)
    {
        try
        {
            _logger.LogInformation("Canceling offer {OfferId}", id);

            var result = await _offerService.CancelOfferAsync(id);
            return Ok(result);
        }
        catch (OfferNotFoundException ex)
        {
            _logger.LogWarning(ex, "Offer not found when canceling");
            return NotFound(ex.Message);
        }
        catch (InvalidOfferStateTransitionException ex)
        {
            _logger.LogWarning(ex, "Invalid state transition when canceling offer");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error canceling offer {OfferId}", id);
            return StatusCode(500, "An error occurred while canceling the offer");
        }
    }
}