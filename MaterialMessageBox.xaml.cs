using System.Windows;

namespace CpuScheduling
{
    /// <summary>
    /// Custom message box window with Material Design styling.
    /// Used for displaying scheduling statistics and other information.
    /// </summary>
    public partial class MaterialMessageBox
    {
        /// <summary>
        /// Gets or sets the message to be displayed in the dialog.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the title of the dialog window.
        /// </summary>
        public new string Title { get; set; }

        /// <summary>
        /// Initializes a new instance of the MaterialMessageBox class.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="title">The title of the dialog</param>
        public MaterialMessageBox(string message, string title)
        {
            InitializeComponent();
            Message = message;
            Title = title;
            DataContext = this;
        }

        /// <summary>
        /// Handles the OK button click event.
        /// Closes the dialog window.
        /// </summary>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
} 