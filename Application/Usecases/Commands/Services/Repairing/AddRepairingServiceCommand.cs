using System.ComponentModel.DataAnnotations;

namespace Application.Usecases.Commands.Services.Repairing;

public class AddRepairingServiceCommand
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Incorrect price")]
    public double Price { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string? Description { get; set; }
    public string? AdditionalInfo { get; set; }
    
    [Required]
    public string ClothingName { get; set; }
    
    [Range(0, int.MaxValue, ErrorMessage = "Incorrect estimated days")]
    public int EstimatedDays { get; set; }
}