namespace MyAtelier.DAL.Entities;

public class RepairingService : Identity
{
    public int EstimatedDays { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public Clothing Clothing { get; set; }
    public int ClothingId { get; set; }
}