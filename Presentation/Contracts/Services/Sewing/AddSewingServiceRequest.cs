using System.ComponentModel.DataAnnotations;

namespace Presentation.Contracts.Services.Sewing;

public class AddSewingServiceRequest
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Price cannot be negative")]
    public double Price { get; set; }
    
    [MaxLength(500)]
    [Required]
    public string Description { get; set; }
    
    [MaxLength(200)]
    public string? AdditionalInfo { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Negative values are not allowed")]
    public double MaterialNeeded { get; set; }
    
    [Required(ErrorMessage = "Name of material is required")]
    public string MaterialName { get; set; }
    
    [Required(ErrorMessage = "Name of clothing is required")]
    public string ClothingName { get; set; }
    
    [Required]
    [RegularExpression("S|M|L|XL", ErrorMessage = "Allowed sizes: 'S', 'M', 'L', 'XL'")]
    public string Size { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Estimated days cannot be negative or 0")]
    public int EstimatedDays { get; set; }
}