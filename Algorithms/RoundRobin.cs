using CpuScheduling.Entity;

namespace CpuScheduling.Algorithms;

public static class RoundRobin
{
    public static List<Process> Schedule(List<Process> processes, int timeQuantum)
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

        var currentTime = 0;
        var index = 0;

        while (readyQueue.Count > 0 || index < processList.Count)
        {
            while (index < processList.Count && processList[index].ArrivalTime <= currentTime)
            {
                readyQueue.Enqueue(processList[index]);
                index++;
            }

            if (readyQueue.Count == 0)
            {
                currentTime = processList[index].ArrivalTime;
                continue;
            }

            var current = readyQueue.Dequeue();

            if (current.StartTime == 0 && current.RemainingTime == current.BurstTime)
                current.StartTime = currentTime;

            int executionTime = Math.Min(timeQuantum, current.RemainingTime);
            currentTime += executionTime;
            current.RemainingTime -= executionTime;
            while (index < processList.Count && processList[index].ArrivalTime <= currentTime)
            {
                readyQueue.Enqueue(processList[index]);
                index++;
            }

            if (current.RemainingTime > 0)
            {
                readyQueue.Enqueue(current);
            }
            else
            {
                current.FinishTime = currentTime;
                current.TurnaroundTime = current.FinishTime - current.ArrivalTime;
                current.WaitingTime = current.TurnaroundTime - current.BurstTime;
                result.Add(current);
            }
        }

        return result;
    }
}
