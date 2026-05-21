using System;

namespace GasServiceApp.Models;

public class OverdueRequestReportItem
{
    public int RequestId { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeadlineDate { get; set; }
    public string Client { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Master { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
