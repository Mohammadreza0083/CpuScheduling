using System.Windows;

namespace CpuScheduling
{
    public partial class MaterialMessageBox
    {
        public string Message { get; set; }
        public new string Title { get; set; }

        public MaterialMessageBox(string message, string title)
        {
            InitializeComponent();
            Message = message;
            Title = title;
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
} 