namespace CpuScheduling.Entity;

/// <summary>
/// Represents a single execution interval (slice) for a process in the Gantt chart.
/// </summary>
public class GanttSlice
{
    public string? Name { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
}