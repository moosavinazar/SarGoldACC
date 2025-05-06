using System.Windows;

namespace SarGoldACC.WpfApp.Helpers;

public static class MessageBoxHelper
{
    public static void ShowError(string message)
    {
        MessageBox.Show(message);
    }
    
    public static void ShowSuccess(string message)
    {
        MessageBox.Show(message);
    }

    public static bool ShowDeleteConfirm(string message)
    {
        var result = MessageBox.Show("آیا از حذف این مورد مطمئن هستید؟", "تأیید حذف", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        return result == MessageBoxResult.Yes;
    }

    public static bool ShowConfirm(string message)
    {
        var result = MessageBox.Show(message, "تأیید", MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }
}