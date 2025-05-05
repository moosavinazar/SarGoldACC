using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

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
}
