using CpuScheduling.Entity;

namespace CpuScheduling.Algorithms;

public class FirstComeFirstServed
{
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

            process.StartTime = Math.Max(currentTime, process.ArrivalTime);
            process.FinishTime = process.StartTime + process.BurstTime;
            currentTime = process.FinishTime;

            result.Add(process);
        }

        return result;
    }
}