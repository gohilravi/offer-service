namespace OfferService.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException() { }
    
    public DomainException(string message) : base(message) { }
    
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class OfferNotFoundException : DomainException
{
    public OfferNotFoundException(long offerId) : base($"Offer with ID {offerId} was not found.") { }
}

public class SellerNotFoundException : DomainException
{
    public SellerNotFoundException(int sellerId) : base($"Seller with ID {sellerId} was not found.") { }
}

public class InvalidOfferStateTransitionException : DomainException
{
    public InvalidOfferStateTransitionException(string currentStatus, string newStatus) 
        : base($"Cannot transition offer from status '{currentStatus}' to '{newStatus}'.") { }
}

public class OfferCannotBeUpdatedException : DomainException
{
    public OfferCannotBeUpdatedException(string status) 
        : base($"Offers with status '{status}' cannot be updated.") { }
}