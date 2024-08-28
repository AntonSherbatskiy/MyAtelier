namespace Application.Models;

public class RepairingServiceModel : ServiceBaseModel
{
    public ClothingModel ClothingModel { get; set; }
    public int EstimatedDays { get; set; }
}