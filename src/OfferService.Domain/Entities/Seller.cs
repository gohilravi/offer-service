using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfferService.Domain.Entities;

public class Seller
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SellerId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastModifiedAt { get; set; }
    
    // Navigation property
    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}