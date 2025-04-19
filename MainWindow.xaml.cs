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
        public MainWindow()
        {
            InitializeComponent();
            ProcessGrid.ItemsSource = new List<Process>();
        }

        private void RunFCFS_Click(object sender, RoutedEventArgs e)
        {
            var inputProcesses = ProcessGrid.Items.OfType<Process>().ToList();
            if (inputProcesses.Count == 0)
            {
                MessageBox.Show("Please enter at least two process.");
                return;
            }

            var scheduled = FirstComeFirstServed.Schedule(inputProcesses);
            ResultGrid.ItemsSource = scheduled;

            DrawGanttChart(scheduled);
        }
        private void RunSJF_Click(object sender, RoutedEventArgs e)
        {
            var inputProcesses = ProcessGrid.Items.OfType<Process>().ToList();
            if (inputProcesses.Count == 0)
            {
                MessageBox.Show("Please enter at least two process.");
                return;
            }

            var scheduled = ShortestJobFirst.Schedule(inputProcesses);
            ResultGrid.ItemsSource = scheduled;

            DrawGanttChart(scheduled);
        }

        private void DrawGanttChart(List<Process> processes)
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
    }
}
