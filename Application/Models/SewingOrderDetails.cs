namespace Application.Models;

public class SewingOrderDetails
{
    public double MaterialNeeded { get; set; }
    public string ClothingSize { get; set; }
    public string ClothingName { get; set; }
    public string MaterialName { get; set; }
    public OrderModel OrderModel { get; set; }
}