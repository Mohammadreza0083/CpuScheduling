using System.Windows;

namespace CpuScheduling
{
    /// <summary>
    /// Dialog window for inputting quantum time in Round Robin algorithm.
    /// Validates input to ensure a positive integer is entered.
    /// </summary>
    public partial class QuantumInputDialog : Window
    {
        /// <summary>
        /// Gets the quantum time value entered by the user.
        /// </summary>
        public int QuantumTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the QuantumInputDialog class.
        /// </summary>
        public QuantumInputDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the OK button click event.
        /// Validates the input and sets the quantum time if valid.
        /// </summary>
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

        /// <summary>
        /// Handles the Cancel button click event.
        /// Closes the dialog without setting a quantum time.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
} 