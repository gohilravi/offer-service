using System.ComponentModel.DataAnnotations;

namespace OfferService.Application.DTOs;

public class CreatePurchaseRequestDto
{
    [Required]
    public long OfferId { get; set; }

    [Required]
    public int BuyerId { get; set; }
    [Required]
    public string ElasticSearchId { get; set; }
}

public class CreatePurchaseResponseDto
{
    public long PurchaseId { get; set; }
    public long OfferId { get; set; }
    public int BuyerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class PurchaseApiResponseDto
{
    public bool Success { get; set; }
    public PurchaseDataDto Data { get; set; } = new();
    public string Message { get; set; } = string.Empty;
    public string[]? Errors { get; set; }
}

public class PurchaseDataDto
{
    public long PurchaseId { get; set; }
}