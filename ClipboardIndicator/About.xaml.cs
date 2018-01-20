using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace ClipboardIndicator
{
    public partial class About : Window
    {
        public About() => InitializeComponent();

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
        private void Button_Click(object sender, RoutedEventArgs e) => DialogResult = true;
    }
}
