namespace GasServiceApp.Models;

public class ClientAddress
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string House { get; set; } = string.Empty;
    public string Apartment { get; set; } = string.Empty;
    public string Entrance { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string FullAddress { get; set; } = string.Empty;
}
