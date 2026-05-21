using System;

namespace GasServiceApp.Models;

public class WorkRecord
{
    public int Id { get; set; }
    public int ServiceRequestId { get; set; }
    public int MasterId { get; set; }
    public DateTime WorkDate { get; set; }
    public decimal Cost { get; set; }
    public string WorkType { get; set; } = string.Empty;
    public string MaterialsUsed { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
}
