using System.ComponentModel.DataAnnotations;

namespace Presentation.Contracts.Orders;

public class UpdateOrderStatusRequest
{
    [Required]
    [RegularExpression("Completed|Canceled", ErrorMessage = "Possible order statuses: 'Completed', 'Canceled'")]
    public string Status { get; set; }
}