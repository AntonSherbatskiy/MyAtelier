namespace MyAtelier.DAL.Entities;

public class ServiceAggregator : Identity
{
    public int? RepairingServiceId { get; set; }
    public int? SewingServiceId { get; set; }
    
    public RepairingService RepairingService { get; set; }
    public SewingService SewingService { get; set; }
    public string ServiceType { get; set; }
    
    public List<Order> Orders { get; set; }
}