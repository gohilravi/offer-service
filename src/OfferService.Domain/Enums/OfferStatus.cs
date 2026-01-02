namespace OfferService.Domain.Enums;

public static class OfferStatus
{
    public const string Offered = "offered";
    public const string Assigned = "assigned";
    public const string Canceled = "canceled";
    
    public static readonly string[] ValidStatuses = { Offered, Assigned, Canceled };
    
    public static bool IsValid(string status)
    {
        return ValidStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
    }
    
    public static bool CanTransitionTo(string currentStatus, string newStatus)
    {
        return currentStatus.ToLower() switch
        {
            "offered" => newStatus.ToLower() is "assigned" or "canceled",
            "assigned" => newStatus.ToLower() is "canceled",
            "canceled" => false,
            _ => false
        };
    }
}