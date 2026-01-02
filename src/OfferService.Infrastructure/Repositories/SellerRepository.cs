using Microsoft.EntityFrameworkCore;
using OfferService.Domain.Entities;
using OfferService.Domain.Interfaces;
using OfferService.Infrastructure.Data;

namespace OfferService.Infrastructure.Repositories;

public class SellerRepository : ISellerRepository
{
    private readonly OfferDbContext _context;

    public SellerRepository(OfferDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Seller>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        return await _context.Sellers
            .OrderBy(s => s.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Seller?> GetByIdAsync(int sellerId)
    {
        return await _context.Sellers.FindAsync(sellerId);
    }

    public async Task<Seller?> GetByEmailAsync(string email)
    {
        return await _context.Sellers
            .FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Sellers.CountAsync();
    }
}