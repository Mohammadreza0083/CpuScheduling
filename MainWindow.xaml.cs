using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CpuScheduling.Algorithms;
using CpuScheduling.Entity;

namespace CpuScheduling
{
    public partial class MainWindow
    {
        public ObservableCollection<ProcessModel> ProcessesModel { get; }

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the ObservableCollection
            ProcessesModel = new ObservableCollection<ProcessModel>();

            // Bind the ObservableCollection to the DataGrid
            ProcessGrid.ItemsSource = ProcessesModel;
        }

        private void RunFCFS_Click(object sender, RoutedEventArgs e)
        {
            var inputProcesses = ConvertToEntityProcesses(ProcessesModel.ToList());
            if (inputProcesses.Count <= 1)
            {
                MessageBox.Show("Please enter at least two processes.");
                return;
            }

            var scheduled = FirstComeFirstServed.Schedule(inputProcesses);
            var resultModels = ConvertToProcessModels(scheduled);
            ResultGrid.ItemsSource = resultModels;

            DrawGanttChart(ConvertToProcessModels(scheduled));
            double avgTurnaround = resultModels.Average(p => p.TurnaroundTime);
            double avgWaiting = resultModels.Average(p => p.WaitingTime);
            MessageBox.Show($"Average Turnaround Time: {avgTurnaround:F2}\nAverage Waiting Time: {avgWaiting:F2}");
        }

        private void RunSJF_Click(object sender, RoutedEventArgs e)
        {
            var inputProcesses = ConvertToEntityProcesses(ProcessesModel.ToList());
            if (inputProcesses.Count <= 1)
            {
                MessageBox.Show("Please enter at least two processes.");
                return;
            }

            var scheduled = ShortestJobFirst.Schedule(inputProcesses);
            var resultModels = ConvertToProcessModels(scheduled);
            ResultGrid.ItemsSource = resultModels;

            DrawGanttChart(ConvertToProcessModels(scheduled));
            double avgTurnaround = resultModels.Average(p => p.TurnaroundTime);
            double avgWaiting = resultModels.Average(p => p.WaitingTime);
            MessageBox.Show($"Average Turnaround Time: {avgTurnaround:F2}\nAverage Waiting Time: {avgWaiting:F2}");
        }

        private void DrawGanttChart(List<ProcessModel> processes)
        {
            GanttChart.Children.Clear();
            double scale = 30;
            double xOffset = 10;

            foreach (var proc in processes)
            {
                var rect = new Rectangle
                {
                    Width = (proc.FinishTime - proc.StartTime) * scale,
                    Height = 40,
                    Fill = new SolidColorBrush(Color.FromRgb(
                        (byte)new Random(proc.Name.GetHashCode()).Next(50, 255),
                        (byte)new Random(proc.Name.GetHashCode() + 1).Next(50, 255),
                        (byte)new Random(proc.Name.GetHashCode() + 2).Next(50, 255)
                    ))
                };

                Canvas.SetLeft(rect, proc.StartTime * scale + xOffset);
                Canvas.SetTop(rect, 10);
                GanttChart.Children.Add(rect);

                var label = new TextBlock
                {
                    Text = proc.Name,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black
                };

                Canvas.SetLeft(label, proc.StartTime * scale + xOffset + 5);
                Canvas.SetTop(label, 15);
                GanttChart.Children.Add(label);

                var time = new TextBlock
                {
                    Text = proc.StartTime.ToString(),
                    FontSize = 10
                };
                Canvas.SetLeft(time, proc.StartTime * scale + xOffset - 5);
                Canvas.SetTop(time, 55);
                GanttChart.Children.Add(time);
            }

            var finalTime = new TextBlock
            {
                Text = processes.Last().FinishTime.ToString(),
                FontSize = 10
            };
            Canvas.SetLeft(finalTime, processes.Last().FinishTime * scale + xOffset - 5);
            Canvas.SetTop(finalTime, 55);
            GanttChart.Children.Add(finalTime);
        }

        private List<Process> ConvertToEntityProcesses(List<ProcessModel> processModels)
        {
            return processModels.Select(pm => new Process
            {
                Name = pm.Name,
                ArrivalTime = pm.ArrivalTime,
                BurstTime = pm.BurstTime
            }).ToList();
        }

        private List<ProcessModel> ConvertToProcessModels(List<Process> entityProcesses)
        {
            return entityProcesses.Select(ep => new ProcessModel
            {
                Name = ep.Name,
                ArrivalTime = ep.ArrivalTime,
                BurstTime = ep.BurstTime,
                StartTime = ep.StartTime,
                FinishTime = ep.FinishTime,
                TurnaroundTime = ep.TurnaroundTime,
                WaitingTime = ep.WaitingTime
            }).ToList();
        }
    }

    public class ProcessModel
    {
        public required string Name { get; set; }
        public int ArrivalTime { get; set; }
        public int BurstTime { get; set; }
        public int StartTime { get; set; }
        public int FinishTime { get; set; }
        public int TurnaroundTime { get; set; }
        public int WaitingTime { get; set; }
    }
}