using Microsoft.EntityFrameworkCore;
using OfferService.Domain.Entities;
using OfferService.Domain.Interfaces;
using OfferService.Infrastructure.Data;

namespace OfferService.Infrastructure.Repositories;

public class OfferRepository : IOfferRepository
{
    private readonly OfferDbContext _context;

    public OfferRepository(OfferDbContext context)
    {
        _context = context;
    }

    public async Task<Offer?> GetByIdAsync(long offerId)
    {
        return await _context.Offers
            .Include(o => o.Seller)
            .FirstOrDefaultAsync(o => o.OfferId == offerId);
    }

    public async Task<IEnumerable<Offer>> GetAllAsync(
        string? status = null,
        DateTime? createdAfter = null,
        DateTime? createdBefore = null,
        string? sortBy = null,
        bool sortDescending = false,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = _context.Offers.Include(o => o.Seller).AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(o => o.Status == status);
        }

        if (createdAfter.HasValue)
        {
            query = query.Where(o => o.CreatedAt >= createdAfter.Value);
        }

        if (createdBefore.HasValue)
        {
            query = query.Where(o => o.CreatedAt <= createdBefore.Value);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "createdat" => sortDescending
                    ? query.OrderByDescending(o => o.CreatedAt)
                    : query.OrderBy(o => o.CreatedAt),
                "status" => sortDescending
                    ? query.OrderByDescending(o => o.Status)
                    : query.OrderBy(o => o.Status),
                "vehiclemake" => sortDescending
                    ? query.OrderByDescending(o => o.VehicleMake)
                    : query.OrderBy(o => o.VehicleMake),
                "vehiclemodel" => sortDescending
                    ? query.OrderByDescending(o => o.VehicleModel)
                    : query.OrderBy(o => o.VehicleModel),
                _ => query.OrderByDescending(o => o.CreatedAt)
            };
        }
        else
        {
            query = query.OrderByDescending(o => o.CreatedAt);
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(
        string? status = null,
        DateTime? createdAfter = null,
        DateTime? createdBefore = null)
    {
        var query = _context.Offers.AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(o => o.Status == status);
        }

        if (createdAfter.HasValue)
        {
            query = query.Where(o => o.CreatedAt >= createdAfter.Value);
        }

        if (createdBefore.HasValue)
        {
            query = query.Where(o => o.CreatedAt <= createdBefore.Value);
        }

        return await query.CountAsync();
    }

    public async Task<Offer> AddAsync(Offer offer)
    {
        await _context.Offers.AddAsync(offer);
        return offer;
    }

    public Task<Offer> UpdateAsync(Offer offer)
    {
        _context.Offers.Update(offer);
        return Task.FromResult(offer);
    }

    public async Task DeleteAsync(Guid offerId)
    {
        var offer = await _context.Offers.FindAsync(offerId);
        if (offer != null)
        {
            _context.Offers.Remove(offer);
        }
    }
}