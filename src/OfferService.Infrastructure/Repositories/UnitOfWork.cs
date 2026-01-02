using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OfferService.Domain.Interfaces;
using OfferService.Infrastructure.Data;

namespace OfferService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly OfferDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(OfferDbContext context)
    {
        _context = context;
        Sellers = new SellerRepository(_context);
        Offers = new OfferRepository(_context);
    }

    public ISellerRepository Sellers { get; }
    public IOfferRepository Offers { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}