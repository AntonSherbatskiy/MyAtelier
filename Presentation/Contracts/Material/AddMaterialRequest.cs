using System.ComponentModel.DataAnnotations;

namespace Presentation.Contracts.Material;

public class AddMaterialRequest
{
    [Required]
    [MaxLength(20)]
    public string MaterialName { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string MaterialTypeName { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity of added material cannot be 0 or negative")]
    public double Quantity { get; set; }
}