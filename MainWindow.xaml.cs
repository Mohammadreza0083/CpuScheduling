using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CpuScheduling.Algorithms;
using CpuScheduling.Entity;

namespace CpuScheduling
{
    /// <summary>
    /// Main window of the CPU Scheduling Simulator application.
    /// Handles the UI interactions and coordinates between different components.
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Collection of process models that is bound to the DataGrid.
        /// Uses ObservableCollection for automatic UI updates.
        /// </summary>
        public ObservableCollection<ProcessModel> ProcessesModel { get; }

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the ObservableCollection
            ProcessesModel = new ObservableCollection<ProcessModel>();

            // Add 10 empty rows with all fields null
            for (int i = 0; i < 10; i++)
            {
                ProcessesModel.Add(new ProcessModel
                {
                    Name = null,
                    ArrivalTime = null,
                    BurstTime = null
                });
            }

            // Bind the ObservableCollection to the DataGrid
            ProcessGrid.ItemsSource = ProcessesModel;

            // Add event handler for cell editing
            ProcessGrid.BeginningEdit += ProcessGrid_BeginningEdit;

            // Set default algorithm
            AlgorithmComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the beginning of cell editing in the ProcessGrid.
        /// Automatically assigns default process names when editing empty name cells.
        /// </summary>
        private void ProcessGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.Header.ToString() == "Process Name" && e.Row.Item is ProcessModel process)
            {
                // If name is null, set the default name
                if (process.Name == null)
                {
                    int index = ProcessesModel.IndexOf(process);
                    process.Name = $"P{index + 1}";
                }
            }
        }

        /// <summary>
        /// Handles the addition of new items to the ProcessGrid.
        /// Creates a new process with all fields initialized to null.
        /// </summary>
        private void ProcessGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            // Get the current count of items
            int currentCount = ProcessesModel.Count;
            
            // Create a new process with all fields null
            e.NewItem = new ProcessModel
            {
                Name = null,
                ArrivalTime = null,
                BurstTime = null
            };
        }

        /// <summary>
        /// Handles the algorithm execution button click.
        /// Validates input, runs the selected algorithm, and displays results.
        /// </summary>
        private void RunAlgorithm_Click(object sender, RoutedEventArgs e)
        {
            // Filter out processes with null values
            var validProcesses = ProcessesModel.Where(p => p.Name != null && p.ArrivalTime.HasValue && p.BurstTime.HasValue).ToList();
            var inputProcesses = ConvertToEntityProcesses(validProcesses);
            
            if (inputProcesses.Count <= 1)
            {
                MessageBox.Show("Please enter at least two processes with valid names, arrival times, and burst times.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<Process> scheduled;
            var selectedAlgorithm = ((ComboBoxItem)AlgorithmComboBox.SelectedItem).Content.ToString();

            switch (selectedAlgorithm)
            {
                case "First Come First Served (FCFS)":
                    scheduled = FirstComeFirstServed.Schedule(inputProcesses);
                    break;
                case "Shortest Job First (SJF)":
                    scheduled = ShortestJobFirst.Schedule(inputProcesses);
                    break;
                case "Round Robin (RR)":
                    // Get quantum time from user
                    var quantumDialog = new QuantumInputDialog();
                    if (quantumDialog.ShowDialog() == true)
                    {
                        scheduled = RoundRobin.Schedule(inputProcesses, quantumDialog.QuantumTime);
                    }
                    else
                    {
                        return;
                    }
                    break;
                case "Shortest Remaining Time (SRT)":
                    scheduled = ShortestTimeRemaining.Schedule(inputProcesses);
                    break;
                default:
                    MessageBox.Show("Please select an algorithm.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
            }

            var resultModels = ConvertToProcessModels(scheduled);
            ResultGrid.ItemsSource = resultModels;

            DrawGanttChart(ConvertToProcessModels(scheduled));
            
            // Show statistics in a modern dialog
            double avgTurnaround = resultModels.Average(p => p.TurnaroundTime);
            double avgWaiting = resultModels.Average(p => p.WaitingTime);
            
            var message = $"Algorithm: {selectedAlgorithm}\n\n" +
                         $"Average Turnaround Time: {avgTurnaround:F2}\n" +
                         $"Average Waiting Time: {avgWaiting:F2}";
            
            var dialog = new MaterialMessageBox(message, "Scheduling Statistics");
            dialog.ShowDialog();
        }

        /// <summary>
        /// Draws the Gantt chart visualization of the scheduling results.
        /// Creates rectangles for each process execution period with time markers.
        /// </summary>
        private void DrawGanttChart(List<ProcessModel> processes)
        {
            GanttChart.Children.Clear();
            double scale = 30;
            double xOffset = 10;

            // Create a gradient brush for the background
            var backgroundBrush = new LinearGradientBrush(
                Colors.LightGray,
                Colors.White,
                new Point(0, 0),
                new Point(1, 1));
            GanttChart.Background = backgroundBrush;

            foreach (var proc in processes)
            {
                // Create a modern-looking rectangle with rounded corners
                var rect = new Rectangle
                {
                    Width = (proc.FinishTime - proc.StartTime) * scale,
                    Height = 40,
                    RadiusX = 5,
                    RadiusY = 5,
                    Fill = new SolidColorBrush(GetProcessColor(proc.Name)),
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(rect, proc.StartTime * scale + xOffset);
                Canvas.SetTop(rect, 10);
                GanttChart.Children.Add(rect);

                // Add process name with modern styling
                var label = new TextBlock
                {
                    Text = proc.Name,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.White,
                    FontSize = 12,
                    Effect = new System.Windows.Media.Effects.DropShadowEffect
                    {
                        Color = Colors.Black,
                        Direction = 320,
                        ShadowDepth = 1,
                        Opacity = 0.5
                    }
                };

                Canvas.SetLeft(label, proc.StartTime * scale + xOffset + 5);
                Canvas.SetTop(label, 15);
                GanttChart.Children.Add(label);

                // Add time markers with modern styling
                var time = new TextBlock
                {
                    Text = proc.StartTime.ToString(),
                    FontSize = 10,
                    Foreground = Brushes.Gray
                };
                Canvas.SetLeft(time, proc.StartTime * scale + xOffset - 5);
                Canvas.SetTop(time, 55);
                GanttChart.Children.Add(time);
            }

            // Add final time marker
            var finalTime = new TextBlock
            {
                Text = processes.Last().FinishTime.ToString(),
                FontSize = 10,
                Foreground = Brushes.Gray
            };
            Canvas.SetLeft(finalTime, processes.Last().FinishTime * scale + xOffset - 5);
            Canvas.SetTop(finalTime, 55);
            GanttChart.Children.Add(finalTime);
        }

        /// <summary>
        /// Generates a consistent color for each process based on its name.
        /// </summary>
        private Color GetProcessColor(string processName)
        {
            // Generate a consistent color for each process
            var hash = processName.GetHashCode();
            return Color.FromRgb(
                (byte)((hash & 0xFF0000) >> 16),
                (byte)((hash & 0x00FF00) >> 8),
                (byte)(hash & 0x0000FF)
            );
        }

        /// <summary>
        /// Converts ProcessModel objects to Process entities for algorithm processing.
        /// </summary>
        private List<Process> ConvertToEntityProcesses(List<ProcessModel> processModels)
        {
            return processModels.Select(pm => new Process
            {
                Name = pm.Name,
                ArrivalTime = pm.ArrivalTime ?? 0,
                BurstTime = pm.BurstTime ?? 0
            }).ToList();
        }

        /// <summary>
        /// Converts Process entities back to ProcessModel objects for display.
        /// </summary>
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

    /// <summary>
    /// Model class representing a process in the UI.
    /// Contains all necessary properties for process scheduling and display.
    /// </summary>
    public class ProcessModel
    {
        public string? Name { get; set; }
        public int? ArrivalTime { get; set; }
        public int? BurstTime { get; set; }
        public int StartTime { get; set; }
        public int FinishTime { get; set; }
        public int TurnaroundTime { get; set; }
        public int WaitingTime { get; set; }
    }
}