using System.ComponentModel.DataAnnotations;

namespace Application.Usecases.Commands.Services.Sewing;

public class AddSewingServiceCommand
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Price cannot be negative")]
    public double Price { get; set; }
    
    [Required]
    [MaxLength(20, ErrorMessage = "Max length for clothing name is {1}")]
    public string ClothingName { get; set; }
    
    [Required]
    [RegularExpression("S|M|L|XL", ErrorMessage = "Only 'S', 'M', 'L', 'XL' sizes are allowed")]
    public string ClothingSize { get; set; }
    
    [Required]
    public string MaterialName { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Material needed quantity cannot must be positive")]
    public double MaterialNeeded { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Estimated days must be positive")]
    public int EstimatedDays { get; set; }
}