using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.Control;

public partial class CustomizableDataGrid : UserControl
{
    public ObservableCollection<DataGridColumn> ConfigurableColumns { get; } = new();

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(CustomizableDataGrid), new PropertyMetadata(null, OnItemsSourceChanged));

    public object ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    
    public static readonly DependencyProperty DeleteActionProperty =
        DependencyProperty.Register(nameof(DeleteAction), typeof(Func<object, Task>), typeof(CustomizableDataGrid));

    public Func<object, Task>? DeleteAction
    {
        get => (Func<object, Task>?)GetValue(DeleteActionProperty);
        set => SetValue(DeleteActionProperty, value);
    }
    
    public static readonly DependencyProperty EditActionProperty =
        DependencyProperty.Register(nameof(EditAction), typeof(Func<object, Task>), typeof(CustomizableDataGrid));

    public Func<object, Task>? EditAction
    {
        get => (Func<object, Task>?)GetValue(EditActionProperty);
        set => SetValue(EditActionProperty, value);
    }
    
    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CustomizableDataGrid ctrl && ctrl.MainDataGrid != null)
        {
            ctrl.MainDataGrid.ItemsSource = e.NewValue as IEnumerable;
        }
    }

    public string ColumnConfigKey { get; set; } = "Default";

    private readonly string configFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SarGoldAcc", "GridConfig");

    public CustomizableDataGrid()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (!Directory.Exists(configFolder))
            Directory.CreateDirectory(configFolder);

        LoadColumnVisibility();
    }

    public void SetColumns(params DataGridColumn[] columns)
    {
        MainDataGrid.Columns.Clear();
        ConfigurableColumns.Clear();

        foreach (var col in columns)
        {
            MainDataGrid.Columns.Add(col);
            ConfigurableColumns.Add(col);
        }
        
        var deleteButtonColumn = new DataGridTemplateColumn
        {
            Header = "حذف",
            CellTemplate = (DataTemplate)Resources["DeleteButtonTemplate"]
        };
        
        var editButtonColumn = new DataGridTemplateColumn
        {
            Header = "ویرایش",
            CellTemplate = (DataTemplate)Resources["EditButtonTemplate"]
        };

        MainDataGrid.Columns.Add(editButtonColumn);
        MainDataGrid.Columns.Add(deleteButtonColumn);

        LoadColumnVisibility(); // Apply saved visibility
    }

    private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        var ctxMenu = new ContextMenu();
        foreach (var col in ConfigurableColumns)
        {
            var item = new MenuItem
            {
                Header = col.Header?.ToString(),
                IsCheckable = true,
                IsChecked = col.Visibility == Visibility.Visible
            };
            item.Checked += (_, _) => { col.Visibility = Visibility.Visible; SaveColumnVisibility(); };
            item.Unchecked += (_, _) => { col.Visibility = Visibility.Collapsed; SaveColumnVisibility(); };
            ctxMenu.Items.Add(item);
        }
        MainDataGrid.ContextMenu = ctxMenu;
    }

    private void SaveColumnVisibility()
    {
        var config = ConfigurableColumns
            .ToDictionary(c => c.Header?.ToString() ?? "", c => c.Visibility == Visibility.Visible);
        var json = JsonSerializer.Serialize(config);
        File.WriteAllText(Path.Combine(configFolder, ColumnConfigKey + ".json"), json);
    }

    private void LoadColumnVisibility()
    {
        string path = Path.Combine(configFolder, ColumnConfigKey + ".json");
        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);
        var config = JsonSerializer.Deserialize<Dictionary<string, bool>>(json);
        if (config == null) return;

        foreach (var col in ConfigurableColumns)
        {
            var header = col.Header?.ToString() ?? "";
            if (config.TryGetValue(header, out var isVisible))
                col.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
    
    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBoxHelper.ShowDeleteConfirm("آیا از حذف این مورد مطمئن هستید؟");
        if (!result) return;
        if (sender is Button button && button.DataContext is object item)
        {
            if (DeleteAction != null)
            {
                await DeleteAction(item);
            }

            if (ItemsSource is IList list)
            {
                list.Remove(item); // حذف از رابط کاربری
            }
        }
    }

    private async void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is object item)
        {
            if (EditAction != null)
            {
                await EditAction(item);
            }
        }
    }
}
