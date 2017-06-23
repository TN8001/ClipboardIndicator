using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;

namespace ClipboardIndicator
{
    public partial class About : Window
    {
        private static Version v = Assembly.GetExecutingAssembly().GetName().Version;
        public string AssemblyVersion { get; } = $"{v.Major}.{v.Minor}.{v.Build}";
        public string AssemblyName { get; } = Assembly.GetEntryAssembly().GetName().Name;
        public string AssemblyCopyright { get; } = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute))).Copyright;

        public About()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
        private void Button_Click(object sender, RoutedEventArgs e) => DialogResult = true;
    }
}
