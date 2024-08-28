namespace Application.Models;

public class PlaceSewingOrderModel
{
    public int Id { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string? AdditionalInfo { get; set; }
    public string ServiceName { get; set; }
    public int EstimatedDays { get; set; }
    public string Size { get; set; }
    public MaterialModel MaterialModel { get; set; }
    public ClothingModel ClothingModel { get; set; }
}