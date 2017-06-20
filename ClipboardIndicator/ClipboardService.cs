using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ClipboardIndicator
{
    public class ClipboardService
    {
        [DllImport("user32.dll", SetLastError = true)]
        private extern static void AddClipboardFormatListener(IntPtr hwnd);
        [DllImport("user32.dll", SetLastError = true)]
        private extern static void RemoveClipboardFormatListener(IntPtr hwnd);

        private const int WM_CLIPBOARDUPDATE = 0x031D;

        public delegate void ClipboardCopiedEventHandler(string text);
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
        public void Clear() => Clipboard.Clear();

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
            var text = "";
            try
            {
                if(Clipboard.Contains​Text())
                    text = Clipboard.GetText()
                                    .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                                    .First();
                else
                {
                    if(!Clipboard.GetDataObject().GetFormats().Any())
                        text = "<Empty>";
                    else
                        text = "<Not Text Data>";
                }
            }
            catch(COMException) { /* CLIPBRD_E_CANT_OPEN対策 */  text = "<!CANT_OPEN>"; }

            Debug.WriteLine($"ClipboardCopied:{text}");
            ClipboardCopied(text);
        }
    }
}

