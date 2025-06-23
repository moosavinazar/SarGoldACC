using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

public class WindowFocusHelper
{
    [DllImport("user32.dll")]
    static extern IntPtr GetShellWindow(); // دسکتاپ

    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hWnd);

    public static void SimulateFocusLossAndRestore(Window window)
    {
        IntPtr desktopHandle = GetShellWindow();
        IntPtr thisHandle = new WindowInteropHelper(window).Handle;

        // فوکوس به دسکتاپ
        SetForegroundWindow(desktopHandle);

        // بعد از یک تاخیر کوچک فوکوس را برگردان
        window.Dispatcher.InvokeAsync(async () =>
        {
            await Task.Delay(150);
            SetForegroundWindow(thisHandle);
        });
    }
}