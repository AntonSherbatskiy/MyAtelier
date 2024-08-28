using System.ComponentModel.DataAnnotations;

namespace Presentation.Contracts.Services.Repairing;

public class AddRepairingServiceRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Price of service cannot be 0 or negative")]
    public double Price { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [MaxLength(200)]
    public string? AdditionalInfo { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string ClothingName { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Estimated days cannot be 0 or negative")]
    public int EstimatedDays { get; set; }
}