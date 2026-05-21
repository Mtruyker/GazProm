using System;

namespace GasServiceApp.Models;

public class InspectionResult
{
    public int Id { get; set; }
    public int ServiceRequestId { get; set; }
    public int EquipmentId { get; set; }
    public DateTime InspectionDate { get; set; }
    public bool IsSafe { get; set; }
    public string GasLeakCheck { get; set; } = string.Empty;
    public string VentilationCheck { get; set; } = string.Empty;
    public string AutomationCheck { get; set; } = string.Empty;
    public string Conclusion { get; set; } = string.Empty;
    public string Recommendations { get; set; } = string.Empty;
}
