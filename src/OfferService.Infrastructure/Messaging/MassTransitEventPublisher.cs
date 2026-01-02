using MassTransit;
using OfferService.Application.Interfaces;

namespace OfferService.Infrastructure.Messaging;

public class MassTransitEventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync<T>(T eventMessage) where T : class
    {
        await _publishEndpoint.Publish(eventMessage);
    }
}