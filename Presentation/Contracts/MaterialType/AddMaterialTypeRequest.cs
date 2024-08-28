using System.ComponentModel.DataAnnotations;

namespace Presentation.Contracts.MaterialType;

public class AddMaterialTypeRequest
{
    [Required]
    [MaxLength(20)]
    public string MaterialTypeName { get; set; }
}