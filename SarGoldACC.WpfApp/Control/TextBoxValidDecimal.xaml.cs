using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SarGoldACC.WpfApp.Control;

public partial class TextBoxValidDecimal : UserControl
{
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
    public TextBoxValidDecimal()
    {
        InitializeComponent();
    }
    private void NameBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (ValidTextBox.Text == "")
        {
            ValidTextBox.Text = "0";
        }
    }
    private void NameBox_KeyDown(object sender, RoutedEventArgs routedEventArgs)
    {
        
    }
    private void NameBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(ValidTextBox);
        if (ValidTextBox.Text == "0")
        {
            ValidTextBox.Text = string.Empty;
        }
        else
        {
            ValidTextBox.SelectAll();
        }
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

        // متن نهایی اگر ورودی اعمال شود
        string proposedText = currentText.Insert(caretIndex, e.Text);

        // اگر کاراکتر عدد باشد
        if (Regex.IsMatch(e.Text, "[0-9]"))
        {
            int dotIndex = currentText.IndexOf('.');
            if (dotIndex >= 0)
            {
                // اگر نقطه وجود دارد، تعداد رقم بعدش رو بررسی کن
                int digitsAfterDecimal = currentText.Substring(dotIndex + 1).Length;

                // اگر کاربر در بخش اعشاری تایپ می‌کند
                if (caretIndex > dotIndex)
                {
                    if (digitsAfterDecimal >= 3)
                    {
                        e.Handled = true; // بیشتر از ۳ رقم اجازه نداره
                        return;
                    }
                }
            }

            e.Handled = false; // عدد مجاز است
        }
        else if (e.Text == ".")
        {
            // فقط یک نقطه مجاز است
            if (currentText.Contains("."))
            {
                e.Handled = true;
            }
            else
            {
                // اگر قبل از نقطه عددی نیست، 0. بنویسه
                if (string.IsNullOrEmpty(currentText) || caretIndex == 0)
                {
                    textBox.Text = currentText.Insert(caretIndex, "0.");
                    textBox.CaretIndex = caretIndex + 2;
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
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
            typeof(TextBoxValidDecimal),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string ValidText
    {
        get => (string)GetValue(ValidTextProperty);
        set => SetValue(ValidTextProperty, value);
    }
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label),
            typeof(string),
            typeof(TextBoxValidDecimal),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }
}