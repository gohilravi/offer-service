namespace OfferService.Application.Events;

public abstract class BaseOfferEvent
{
    public long OfferId { get; set; }
    public int SellerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public long? PurchaseId { get; set; }
    public long? TransportId { get; set; }
    
    // Common vehicle summary fields for all events
    public string Vin { get; set; } = string.Empty;
    public string VehicleYear { get; set; } = string.Empty;
    public string VehicleMake { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public string VehicleTrim { get; set; } = string.Empty;
}

public class OfferCreated : BaseOfferEvent
{
    // Detailed Vehicle Information (only in OfferCreated)
    public string VehicleBodyType { get; set; } = string.Empty;
    public string VehicleCabType { get; set; } = string.Empty;
    public int VehicleDoorCount { get; set; }
    public string VehicleFuelType { get; set; } = string.Empty;
    public string VehicleBodyStyle { get; set; } = string.Empty;
    public string VehicleUsage { get; set; } = string.Empty;
    
    // Location
    public string VehicleZipCode { get; set; } = string.Empty;
    
    // Ownership
    public string OwnershipType { get; set; } = string.Empty;
    public string OwnershipTitleType { get; set; } = string.Empty;
    
    // Condition
    public int Mileage { get; set; }
    public bool IsMileageUnverifiable { get; set; }
    public string DrivetrainCondition { get; set; } = string.Empty;
    public string KeyOrFobAvailable { get; set; } = string.Empty;
    public string WorkingBatteryInstalled { get; set; } = string.Empty;
    public string AllTiresInflated { get; set; } = string.Empty;
    public string WheelsRemoved { get; set; } = string.Empty;
    public bool WheelsRemovedDriverFront { get; set; }
    public bool WheelsRemovedDriverRear { get; set; }
    public bool WheelsRemovedPassengerFront { get; set; }
    public bool WheelsRemovedPassengerRear { get; set; }
    public string BodyPanelsIntact { get; set; } = string.Empty;
    public string BodyDamageFree { get; set; } = string.Empty;
    public string MirrorsLightsGlassIntact { get; set; } = string.Empty;
    public string InteriorIntact { get; set; } = string.Empty;
    public string FloodFireDamageFree { get; set; } = string.Empty;
    public string EngineTransmissionCondition { get; set; } = string.Empty;
    public string AirbagsDeployed { get; set; } = string.Empty;
    
    // Seller Information
    public string SellerName { get; set; } = string.Empty;
    public string SellerNetworkId { get; set; } = string.Empty;
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