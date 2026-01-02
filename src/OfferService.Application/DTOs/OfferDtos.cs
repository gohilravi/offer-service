using System.ComponentModel.DataAnnotations;

namespace OfferService.Application.DTOs;

public class CreateOfferDto
{
    // Required fields
    [Required]
    public int SellerId { get; set; }
    
    // Vehicle Identification Number
    [MaxLength(17)]
    public string? Vin { get; set; }
    
    // Vehicle Information
    [Required]
    [MaxLength(50)]
    public string VehicleYear { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string VehicleMake { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string VehicleModel { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string VehicleTrim { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string VehicleBodyType { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string VehicleCabType { get; set; } = string.Empty;
    
    [Range(1, 10)]
    public int VehicleDoorCount { get; set; }
    
    [MaxLength(100)]
    public string VehicleFuelType { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string VehicleBodyStyle { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string VehicleUsage { get; set; } = string.Empty;
    
    // Location
    [Required]
    [MaxLength(20)]
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
}

public class UpdateOfferDto
{
    // Vehicle Identification Number (updateable)
    [MaxLength(17)]
    public string? Vin { get; set; }
    
    // Vehicle Information (updateable)
    public string? VehicleYear { get; set; }
    public string? VehicleMake { get; set; }
    public string? VehicleModel { get; set; }
    public string? VehicleTrim { get; set; }
    public string? VehicleBodyType { get; set; }
    public string? VehicleCabType { get; set; }
    public int? VehicleDoorCount { get; set; }
    public string? VehicleFuelType { get; set; }
    public string? VehicleBodyStyle { get; set; }
    public string? VehicleUsage { get; set; }
    
    // Location (updateable)
    public string? VehicleZipCode { get; set; }
    
    // Ownership (updateable)
    public string? OwnershipType { get; set; }
    public string? OwnershipTitleType { get; set; }
    
    // Condition (updateable)
    public int? Mileage { get; set; }
    public bool? IsMileageUnverifiable { get; set; }
    public string? DrivetrainCondition { get; set; }
    public string? KeyOrFobAvailable { get; set; }
    public string? WorkingBatteryInstalled { get; set; }
    public string? AllTiresInflated { get; set; }
    public string? WheelsRemoved { get; set; }
    public bool? WheelsRemovedDriverFront { get; set; }
    public bool? WheelsRemovedDriverRear { get; set; }
    public bool? WheelsRemovedPassengerFront { get; set; }
    public bool? WheelsRemovedPassengerRear { get; set; }
    public string? BodyPanelsIntact { get; set; }
    public string? BodyDamageFree { get; set; }
    public string? MirrorsLightsGlassIntact { get; set; }
    public string? InteriorIntact { get; set; }
    public string? FloodFireDamageFree { get; set; }
    public string? EngineTransmissionCondition { get; set; }
    public string? AirbagsDeployed { get; set; }
}

public class AssignOfferDto
{
    [Required]
    public int BuyerId { get; set; }
    
    [Required]
    public int CarrierId { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string BuyerZipCode { get; set; } = string.Empty;
}

public class OfferDto
{
    public long OfferId { get; set; }
    public int SellerId { get; set; }
    public string SellerNetworkId { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    
    // Vehicle Identification Number
    public string? Vin { get; set; }
    
    // Vehicle Information
    public string VehicleYear { get; set; } = string.Empty;
    public string VehicleMake { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public string VehicleTrim { get; set; } = string.Empty;
    public string VehicleBodyType { get; set; } = string.Empty;
    public string VehicleCabType { get; set; } = string.Empty;
    public int VehicleDoorCount { get; set; }
    public string VehicleFuelType { get; set; } = string.Empty;
    public string VehicleBodyStyle { get; set; } = string.Empty;
    public string VehicleUsage { get; set; } = string.Empty;
    
    // Location
    public string VehicleZipCode { get; set; } = string.Empty;
    public string? BuyerZipCode { get; set; }
    
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
    
    // Meta
    public string Status { get; set; } = string.Empty;
    public Guid? PurchaseId { get; set; }
    public Guid? TransportId { get; set; }
    public int? BuyerId { get; set; }
    public int? CarrierId { get; set; }
    public Guid NoSQLIndexId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
}

public class PaginatedOffersDto
{
    public IEnumerable<OfferDto> Offers { get; set; } = new List<OfferDto>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}