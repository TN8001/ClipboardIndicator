using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ClipboardIndicator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //This seems really hacky but setting
            //https://stackoverflow.com/questions/42769357/wpf-sizetocontent-not-working-when-windowstyle-none
            Width = 0;
            Height = 0;

            DataContext = new ViewModel(new ClipboardService(this));
        }

        //余計なお世話な場合region丸ごと削除で大丈夫です
        #region ウィンドウ移動制限
        private const int WM_MOVING = 0x0216;
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left, Top, Right, Bottom; }
        private HwndSource hwndSource;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            hwndSource.AddHook(WndProc);
        }
        protected override void OnClosed(EventArgs e)
        {
            hwndSource.RemoveHook(WndProc);
            hwndSource.Dispose();
            hwndSource = null;

            base.OnClosed(e);
        }
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(msg != WM_MOVING)
            {
                handled = false;
                return IntPtr.Zero;
            }

            var rect = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
            var w = (int)SystemParameters.PrimaryScreenWidth;
            var h = (int)SystemParameters.PrimaryScreenHeight;

            if(rect.Left < 0)
            {
                rect.Right = rect.Right - rect.Left;
                rect.Left = 0;
            }
            if(rect.Top < 0)
            {
                rect.Bottom = rect.Bottom - rect.Top;
                rect.Top = 0;
            }
            if(rect.Right > w)
            {
                rect.Left = w - rect.Right + rect.Left;
                rect.Right = w;
            }
            if(rect.Bottom > h)
            {
                rect.Top = h - rect.Bottom + rect.Top;
                rect.Bottom = h;
            }

            Marshal.StructureToPtr(rect, lParam, true);

            handled = true;
            return new IntPtr(1);
        }
        #endregion
    }
}
