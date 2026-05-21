using System;

namespace GasServiceApp.Models;

public class Equipment
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int AddressId { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime InstallationDate { get; set; }
    public DateTime? NextInspectionDate { get; set; }
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
