using CpuScheduling.Entity;

namespace CpuScheduling.Algorithms;

public static class ShortestTimeRemaining
{
    /// <summary>
    /// Schedules processes using the Shortest Remaining Time (SRT) algorithm.
    /// </summary>
    /// <param name="processes">List of processes to be scheduled</param>
    /// <returns>Tuple: List of scheduled processes, List of Gantt slices</returns>
    public static (List<Process> scheduled, List<GanttSlice> gantt) Schedule(List<Process> processes)
    {
        var result = new List<Process>();
        var processList = processes
            .Select(p => new Process
            {
                Name = p.Name,
                ArrivalTime = p.ArrivalTime,
                BurstTime = p.BurstTime,
                RemainingTime = p.BurstTime
            })
            .OrderBy(p => p.ArrivalTime)
            .ToList();

        var gantt = new List<GanttSlice>();
        var currentTime = 0;
        var completed = new HashSet<Process>();
        Process? lastProcess = null;
        int sliceStart = 0;

        while (completed.Count < processList.Count)
        {
            var available = processList
                .Where(p => p.ArrivalTime <= currentTime && !completed.Contains(p) && p.RemainingTime > 0)
                .OrderBy(p => p.RemainingTime)
                .ThenBy(p => p.ArrivalTime)
                .ToList();

            if (available.Any())
            {
                var selected = available.First();

                if (selected.StartTime == 0 && selected.RemainingTime == selected.BurstTime)
                    selected.StartTime = currentTime;

                // Track Gantt slices
                if (lastProcess != selected)
                {
                    if (lastProcess != null)
                        gantt.Add(new GanttSlice { Name = lastProcess.Name, Start = sliceStart, End = currentTime });
                    sliceStart = currentTime;
                    lastProcess = selected;
                }

                selected.RemainingTime--;
                currentTime++;

                if (selected.RemainingTime == 0)
                {
                    selected.FinishTime = currentTime;
                    selected.TurnaroundTime = selected.FinishTime - selected.ArrivalTime;
                    selected.WaitingTime = selected.TurnaroundTime - selected.BurstTime;
                    completed.Add(selected);
                    result.Add(selected);
                }
            }
            else
            {
                // Track idle time in Gantt chart
                if (lastProcess != null)
                {
                    gantt.Add(new GanttSlice { Name = lastProcess.Name, Start = sliceStart, End = currentTime });
                    lastProcess = null;
                }
                currentTime++;
                sliceStart = currentTime;
            }
        }
        // Add the last running process slice
        if (lastProcess != null)
            gantt.Add(new GanttSlice { Name = lastProcess.Name, Start = sliceStart, End = currentTime });

        return (result, gantt);
    }
}