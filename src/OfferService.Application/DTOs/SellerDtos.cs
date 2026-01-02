namespace OfferService.Application.DTOs;

public class SellerDto
{
    public int SellerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
}

public class PaginatedSellersDto
{
    public IEnumerable<SellerDto> Sellers { get; set; } = new List<SellerDto>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}