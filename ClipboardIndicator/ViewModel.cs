using System.Windows;

namespace ClipboardIndicator
{
    public class ViewModel : BindableBase
    {
        public string ClipboardText { get => _ClipboardText; set => SetProperty(ref _ClipboardText, value); }
        private string _ClipboardText;

        public SettingsModel Setting { get; }
        public SettingWindow SettingWindow { get; }

        public DelegateCommand ClearCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand ShowSettingCommand { get; }
        public DelegateCommand ShowAboutCommand { get; }

        public ViewModel()
        {
            Setting = new SettingsModel();
        }
        public ViewModel(ClipboardService clip)
        {
            var serializer = new SerializeHelper<SettingsModel>();
            Setting = serializer.Load();

            SettingWindow = new SettingWindow();
            SettingWindow.DataContext = Setting;

            ClearCommand = new DelegateCommand(() => clip.Clear());
            ShowSettingCommand = new DelegateCommand(() => SettingWindow.Show());
            CloseCommand = new DelegateCommand(() =>
            {
                serializer.Save(Setting);
                Application.Current.MainWindow.Close();
            });
            ShowAboutCommand = new DelegateCommand(() =>
            {
                var dlg = new About();
                dlg.Owner = Application.Current.MainWindow;
                dlg.ShowDialog();
            });

            clip.ClipboardCopied += txt => ClipboardText = txt;
        }
    }
}
