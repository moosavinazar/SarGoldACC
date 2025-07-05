using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace SarGoldACC.WpfApp.Control;

public partial class ComboBoxSelector : UserControl
{
    public ComboBoxSelector()
    {
        InitializeComponent();
    }

    public IServiceProvider ServiceProvider { get; set; }

    // ItemsSource (لیست کلی آیتم‌ها)
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(ComboBoxSelector),
            new PropertyMetadata(null));

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    // SelectedValue (مقدار انتخاب شده - معمولاً Id)
    public static readonly DependencyProperty SelectedValueProperty =
        DependencyProperty.Register(nameof(SelectedValue),
            typeof(object),
            typeof(ComboBoxSelector),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    // ComboBoxSelector.xaml.cs
    public static readonly DependencyProperty AddWindowTypeProperty =
        DependencyProperty.Register(nameof(AddWindowType),
            typeof(Type),
            typeof(ComboBoxSelector),
            new PropertyMetadata(null));

    public Type AddWindowType
    {
        get => (Type)GetValue(AddWindowTypeProperty);
        set => SetValue(AddWindowTypeProperty, value);
    }
    
    public object SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    // DisplayMemberPath (نام فیلد نمایشی - مثلاً "Name")
    public static readonly DependencyProperty DisplayMemberPathProperty =
        DependencyProperty.Register(nameof(DisplayMemberPath),
            typeof(string),
            typeof(ComboBoxSelector),
            new PropertyMetadata("Name"));

    public string DisplayMemberPath
    {
        get => (string)GetValue(DisplayMemberPathProperty);
        set => SetValue(DisplayMemberPathProperty, value);
    }

    // SelectedValuePath (نام فیلدی که مقدار اصلی را مشخص می‌کند - مثلاً "Id")
    public static readonly DependencyProperty SelectedValuePathProperty =
        DependencyProperty.Register(nameof(SelectedValuePath),
            typeof(string),
            typeof(ComboBoxSelector),
            new PropertyMetadata("Id"));

    public string SelectedValuePath
    {
        get => (string)GetValue(SelectedValuePathProperty);
        set => SetValue(SelectedValuePathProperty, value);
    }

    // SearchText (متن فیلتر یا جستجو)
    public static readonly DependencyProperty SearchTextProperty =
        DependencyProperty.Register(nameof(SearchText),
            typeof(string),
            typeof(ComboBoxSelector),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string SearchText
    {
        get => (string)GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }

    // آیا دکمه + نمایش داده شود؟
    public static readonly DependencyProperty CanAccessAddButtonProperty =
        DependencyProperty.Register(nameof(CanAccessAddButton),
            typeof(bool),
            typeof(ComboBoxSelector),
            new PropertyMetadata(false));

    public bool CanAccessAddButton
    {
        get => (bool)GetValue(CanAccessAddButtonProperty);
        set => SetValue(CanAccessAddButtonProperty, value);
    }

    // رویداد جستجو یا باز کردن لیست
    private void CustomComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (sender is ComboBox comboBox)
        {
            comboBox.IsDropDownOpen = true;
        }
    }

    private void CustomComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Insert)
        {
            OpenWindow();
        }
    }

    private void ClickAdd(object sender, RoutedEventArgs e)
    {
        OpenWindow();
    }

    private void OpenWindow()
    {
        if (ServiceProvider == null || AddWindowType == null) return;

        // از DI پنجرهٔ درخواستی را می‌گیریم
        var win = ServiceProvider.GetService(AddWindowType) as Window;
        win?.ShowDialog();
    }

}
