using CpuScheduling.Entity;

namespace CpuScheduling.Algorithms;

/// <summary>
/// Implements the First Come First Served (FCFS) CPU scheduling algorithm.
/// Processes are executed in the order of their arrival time.
/// </summary>
public class FirstComeFirstServed
{
    /// <summary>
    /// Schedules processes using the FCFS algorithm.
    /// </summary>
    /// <param name="processes">List of processes to be scheduled</param>
    /// <returns>List of scheduled processes with calculated timing information</returns>
    public static List<Process> Schedule(List<Process> processes)
    {
        var result = new List<Process>();
        //if you want sort process before starting algorithms use sorted variable instead processes
        // var sorted = processes.OrderBy(p => p.ArrivalTime).ToList();
        int currentTime = 0;

        foreach (var p in processes)
        {
            var process = new Process
            {
                Name = p.Name,
                ArrivalTime = p.ArrivalTime,
                BurstTime = p.BurstTime
            };

            // Calculate start time (max of current time and arrival time)
            process.StartTime = Math.Max(currentTime, process.ArrivalTime);
            process.FinishTime = process.StartTime + process.BurstTime;
            process.TurnaroundTime = process.FinishTime - process.ArrivalTime;
            process.WaitingTime = process.TurnaroundTime - process.BurstTime;
            currentTime = process.FinishTime;

            result.Add(process);
        }

        return result;
    }
}