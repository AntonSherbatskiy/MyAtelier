namespace MyAtelier.DAL.Entities;

public class UserCode : Identity
{
    public string Email { get; set; }
    
    public int Code { get; set; }
}