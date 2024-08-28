using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class MaterialModel
{
    [Required()]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50, ErrorMessage = "Max length of material name is {1}")]
    public string Name { get; set; }
    
    [Range(0, int.MaxValue, ErrorMessage = "Incorrect material quantity")]
    public int Quantity { get; set; }
    public List<SewingServiceModel>? SewingServiceModels { get; set; }
}