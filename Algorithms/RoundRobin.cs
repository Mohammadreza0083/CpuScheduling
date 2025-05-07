using CpuScheduling.Entity;

namespace CpuScheduling.Algorithms;

/// <summary>
/// Implements the Round Robin (RR) CPU scheduling algorithm.
/// Each process gets a fixed time quantum for execution before being preempted.
/// </summary>
public static class RoundRobin
{
    /// <summary>
    /// Schedules processes using the Round Robin algorithm.
    /// </summary>
    /// <param name="processes">List of processes to be scheduled</param>
    /// <param name="timeQuantum">Time quantum for each process execution</param>
    /// <returns>Tuple: List of scheduled processes, List of Gantt slices</returns>
    public static (List<Process> scheduled, List<GanttSlice> gantt) Schedule(List<Process> processes, int timeQuantum)
    {
        var result = new List<Process>();
        var readyQueue = new Queue<Process>();
        var processList = processes.Select(p => new Process
        {
            Name = p.Name,
            ArrivalTime = p.ArrivalTime,
            BurstTime = p.BurstTime,
            RemainingTime = p.BurstTime
        }).OrderBy(p => p.ArrivalTime).ToList();

        var gantt = new List<GanttSlice>();
        var currentTime = 0;
        var index = 0;

        while (readyQueue.Count > 0 || index < processList.Count)
        {
            // Add newly arrived processes to the ready queue
            while (index < processList.Count && processList[index].ArrivalTime <= currentTime)
            {
                readyQueue.Enqueue(processList[index]);
                index++;
            }

            // If no process is ready, advance time to next arrival
            if (readyQueue.Count == 0)
            {
                currentTime = processList[index].ArrivalTime;
                continue;
            }

            var current = readyQueue.Dequeue();

            // Set start time for first execution
            if (current.StartTime == 0 && current.RemainingTime == current.BurstTime)
                current.StartTime = currentTime;

            // Execute process for time quantum or remaining time
            int executionTime = Math.Min(timeQuantum, current.RemainingTime);
            int sliceStart = currentTime;
            currentTime += executionTime;
            current.RemainingTime -= executionTime;
            int sliceEnd = currentTime;

            // Add Gantt slice
            gantt.Add(new GanttSlice { Name = current.Name, Start = sliceStart, End = sliceEnd });

            // Add newly arrived processes during execution
            while (index < processList.Count && processList[index].ArrivalTime <= currentTime)
            {
                readyQueue.Enqueue(processList[index]);
                index++;
            }

            // If process is not finished, add it back to ready queue
            if (current.RemainingTime > 0)
            {
                readyQueue.Enqueue(current);
            }
            else
            {
                // Process is finished, calculate timing information
                current.FinishTime = currentTime;
                current.TurnaroundTime = current.FinishTime - current.ArrivalTime;
                current.WaitingTime = current.TurnaroundTime - current.BurstTime;
                result.Add(current);
            }
        }

        return (result, gantt);
    }
}
