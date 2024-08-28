namespace Application.Models;

public class PlaceRepairingOrderModel
{
    public int Id { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string? AdditionalInfo { get; set; }
    public string ServiceName { get; set; }
    public int EstimatedDays { get; set; }
    public ClothingModel ClothingModel { get; set; }
}