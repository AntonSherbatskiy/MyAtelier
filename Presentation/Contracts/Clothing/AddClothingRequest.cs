using System.ComponentModel.DataAnnotations;

namespace Presentation.Contracts.Clothing;

public class AddClothingRequest
{
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }
}