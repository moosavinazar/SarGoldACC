using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.Controls;

public partial class CustomDataGrid : UserControl
{
    private readonly GridSettingsFileService _settingsService;
    private string _gridName;

    public CustomDataGrid()
    {
        InitializeComponent();
        MainDataGrid.Tag = this;
        ColumnSettings = new ObservableCollection<ColumnSetting>();
        _settingsService = new GridSettingsFileService("default_user"); // جایگزین با userId واقعی
        SetValue(ColumnsProperty, new ObservableCollection<DataGridColumn>()); // مقداردهی اولیه Columns
    }

    // Dependency Property برای ItemsSource
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(CustomDataGrid),
            new PropertyMetadata(null, OnItemsSourceChanged));

    public object ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    // Dependency Property برای Columns
    public static readonly DependencyProperty ColumnsProperty =
        DependencyProperty.Register(nameof(Columns), typeof(ObservableCollection<DataGridColumn>), typeof(CustomDataGrid),
            new PropertyMetadata(null, OnColumnsChanged));

    public ObservableCollection<DataGridColumn> Columns
    {
        get => (ObservableCollection<DataGridColumn>)GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    // مجموعه تنظیمات ستون‌ها
    public ObservableCollection<ColumnSetting> ColumnSettings { get; }

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (CustomDataGrid)d;
        control.MainDataGrid.ItemsSource = e.NewValue as IEnumerable;
    }

    private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (CustomDataGrid)d;
        control.UpdateColumns();
    }

    private void UpdateColumns()
    {
        MainDataGrid.Columns.Clear();
        ColumnSettings.Clear();

        if (Columns == null) return;

        foreach (var column in Columns)
        {
            MainDataGrid.Columns.Add(column);
            var setting = new ColumnSetting
            {
                DisplayName = column.Header?.ToString() ?? string.Empty,
                BindingPath = (column as DataGridBoundColumn)?.Binding is Binding binding ? binding.Path.Path : string.Empty,
                IsVisible = column.Visibility == Visibility.Visible
            };
            setting.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(ColumnSetting.IsVisible))
                {
                    column.Visibility = setting.IsVisible ? Visibility.Visible : Visibility.Collapsed;
                    await SaveGridSettings();
                }
            };
            ColumnSettings.Add(setting);
        }

        _ = LoadGridSettings();
    }

    public async Task LoadGridSettings()
    {
        var all = await _settingsService.LoadSettingsAsync();
        var setting = all.FirstOrDefault(s => s.GridName == _gridName);
        if (setting == null) return;

        foreach (var colSetting in ColumnSettings)
        {
            if (setting.ColumnVisibility.TryGetValue(colSetting.BindingPath, out var isVisible))
            {
                colSetting.IsVisible = isVisible;
                var column = MainDataGrid.Columns.FirstOrDefault(c =>
                    (c as DataGridBoundColumn)?.Binding is Binding b && b.Path.Path == colSetting.BindingPath);
                if (column != null)
                    column.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }

    private async Task SaveGridSettings()
    {
        var all = await _settingsService.LoadSettingsAsync();
        var setting = all.FirstOrDefault(s => s.GridName == _gridName);
        if (setting == null)
        {
            setting = new GridColumnSetting { GridName = _gridName };
            all.Add(setting);
        }

        foreach (var colSetting in ColumnSettings)
        {
            setting.ColumnVisibility[colSetting.BindingPath] = colSetting.IsVisible;
        }

        await _settingsService.SaveSettingsAsync(all);
    }

    private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        if (MainDataGrid.SelectedItem == null)
        {
            e.Handled = true;
        }
    }

    public void Initialize(string gridName)
    {
        _gridName = gridName;
        if (Columns == null)
        {
            SetValue(ColumnsProperty, new ObservableCollection<DataGridColumn>());
        }
        UpdateColumns();
    }
}

// مدل تنظیمات ستون
public class ColumnSetting : INotifyPropertyChanged
{
    private bool _isVisible = true;
    public string DisplayName { get; set; }
    public string BindingPath { get; set; }

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible != value)
            {
                _isVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsVisible)));
            }
        }
    }
    
    private void GroupDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        // در صورت نیاز اینجا کدی بنویسید
    }


    public event PropertyChangedEventHandler PropertyChanged;
}