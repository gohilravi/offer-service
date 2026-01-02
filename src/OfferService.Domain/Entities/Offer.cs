using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfferService.Domain.Entities;

public class Offer
{
    // Offer Identification
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long OfferId { get; set; }
    
    [Required]
    [ForeignKey(nameof(Seller))]
    public int SellerId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string SellerNetworkId { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string SellerName { get; set; } = string.Empty;
    
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
    
    [MaxLength(20)]
    public string? BuyerZipCode { get; set; }
    
    // Ownership
    [MaxLength(100)]
    public string OwnershipType { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string OwnershipTitleType { get; set; } = string.Empty;
    
    // Condition
    public int Mileage { get; set; }
    
    public bool IsMileageUnverifiable { get; set; }
    
    [MaxLength(100)]
    public string DrivetrainCondition { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string KeyOrFobAvailable { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string WorkingBatteryInstalled { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string AllTiresInflated { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string WheelsRemoved { get; set; } = string.Empty;
    
    public bool WheelsRemovedDriverFront { get; set; }
    
    public bool WheelsRemovedDriverRear { get; set; }
    
    public bool WheelsRemovedPassengerFront { get; set; }
    
    public bool WheelsRemovedPassengerRear { get; set; }
    
    [MaxLength(100)]
    public string BodyPanelsIntact { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string BodyDamageFree { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string MirrorsLightsGlassIntact { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string InteriorIntact { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string FloodFireDamageFree { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string EngineTransmissionCondition { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string AirbagsDeployed { get; set; } = string.Empty;
    
    // Offer Meta
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "offered";
    
    public long? PurchaseId { get; set; }
    
    public long? TransportId { get; set; }
    
    public int? BuyerId { get; set; }
    
    public int? CarrierId { get; set; }
    
    [Required]
    public Guid NoSQLIndexId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastModifiedAt { get; set; }
    
    // Navigation property
    public virtual Seller Seller { get; set; } = null!;
}