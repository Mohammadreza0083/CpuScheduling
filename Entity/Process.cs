namespace CpuScheduling.Entity;

/// <summary>
/// Represents a process in the CPU scheduling system.
/// Contains all necessary information for process scheduling and timing calculations.
/// </summary>
public class Process
{
    /// <summary>
    /// Unique identifier for the process
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Time at which the process arrives in the system
    /// </summary>
    public int ArrivalTime { get; set; }

    /// <summary>
    /// Total CPU time required by the process
    /// </summary>
    public int BurstTime { get; set; }

    /// <summary>
    /// Time at which the process starts execution
    /// </summary>
    public int StartTime { get; set; }

    /// <summary>
    /// Time at which the process completes execution
    /// </summary>
    public int FinishTime { get; set; }

    /// <summary>
    /// Total time from arrival to completion (FinishTime - ArrivalTime)
    /// </summary>
    public int TurnaroundTime { get; set; }

    /// <summary>
    /// Time spent waiting in the ready queue (TurnaroundTime - BurstTime)
    /// </summary>
    public int WaitingTime { get; set; }

    /// <summary>
    /// Remaining CPU time needed for process completion
    /// Used in preemptive algorithms
    /// </summary>
    public int RemainingTime { get; set; }
}