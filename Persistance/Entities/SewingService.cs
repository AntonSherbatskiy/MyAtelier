namespace MyAtelier.DAL.Entities;

public class SewingService : Identity
{
    public double Price { get; set; }
    public string Description { get; set; }
    public string ClothingSize { get; set; }
    public int EstimatedDays { get; set; }
    public double MaterialNeeded { get; set; }
    public Material Material { get; set; }
    public int MaterialId { get; set; }
    public Clothing Clothing { get; set; }
    public int ClothingId { get; set; }
}