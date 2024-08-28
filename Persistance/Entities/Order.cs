namespace MyAtelier.DAL.Entities;

public class Order : Identity
{
    public DateTime PlacedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Status { get; set; }
    public string? AdditionalInformation { get; set; }
    public bool? IsClothesBrought { get; set; }
    public string ServiceType { get; set; }
    public string ClothingName { get; set; }
    public string? ClothingSize { get; set; }
    public string? MaterialName { get; set; }
    public ApplicationUser User { get; set; }
    public string UserId { get; set; }
    public double Price { get; set; }
    public ServiceAggregator? ServiceAggregator { get; set; }
    public int? ServiceAggregatorId { get; set; }
}