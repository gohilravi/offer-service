using OfferService.Application.Events;

namespace OfferService.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T eventMessage) where T : class;
}