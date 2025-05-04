using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.WpfApp.Controls;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Group : Window
{
    private readonly GroupViewModel _viewModel;
    private Point _dragStartPoint;

    public Group(GroupViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        GroupDataGrid.Initialize("GroupGrid");
        _viewModel.SetColumnsVisibilityAction = (showId, showName, showLabel) =>
        {
            /*IdColumn.Visibility = showId ? Visibility.Visible : Visibility.Collapsed;
            NameColumn.Visibility = showName ? Visibility.Visible : Visibility.Collapsed;
            LabelColumn.Visibility = showLabel ? Visibility.Visible : Visibility.Collapsed;*/
        };
        
        var allView = CollectionViewSource.GetDefaultView(_viewModel.AllPermissions);
        allView.SortDescriptions.Add(new SortDescription(nameof(PermissionDto.Label), ListSortDirection.Ascending));

        var selectedView = CollectionViewSource.GetDefaultView(_viewModel.SelectedPermissions);
        selectedView.SortDescriptions.Add(new SortDescription(nameof(PermissionDto.Label), ListSortDirection.Ascending));

        this.Focus();
    }
    
    private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _dragStartPoint = e.GetPosition(null);
    }
    
    private void ListBox_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var listBox = sender as ListBox;
            var mousePos = e.GetPosition(null);
            var diff = _dragStartPoint - mousePos;

            // بررسی جابه‌جایی حداقلی برای شروع Drag
            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                var selectedItems = listBox.SelectedItems.Cast<PermissionDto>().ToList();
                if (selectedItems.Any())
                {
                    DragDrop.DoDragDrop(listBox, selectedItems, DragDropEffects.Move);
                }
            }
        }
    }

    
    private void SelectedPermissionsListBox_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(typeof(List<PermissionDto>)) is List<PermissionDto> items)
        {
            foreach (var item in items.ToList())
            {
                if (_viewModel.AllPermissions.Contains(item))
                {
                    _viewModel.AllPermissions.Remove(item);
                    _viewModel.SelectedPermissions.Add(item);
                }
            }
        }
    }
    
    private void AllPermissionsListBox_Drop(object sender, DragEventArgs e)
    {
        
        if (e.Data.GetData(typeof(List<PermissionDto>)) is List<PermissionDto> items)
        {
            foreach (var item in items.ToList()) // استفاده از ToList برای جلوگیری از تغییر مجموعه در حین تکرار
            {
                if (_viewModel.SelectedPermissions.Contains(item))
                {
                    _viewModel.SelectedPermissions.Remove(item);
                    _viewModel.AllPermissions.Add(item);
                }
            }
        }
    }
    
    private void MoveToSelected_Click(object sender, RoutedEventArgs e)
    {
        var selectedItems = AllPermissionsListBox.SelectedItems.Cast<PermissionDto>().ToList();
        foreach (var item in selectedItems)
        {
            _viewModel.AllPermissions.Remove(item);
            _viewModel.SelectedPermissions.Add(item);
        }
    }

    private void MoveToAll_Click(object sender, RoutedEventArgs e)
    {
        var selectedItems = SelectedPermissionsListBox.SelectedItems.Cast<PermissionDto>().ToList();
        foreach (var item in selectedItems)
        {
            _viewModel.SelectedPermissions.Remove(item);
            _viewModel.AllPermissions.Add(item);
        }
    }


    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        
        //await _viewModel.LoadGridSettings();
        /*IdColumn.Visibility = _viewModel.ShowIdColumn ? Visibility.Visible : Visibility.Collapsed;
        NameColumn.Visibility = _viewModel.ShowNameColumn ? Visibility.Visible : Visibility.Collapsed;
        LabelColumn.Visibility = _viewModel.ShowLabelColumn ? Visibility.Visible : Visibility.Collapsed;*/
        //Keyboard.Focus(this);
    }

    private void GroupWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    
    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as GroupViewModel;
        if (viewModel == null) return;

        // فقط بار اول ستون‌ها را تعریف کن (برای جلوگیری از اضافه شدن مجدد)
        if (CustomDataGrid.Columns == null || CustomDataGrid.Columns.Count == 0)
        {
            CustomDataGrid.Columns = new ObservableCollection<DataGridColumn>()
            {
                new DataGridTextColumn
                {
                    Header = "نام گروه",
                    Binding = new Binding("Name")
                }
            };
        }

        CustomDataGrid.ItemsSource = viewModel.AllGroups;
        CustomDataGrid.Initialize("GroupsGrid");
    }
    
    /*private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        var dataGrid = sender as DataGrid;
        if (dataGrid.SelectedItem == null)
        {
            e.Handled = true; // غیرفعال کردن منو اگر ردیفی انتخاب نشده باشد
        }
    }*/

}