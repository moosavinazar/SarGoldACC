using System.Windows;
using System.Windows.Controls;
using PersianDateControlsPlus.PersianDate;

namespace SarGoldACC.WpfApp.Control;

public partial class TextBoxDate : UserControl
{
    public TextBoxDate()
    {
        InitializeComponent();
    }
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label),
            typeof(string),
            typeof(TextBoxDate),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }
    public static readonly DependencyProperty DateProperty =
        DependencyProperty.Register(nameof(Date),
            typeof(PersianDate),
            typeof(TextBoxDate),
            new FrameworkPropertyMetadata(PersianDate.Today, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public PersianDate Date
    {
        get => (PersianDate)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }
}