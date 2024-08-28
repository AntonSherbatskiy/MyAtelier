namespace MyAtelier.DAL.Entities;

public class Clothing : Identity
{
    public string Name { get; set; }
    public List<RepairingService> RepairingServices { get; set; }
    public List<SewingService> SewingServices { get; set; }
}