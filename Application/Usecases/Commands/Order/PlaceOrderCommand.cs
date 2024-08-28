namespace Application.Usecases.Commands.Order;

public class PlaceOrderCommand
{
    public int ServiceId { get; set; }
    public string ServiceCategory { get; set; }
    public string ClothingName { get; set; }
    public string? ClothingSize { get; set; }
    public string? MaterialName { get; set; }
    public int? MaterialNeeded { get; set; }
    public string? AdditionalInfo { get; set; }
}