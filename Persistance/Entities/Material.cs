namespace MyAtelier.DAL.Entities;

public class Material : Identity
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    
    public List<SewingService> SewingServices { get; set; }
}