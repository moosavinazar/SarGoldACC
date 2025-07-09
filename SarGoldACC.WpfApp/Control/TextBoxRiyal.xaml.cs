using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SarGoldACC.WpfApp.Control;

public partial class TextBoxRiyal : UserControl
{
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
    private bool _isUpdatingTextManually = false;
    public TextBoxRiyal()
    {
        InitializeComponent();
        ValidTextBox.Text = "0";
    }
    private void NameBox_GotFocus(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(ValidTextBox);
        ValidTextBox.Text = RemoveSeparators(ValidTextBox.Text);
        if (ValidTextBox.Text == "0")
            ValidTextBox.Text = "";
        else
            ValidTextBox.SelectAll();

        // زبان انگلیسی
        LoadKeyboardLayout("00000409", 1);
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }

    private void NameBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ValidTextBox.Text))
        {
            ValidTextBox.Text = "0";
        }
        // مقدار خام در بایند
        ValidText = RemoveSeparators(ValidTextBox.Text);
        // اعمال جداکننده در نمایش
        if (long.TryParse(ValidText, out var number))
        {
            ValidTextBox.Text = number.ToString("N0", CultureInfo.InvariantCulture);
        }
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
                        Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom
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
        var textBox = sender as TextBox;
        if (textBox == null) return;

        string currentText = textBox.Text;
        int caretIndex = textBox.CaretIndex;

        // اگر کاراکتر عدد باشد
        if (Regex.IsMatch(e.Text, "[0-9]"))
        {
            e.Handled = false; // عدد مجاز است
        }
        else if (e.Text == ".")
        {
            // بررسی وجود عدد قبل از مکان درج
            bool hasDigitBefore = caretIndex > 0 && char.IsDigit(currentText[caretIndex - 1]);

            if (hasDigitBefore)
            {
                textBox.Text = currentText.Insert(caretIndex, "000");
                textBox.CaretIndex = caretIndex + 3;
            }

            e.Handled = true; // جلو ورود خود نقطه گرفته شود
        }
        else
        {
            // هر چیز غیر از عدد و نقطه رد شود
            e.Handled = true;
        }
    }
    public static readonly DependencyProperty ValidTextProperty =
        DependencyProperty.Register(nameof(ValidText),
            typeof(string),
            typeof(TextBoxRiyal),
            new FrameworkPropertyMetadata(string.Empty, 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                OnValidTextChanged));
    private static void OnValidTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBoxRiyal control && e.NewValue is string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                control.ValidTextBox.Text = "0";
            }
            else if (long.TryParse(newValue.Replace(",", ""), out var number))
            {
                control.ValidTextBox.Text = number.ToString("N0", CultureInfo.InvariantCulture);
            }
            else
            {
                control.ValidTextBox.Text = newValue;
            }
        }
    }
    public string ValidText
    {
        get => (string)GetValue(ValidTextProperty);
        set => SetValue(ValidTextProperty, value);
    }
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label),
            typeof(string),
            typeof(TextBoxRiyal),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }
    private string RemoveSeparators(string input)
    {
        return input.Replace(",", "");
    }
    private void ValidTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdatingTextManually) return;

        var textBox = sender as TextBox;
        if (textBox == null) return;

        string rawText = RemoveSeparators(textBox.Text);
        if (string.IsNullOrWhiteSpace(rawText)) return;

        if (long.TryParse(rawText, out var value))
        {
            _isUpdatingTextManually = true;

            int caretIndex = textBox.CaretIndex;
            textBox.Text = value.ToString("N0", CultureInfo.InvariantCulture);

            // قرار دادن caret در انتها (یا تنظیم مجدد اگر نیاز بود دقیق‌تر)
            textBox.CaretIndex = textBox.Text.Length;

            ValidText = value.ToString(); // مقدار بدون کاما برای بایند

            _isUpdatingTextManually = false;
        }
    }
}