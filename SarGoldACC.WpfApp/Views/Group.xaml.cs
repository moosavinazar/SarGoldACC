using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Group : Window
{
    private readonly GroupViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    private Point _dragStartPoint;

    public Group(GroupViewModel viewModel, IAuthorizationService authorizationService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authorizationService = authorizationService;
        DataContext = _viewModel;
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
        Keyboard.Focus(this);
    }

    private void GroupWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    
    private void GroupDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        GroupDataGrid.DeleteAction = async obj =>
        {
            if (obj is GroupDto group)
            {
                await _viewModel.DeleteAsync(group.Id);
            }
        };
        
        GroupDataGrid.EditAction = async obj =>
        {
            if (obj is GroupDto group)
            {
                await _viewModel.EditAsync(group.Id);
            }
        };
        
        GroupDataGrid.ColumnConfigKey = $"GroupGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        GroupDataGrid.SetColumns(
            new DataGridTextColumn { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "گروه", Binding = new Binding("Name"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام گروه", Binding = new Binding("Label"), Width = new DataGridLength(7, DataGridLengthUnitType.Star) }
        );
    }


}