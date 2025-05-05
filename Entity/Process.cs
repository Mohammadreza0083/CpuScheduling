namespace CpuScheduling.Entity;

public class Process
{
    public required string Name { get; set; }
    public int ArrivalTime { get; set; }
    public int BurstTime { get; set; }
    public int StartTime { get; set; }
    public int FinishTime { get; set; }
    public int TurnaroundTime { get; set; }
    public int WaitingTime { get; set; }
    public int RemainingTime { get; set; }
}