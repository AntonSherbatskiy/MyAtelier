namespace Application.Models;

public class SewingServiceModel : ServiceBaseModel
{
    public double MaterialNeeded { get; set; }
    public string ClothingSize { get; set; }
    public int EstimatedDays { get; set; }
    public ClothingModel ClothingModel { get; set; }
    public MaterialModel MaterialModel { get; set; }
}