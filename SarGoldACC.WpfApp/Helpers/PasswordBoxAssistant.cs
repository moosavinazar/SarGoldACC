using System.Windows;
using System.Windows.Controls;

namespace SarGoldACC.WpfApp.Helpers;

public static class PasswordBoxAssistant
{
    public static readonly DependencyProperty BoundPassword =
        DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxAssistant),
            new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

    public static string GetBoundPassword(DependencyObject dp) =>
        (string)dp.GetValue(BoundPassword);

    public static void SetBoundPassword(DependencyObject dp, string value) =>
        dp.SetValue(BoundPassword, value);

    private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PasswordBox passwordBox)
        {
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
            if (!GetIsUpdating(passwordBox))
                passwordBox.Password = e.NewValue?.ToString() ?? string.Empty;
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }
    }

    private static readonly DependencyProperty IsUpdatingProperty =
        DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxAssistant));

    private static bool GetIsUpdating(DependencyObject dp) =>
        (bool)dp.GetValue(IsUpdatingProperty);

    private static void SetIsUpdating(DependencyObject dp, bool value) =>
        dp.SetValue(IsUpdatingProperty, value);

    private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            SetIsUpdating(passwordBox, true);
            SetBoundPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}