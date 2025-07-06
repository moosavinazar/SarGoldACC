using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SarGoldACC.WpfApp.Control;

public partial class TextBoxValidDecimal : UserControl, IDataErrorInfo
{
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
    public TextBoxValidDecimal()
    {
        InitializeComponent();
    }
    // IDataErrorInfo
    public string Error => null;
    public string this[string columnName]
    {
        get
        {
            if (columnName == nameof(ValidText))
            {
                if (!AllowNullText && string.IsNullOrWhiteSpace(ValidText))
                    return NotValidTextMessage;

                if (!Regex.IsMatch(ValidText, @ValidTextPattern))
                    return NotValidTextMessage;
            }
            return null;
        }
    }
    private void NameBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(ValidTextBox);
        ValidTextBox.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void NameBox_Loaded(object sender, RoutedEventArgs e)
    {
        DependencyPropertyDescriptor
            .FromProperty(Validation.HasErrorProperty, typeof(TextBox))
            .AddValueChanged(ValidTextBox, (s, args) =>
            {
                var tb = s as TextBox;
                if (Validation.GetHasError(tb))
                {
                    ToolTip tt = new ToolTip
                    {
                        Content = Validation.GetErrors(tb)[0].ErrorContent,
                        IsOpen = true,
                        PlacementTarget = tb,
                        StaysOpen = true,
                        Placement = System.Windows.Controls.Primitives.PlacementMode.Right
                    };
                    tb.ToolTip = tt;
                }
                else
                {
                    if (tb.ToolTip is ToolTip ttip)
                        ttip.IsOpen = false;
                }
            });
    }
    private void NameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @ValidTextPattern);
    }
    public static readonly DependencyProperty ValidTextProperty =
        DependencyProperty.Register(nameof(ValidText),
            typeof(string),
            typeof(TextBoxValidate),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string ValidText
    {
        get => (string)GetValue(ValidTextProperty);
        set => SetValue(ValidTextProperty, value);
    }
    public static readonly DependencyProperty ValidTextPatternProperty =
        DependencyProperty.Register(nameof(ValidTextPattern),
            typeof(string),
            typeof(TextBoxValidate),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string ValidTextPattern
    {
        get => (string)GetValue(ValidTextPatternProperty);
        set => SetValue(ValidTextPatternProperty, value);
    }
    public static readonly DependencyProperty NotValidTextMessageProperty =
        DependencyProperty.Register(nameof(NotValidTextMessage),
            typeof(string),
            typeof(TextBoxValidate),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string NotValidTextMessage
    {
        get => (string)GetValue(NotValidTextMessageProperty);
        set => SetValue(NotValidTextMessageProperty, value);
    }
    public static readonly DependencyProperty AllowNullTextProperty =
        DependencyProperty.Register(nameof(AllowNullText),
            typeof(bool),
            typeof(TextBoxValidate),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public bool AllowNullText
    {
        get => (bool)GetValue(AllowNullTextProperty);
        set => SetValue(AllowNullTextProperty, value);
    }
}