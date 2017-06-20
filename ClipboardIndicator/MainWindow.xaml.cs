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
            if(msg == WM_MOVING)
            {
                var window = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
                var w = (int)SystemParameters.PrimaryScreenWidth;
                var h = (int)SystemParameters.PrimaryScreenHeight;

                if(window.Left < 0) SetLeft(ref window);
                if(window.Top < 0) SetTop(ref window);
                if(window.Right > w) SetRight(ref window, w);
                if(window.Bottom > h) SetBottom(ref window, h);

                Marshal.StructureToPtr(window, lParam, true);

                handled = true;
                return new IntPtr(1);
            }

            handled = false;
            return IntPtr.Zero;
        }
        private const int WM_MOVING = 0x0216;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left, Top, Right, Bottom; }

        private void SetLeft(ref RECT rect)
        {
            rect.Right = rect.Right - rect.Left;
            rect.Left = 0;
        }
        private void SetTop(ref RECT rect)
        {
            rect.Bottom = rect.Bottom - rect.Top;
            rect.Top = 0;
        }
        private void SetRight(ref RECT rect, int width)
        {
            rect.Left = width - rect.Right + rect.Left;
            rect.Right = width;
        }
        private void SetBottom(ref RECT rect, int height)
        {
            rect.Top = height - rect.Bottom + rect.Top;
            rect.Bottom = height;
        }
        #endregion
    }
}
