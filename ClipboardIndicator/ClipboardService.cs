using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace ClipboardIndicator
{
    public delegate void ClipboardCopiedEventHandler(string text);

    public class ClipboardService
    {
        private const int WM_CLIPBOARDUPDATE = 0x031D;

        [DllImport("user32.dll", SetLastError = true)]
        private extern static void AddClipboardFormatListener(IntPtr hwnd);
        [DllImport("user32.dll", SetLastError = true)]
        private extern static void RemoveClipboardFormatListener(IntPtr hwnd);


        public event ClipboardCopiedEventHandler ClipboardCopied;

        private Window actualWindow;
        private IntPtr handle;

        public ClipboardService(Window window)
        {
            actualWindow = window;

            if(!actualWindow.IsArrangeValid)
                actualWindow.SourceInitialized += (s, e) => Hook();
            else Hook();

            actualWindow.Closing += (s, e) => Unhook();
        }

        public void Clear()
        {
            for(var i = 0; i < 5; i++) //CLIPBRD_E_CANT_OPEN対策にリトライ処理
            {
                try
                {
                    Clipboard.Clear();
                    return;
                }
                catch { Task.Delay(100).Wait(); }
            }
        }

        private void Hook()
        {
            handle = new WindowInteropHelper(actualWindow).Handle;
            HwndSource.FromHwnd(handle).AddHook(WndProc);
            AddClipboardFormatListener(handle);

            OnClipboardCopied();
        }
        private void Unhook()
        {
            RemoveClipboardFormatListener(handle);
            HwndSource.FromHwnd(handle).RemoveHook(WndProc);
            handle = IntPtr.Zero;
        }
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(msg == WM_CLIPBOARDUPDATE)
                OnClipboardCopied();

            return IntPtr.Zero;
        }
        private void OnClipboardCopied()
        {
            var text = GetText();
            Debug.WriteLine($"ClipboardCopied:{text}");
            ClipboardCopied(text);
        }
        private string GetText()
        {
            for(var i = 0; i < 5; i++) //CLIPBRD_E_CANT_OPEN対策にリトライ処理
            {
                try
                {
                    return GetClipboardTextFirstLine();
                }
                catch { Task.Delay(100).Wait(); }
            }

            return "<!CANT_OPEN>";
        }
        private string GetClipboardTextFirstLine()
        {
            if(Clipboard.Contains​Text())
                return Clipboard.GetText()
                                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                                .First();
            else if(!Clipboard.GetDataObject().GetFormats().Any())
                return "<Empty>";
            else
                return "<Not Text Data>";
        }
    }
}

