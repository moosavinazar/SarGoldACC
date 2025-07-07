using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SarGoldACC.WpfApp.Control;

public partial class TextBoxValidate : UserControl, IDataErrorInfo
{
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
    public TextBoxValidate()
    {
        InitializeComponent();
        ValidTextBox.AddHandler(CommandManager.PreviewExecutedEvent,
            new ExecutedRoutedEventHandler(TextBox_PreviewExecuted), true);
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

                if (!Regex.IsMatch(ValidText, @ValidTextFinalPattern))
                    return NotValidTextMessage;
            }
            return null;
        }
    }
    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(ValidTextBox);
        ValidTextBox.SelectAll();

        string langCode = InputLanguage switch
        {
            InputLanguageEnum.Persian => "00000429",
            InputLanguageEnum.English => "00000409",
            _ => "00000409"
        };
        string langString = InputLanguage switch
        {
            InputLanguageEnum.Persian => "fa-IR",
            InputLanguageEnum.English => "en-US",
            _ => "en-US"
        };
        LoadKeyboardLayout(langCode, 1);
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo(langString);
    }
    private void TextBox_Loaded(object sender, RoutedEventArgs e)
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
    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @ValidTextPattern);
    }
    private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Command == ApplicationCommands.Paste)
        {
            if (Clipboard.ContainsText())
            {
                string pasteText = Clipboard.GetText();
                if (!Regex.IsMatch(pasteText, @ValidTextPattern))
                {
                    e.Handled = true;
                }
            }
        }
    }
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label),
            typeof(string),
            typeof(TextBoxValidate),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
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
    public static readonly DependencyProperty ValidTextFinalPatternProperty =
        DependencyProperty.Register(nameof(ValidTextFinalPattern),
            typeof(string),
            typeof(TextBoxValidate),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string ValidTextFinalPattern
    {
        get => (string)GetValue(ValidTextFinalPatternProperty);
        set => SetValue(ValidTextFinalPatternProperty, value);
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
    public static readonly DependencyProperty InputLanguageProperty =
        DependencyProperty.Register(nameof(InputLanguage), typeof(InputLanguageEnum), typeof(TextBoxValidate),
            new PropertyMetadata(InputLanguageEnum.English));

    public InputLanguageEnum InputLanguage
    {
        get => (InputLanguageEnum)GetValue(InputLanguageProperty);
        set => SetValue(InputLanguageProperty, value);
    }
}
public enum InputLanguageEnum
{
    English,
    Persian
}