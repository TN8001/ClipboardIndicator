using System.Windows;

namespace ClipboardIndicator
{
    public class ViewModel : BindableBase
    {
        public string ClipboardText { get => _ClipboardText; set => Set(ref _ClipboardText, value); }
        private string _ClipboardText;

        public SettingsModel Setting { get; }
        public SettingWindow SettingWindow { get; }

        public DelegateCommand ClearCommand { get; }
        public DelegateCommand TopMostCommand { get; }
        public DelegateCommand ShowSettingCommand { get; }
        public DelegateCommand ShowAboutCommand { get; }
        public DelegateCommand CloseCommand { get; }

        // DesignInstance用
        public ViewModel() => Setting = new SettingsModel();
        // clip <- MainWindowから注入
        public ViewModel(ClipboardService clip)
        {
            var serializer = new SerializeHelper<SettingsModel>();
            Setting = serializer.Load();

            SettingWindow = new SettingWindow();
            SettingWindow.DataContext = Setting;

            ClearCommand = new DelegateCommand(() => clip.Clear());
            TopMostCommand = new DelegateCommand(() =>
            {
                var w = Application.Current.MainWindow;
                w.Topmost = false;
                w.Topmost = true;
            });
            ShowSettingCommand = new DelegateCommand(() => SettingWindow.Show());
            ShowAboutCommand = new DelegateCommand(() =>
            {
                var dlg = new About();
                dlg.Owner = Application.Current.MainWindow;
                dlg.ShowDialog();
            });
            CloseCommand = new DelegateCommand(() =>
            {
                serializer.Save(Setting);
                Application.Current.MainWindow.Close();
            });

            clip.ClipboardCopied += text => ClipboardText = text;
        }
    }
}
