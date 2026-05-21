using System;

namespace GasServiceApp.Models;

public class CompletedWorkReportItem
{
    public int RequestId { get; set; }
    public DateTime CompletionDate { get; set; }
    public string Client { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;
    public string Master { get; set; } = string.Empty;
    public string WorkType { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string Result { get; set; } = string.Empty;
}
