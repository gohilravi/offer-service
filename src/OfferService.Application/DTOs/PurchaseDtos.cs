using System.ComponentModel.DataAnnotations;

namespace OfferService.Application.DTOs;

public class CreatePurchaseRequestDto
{
    [Required]
    public long OfferId { get; set; }
    
    [Required]
    public int BuyerId { get; set; }
}

public class CreatePurchaseResponseDto
{
    public Guid PurchaseId { get; set; }
    public long OfferId { get; set; }
    public int BuyerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}