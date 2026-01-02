namespace OfferService.Application.Events;

public abstract class BaseOfferEvent
{
    public long OfferId { get; set; }
    public int SellerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid? PurchaseId { get; set; }
    public Guid? TransportId { get; set; }
    
    // Vehicle Identification Number
    public string Vin { get; set; } = string.Empty;
    
    // Vehicle summary fields for events
    public string VehicleYear { get; set; } = string.Empty;
    public string VehicleMake { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public string VehicleTrim { get; set; } = string.Empty;
}

public class OfferCreated : BaseOfferEvent
{
}

public class OfferAssigned : BaseOfferEvent
{
}

public class OfferUpdated : BaseOfferEvent
{
}

public class OfferCanceled : BaseOfferEvent
{
}