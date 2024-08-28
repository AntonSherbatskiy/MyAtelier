using System.ComponentModel.DataAnnotations;

namespace Presentation.Contracts.Orders;

public class PlaceOrderRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Identifier of service cannot be 0 or negative")]
    public int ServiceId { get; set; }
    
    [Required]
    [RegularExpression("Sewing|Repairing", ErrorMessage = "Possible service categories: 'Sewing', 'Repairing'")]
    public string ServiceCategory { get; set; }
}