using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace SarGoldACC.WpfApp.Control;

public partial class ComboBoxSelector : UserControl
{
    public IServiceProvider ServiceProvider { get; set; }
    private List<object> _filteredItems = new();
    private bool _isUserTyping = false;
    
    public ComboBoxSelector()
    {
        InitializeComponent();

        CustomComboBox.Loaded += (s, e) =>
        {
            Dispatcher.InvokeAsync(() =>
            {
                var textBox = (TextBox)CustomComboBox.Template.FindName("PART_EditableTextBox", CustomComboBox);
                if (textBox != null)
                {
                    /*textBox.TextChanged += (s2, e2) =>
                    {
                        // به جای SetValue مستقیم، از setter استفاده کن
                        SearchText = textBox.Text;
                        CustomComboBox.IsDropDownOpen = _filteredItems.Any();
                    };*/
                    textBox.TextChanged += (s2, e2) =>
                    {
                        if (_isUserTyping)
                        {
                            SearchText = textBox.Text;
                        }
                    };
                    textBox.GotFocus += (s3, e3) =>
                    {
                        _isUserTyping = true;
                    };
                }

                FilterItems(); // بار اول فیلتر
            });
        };
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(ComboBoxSelector),
            new PropertyMetadata(null, OnItemsSourceChanged));

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ComboBoxSelector selector)
        {
            selector.FilterItems();
        }
    }
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
        set
        {
            SetValue(SearchTextProperty, value);
            FilterItems();

            if (_isUserTyping)
            {
                CustomComboBox.IsDropDownOpen = _filteredItems.Any();
            }
        }
    }
    private void FilterItems()
    {
        if (ItemsSource == null)
            return;

        string search = SearchText?.Trim() ?? "";

        var items = ItemsSource.Cast<object>();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var filtered = items
                .Where(item =>
                {
                    var prop = item.GetType().GetProperty(DisplayMemberPath);
                    if (prop == null) return false;
                    var value = prop.GetValue(item)?.ToString();
                    return value != null && value.Contains(search, StringComparison.OrdinalIgnoreCase);
                })
                .ToList();

            _filteredItems = filtered;
        }
        else
        {
            _filteredItems = items.ToList();
        }

        CustomComboBox.ItemsSource = null;
        CustomComboBox.ItemsSource = _filteredItems;

        // ✅ فقط اگر مقدار دقیقاً برابر بود، انتخاب کن
        if (_filteredItems.Count == 1)
        {
            var singleItem = _filteredItems[0];
            var displayProp = singleItem.GetType().GetProperty(DisplayMemberPath);
            var value = displayProp?.GetValue(singleItem)?.ToString();

            if (!string.IsNullOrWhiteSpace(value) && value.Equals(search, StringComparison.OrdinalIgnoreCase))
            {
                var selectedProp = singleItem.GetType().GetProperty(SelectedValuePath);
                if (selectedProp != null)
                {
                    SelectedValue = selectedProp.GetValue(singleItem);
                    CustomComboBox.IsDropDownOpen = false;
                }
            }
        }
    }

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label),
            typeof(string),
            typeof(ComboBoxSelector),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
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
