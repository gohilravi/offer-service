namespace OfferService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ISellerRepository Sellers { get; }
    IOfferRepository Offers { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}