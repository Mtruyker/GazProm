using System;

namespace GasServiceApp.Models;

public class ServiceRequest
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int AddressId { get; set; }
    public int EquipmentId { get; set; }
    public int? MasterId { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeadlineDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string RequestType { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public bool IsOverdue => CompletionDate is null && DeadlineDate.Date < DateTime.Today;
}
