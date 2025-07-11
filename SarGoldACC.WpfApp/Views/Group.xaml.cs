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
        GroupNameBox.Focus();
    }
    private void AllPermissionsListBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            MoveToSelected_Click(sender, e);
        }
        if (e.Key == Key.Enter)
        {
            SelectedPermissionsListBox.Focus();
            e.Handled = true;
        }
    }
    
    private void SelectedPermissionsListBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Right)
        {
            MoveToAll_Click(sender, e);
        }
        if (e.Key == Key.Enter)
        {
            SaveButton.Focus();
            e.Handled = true;
        }
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
        else if (e.Key == Key.Enter && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && SaveButton.IsEnabled)
        {
            Save();
        }
        else if (e.Key == Key.F5)
        {
            ClearForm();
        }
    }

    private async void ClickSaveGroup(object sender, RoutedEventArgs e)
    {
        Save();
    }
    
    private void GroupDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        GroupDataGrid.DeleteActionShow = _viewModel.CanAccessGroupDelete;
        GroupDataGrid.EditActionShow = _viewModel.CanAccessGroupEdit;
        
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
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        ClearForm();
    }

    private async void ClearForm()
    {
        _viewModel.GroupName = "";
        _viewModel.GroupLabel = "";
        
        _viewModel.SelectedPermissions.Clear();
        _viewModel.AllPermissions.Clear();
        await _viewModel.LoadPermissionsAsync();
        _viewModel.Clear();
    }
    private async void Save()
    {
        if (!_viewModel.CanSave) return;
        await _viewModel.SaveGroup();
        ClearForm();
    }
}