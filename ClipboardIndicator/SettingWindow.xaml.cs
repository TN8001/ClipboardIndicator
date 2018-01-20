using System;
using System.ComponentModel;
using System.Windows;

namespace ClipboardIndicator
{
    public partial class SettingWindow : Window
    {
        public SettingWindow() => InitializeComponent();

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.HideMinMaxButton();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;
            Hide();
        }
    }
}
