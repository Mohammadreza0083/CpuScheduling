using System.Windows;

namespace CpuScheduling
{
    public partial class QuantumInputDialog : Window
    {
        public int QuantumTime { get; private set; }

        public QuantumInputDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantumTextBox.Text, out int quantum) && quantum > 0)
            {
                QuantumTime = quantum;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a valid positive integer for quantum time.", 
                              "Invalid Input", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
} 