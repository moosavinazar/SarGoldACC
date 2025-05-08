using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class User : Window
{
    
    private readonly UserViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    private Point _dragStartPoint;
    
    public User(UserViewModel viewModel, IAuthorizationService authorizationService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authorizationService = authorizationService;
        DataContext = _viewModel;
        var allView = CollectionViewSource.GetDefaultView(_viewModel.AllGroups);
        allView.SortDescriptions.Add(new SortDescription(nameof(GroupDto.Label), ListSortDirection.Ascending));

        var selectedView = CollectionViewSource.GetDefaultView(_viewModel.SelectedGroups);
        selectedView.SortDescriptions.Add(new SortDescription(nameof(GroupDto.Label), ListSortDirection.Ascending));
        
        UserNameBox.Focus();
    }
    
    private void UserWindow_KeyDown(object sender, KeyEventArgs e)
    {
        
    }
    
    private void UserNameBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            PasswordBox.Focus();
            e.Handled = true;
        }
    }
    
    private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            ConfirmPasswordBox.Focus();
            e.Handled = true;
        }
    }
    
    private void PasswordConfirmBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            NameBox.Focus();
            e.Handled = true;
        }
    }
    
    private void NameBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            PhoneNumberBox.Focus();
            e.Handled = true;
        }
    }
    
    private void PhoneNumberBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            AllGroupsListBox.Focus();
            e.Handled = true;
        }
    }

    private void AllGroupsListBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            MoveToSelected_Click(sender, e);
        }
        if (e.Key == Key.Enter)
        {
            SelectedGroupsListBox.Focus();
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
                var selectedItems = listBox.SelectedItems.Cast<GroupDto>().ToList();
                if (selectedItems.Any())
                {
                    DragDrop.DoDragDrop(listBox, selectedItems, DragDropEffects.Move);
                }
            }
        }
    }

    private void AllGroupsListBox_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(typeof(List<GroupDto>)) is List<GroupDto> items)
        {
            foreach (var item in items.ToList()) // استفاده از ToList برای جلوگیری از تغییر مجموعه در حین تکرار
            {
                if (_viewModel.SelectedGroups.Contains(item))
                {
                    _viewModel.SelectedGroups.Remove(item);
                    _viewModel.AllGroups.Add(item);
                }
            }
        }
    }

    private void MoveToSelected_Click(object sender, RoutedEventArgs e)
    {
        var selectedItems = AllGroupsListBox.SelectedItems.Cast<GroupDto>().ToList();
        foreach (var item in selectedItems)
        {
            _viewModel.AllGroups.Remove(item);
            _viewModel.SelectedGroups.Add(item);
        }
    }

    private void MoveToAll_Click(object sender, RoutedEventArgs e)
    {
        var selectedItems = SelectedGroupsListBox.SelectedItems.Cast<GroupDto>().ToList();
        foreach (var item in selectedItems)
        {
            _viewModel.SelectedGroups.Remove(item);
            _viewModel.AllGroups.Add(item);
        }
    }

    private void SelectedGroupsListBox_KeyDown(object sender, KeyEventArgs e)
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

    private void SelectedGroupsListBox_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(typeof(List<GroupDto>)) is List<GroupDto> items)
        {
            foreach (var item in items.ToList())
            {
                if (_viewModel.AllGroups.Contains(item))
                {
                    _viewModel.AllGroups.Remove(item);
                    _viewModel.SelectedGroups.Add(item);
                }
            }
        }
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
    }
    
    private async void ClickSaveGroup(object sender, RoutedEventArgs e)
    {
        if (PasswordBox.Password != ConfirmPasswordBox.Password)
        {
            MessageBoxHelper.ShowError("رمز عبور با تکرار آن مطابقت ندارد");
            return;
        }
        await _viewModel.SaveUser();
        UserNameBox.Text = "";
        PasswordBox.Password = "";
        ConfirmPasswordBox.Password = "";
        NameBox.Text = "";
        PhoneNumberBox.Text = "";
        _viewModel.SelectedBranchId = _authorizationService.GetCurrentUser().BranchId;
        
        _viewModel.SelectedGroups.Clear();
        _viewModel.AllGroups.Clear();
        await _viewModel.LoadGroupsAsync();
    }

    private void UserDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        UserDataGrid.DeleteActionShow = _viewModel.CanAccessUserDelete;
        UserDataGrid.EditActionShow = _viewModel.CanAccessUserEdit;
        
        UserDataGrid.DeleteAction = async obj =>
        {
            if (obj is UserDto user)
            {
                await _viewModel.DeleteAsync(user.Id);
            }
        };
        
        UserDataGrid.EditAction = async obj =>
        {
            if (obj is UserDto user)
            {
                await _viewModel.EditAsync(user.Id);
            }
        };
        
        UserDataGrid.ColumnConfigKey = $"UserGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        UserDataGrid.SetColumns(
            new DataGridTextColumn { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام کاربری", Binding = new Binding("Username"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام", Binding = new Binding("Name"), Width = new DataGridLength(7, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "شماره تماس", Binding = new Binding("PhoneNumber"), Width = new DataGridLength(7, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "شعبه", Binding = new Binding("BranchName"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) }
        );
    }
}