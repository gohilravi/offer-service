using System.ComponentModel.DataAnnotations;

namespace OfferService.Application.DTOs;

public class ScheduleWindowDto
{
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    public DateTime ScheduledDate { get; set; }
}

public class CreateTransportDto
{
    [Required]
    public long OfferId { get; set; }
    
    [Required]
    public Guid PurchaseId { get; set; }
    
    [Required]
    public int SellerId { get; set; }
    
    [Required]
    public int BuyerId { get; set; }
    
    [Required]
    public int CarrierId { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string SellerZipCode { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string BuyerZipCode { get; set; } = string.Empty;
    
    [Required]
    public ScheduleWindowDto ScheduleWindow { get; set; } = new();
    
    [Required]
    [MaxLength(100)]
    public string ElasticSearchId { get; set; } = string.Empty;
}

public class CreateTransportResponseDto
{
    public Guid TransportId { get; set; }
    public long OfferId { get; set; }
    public Guid PurchaseId { get; set; }
    public int CarrierId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}