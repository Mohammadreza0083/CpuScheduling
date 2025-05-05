using CpuScheduling.Entity;

namespace CpuScheduling.Algorithms;

public static class ShortestTimeRemaining
{
    public static List<Process> Schedule(List<Process> processes)
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

        var currentTime = 0;
        var completed = new HashSet<Process>();

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
                currentTime++;
            }
        }

        return result;
    }
}