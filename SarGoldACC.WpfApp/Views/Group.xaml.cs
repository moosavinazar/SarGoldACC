using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Group : Window
{
    public Group()
    {
        InitializeComponent();
        this.Focus();
    }
    
    private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var listBox = sender as ListBox;
        var item = (listBox?.SelectedItem as Permission);
        if (item != null)
        {
            DragDrop.DoDragDrop(listBox, item, DragDropEffects.Move);
        }
    }

    private void SelectedPermissionsListBox_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(Permission)))
        {
            var permission = e.Data.GetData(typeof(Permission)) as Permission;
            var viewModel = (GroupViewModel)DataContext;

            if (permission != null && !viewModel.SelectedPermissions.Contains(permission))
            {
                viewModel.SelectedPermissions.Add(permission);
                viewModel.AllPermissions.Remove(permission);
            }
        }
    }

    private void AllPermissionsListBox_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(Permission)))
        {
            var permission = e.Data.GetData(typeof(Permission)) as Permission;
            var viewModel = (GroupViewModel)DataContext;

            if (permission != null && !viewModel.AllPermissions.Contains(permission))
            {
                viewModel.AllPermissions.Add(permission);
                viewModel.SelectedPermissions.Remove(permission);
            }
        }
    }
    
    private void Window_Loaded(object sender, RoutedEventArgs e)
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

}