namespace Application.Models;

public class OrderModel
{
    public int Id { get; set; }
    public DateTime PlacedAt { get; set; }
    public string Status { get; set; }
    public bool IsClothesBrought { get; set; }
    public string ServiceType { get; set; }
    public string ClothingName { get; set; }
    public string? ClothingSize { get; set; }
    public string? MaterialName { get; set; }
    public string AdditionalInfo { get; set; }
    public double Price { get; set; }
    public int ServiceAggregatorId { get; set; }
    public DateTime? CompletedAt { get; set; }
}