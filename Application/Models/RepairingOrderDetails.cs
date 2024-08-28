namespace Application.Models;

public class RepairingOrderDetails
{
    public string ClothingName { get; set; }
    public int EstimatedDays { get; set; }
    public OrderModel OrderModel { get; set; }
}