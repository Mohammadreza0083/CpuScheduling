using CpuScheduling.Entity;

namespace CpuScheduling.Algorithms;

public class ShortestJobFirst
{
    public static List<Process> Schedule(List<Process> processes)
    {
        var result = new List<Process>();
        var waitingList = processes.OrderBy(p => p.ArrivalTime).ToList();
        int currentTime = 0;

        while (waitingList.Count > 0)
        {
            var available = waitingList.Where(p => p.ArrivalTime <= currentTime).ToList();

            if (available.Count == 0)
            {
                currentTime = waitingList.Min(p => p.ArrivalTime);
                continue;
            }

            var shortest = available.OrderBy(p => p.BurstTime).First();

            var process = new Process
            {
                Name = shortest.Name,
                ArrivalTime = shortest.ArrivalTime,
                BurstTime = shortest.BurstTime
            };

            process.StartTime = Math.Max(currentTime, process.ArrivalTime);
            process.FinishTime = process.StartTime + process.BurstTime;
            process.TurnaroundTime = process.FinishTime - process.ArrivalTime;
            process.WaitingTime = process.StartTime - process.ArrivalTime;
            currentTime = process.FinishTime;

            result.Add(process);
            waitingList.Remove(shortest);
        }

        return result;
    }
}