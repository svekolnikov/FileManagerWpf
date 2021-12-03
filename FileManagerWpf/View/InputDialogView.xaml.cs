using System.Windows;

namespace FileManagerWpf.View
{
    // https://wpf-tutorial.com/dialogs/creating-a-custom-input-dialog/
    public partial class InputDialogView : Window
    {
        /// <summary>
        /// Simple dialog window
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="label">Lable</param>
        /// <param name="inputText">Default suggested text</param>
        public InputDialogView(string title, string label, string inputText)
        {
            InitializeComponent();

            Title = title;
            Label.Content = label;
            InputText.Text = inputText;
        }

        public string Answer => InputText.Text;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            InputText.SelectAll();
            InputText.Focus();            
        }
    }
}
