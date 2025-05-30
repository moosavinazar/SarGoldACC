using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.MadeCategory;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class MadeSubCategory : Window
{
    private readonly MadeSubCategoryViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    private readonly IServiceProvider _serviceProvider;
    
    public MadeSubCategory(MadeSubCategoryViewModel viewModel, 
        IAuthorizationService authorizationService, IServiceProvider serviceProvide)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authorizationService = authorizationService;
        DataContext = _viewModel;
        _serviceProvider = serviceProvide;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }

    private async void ClickSaveMadeSubCategory(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveMadeSubCategory();
        MadeSubCategoryNameBox.Text = "";
    }
    
    private async void ClickAddMadeCategory(object sender, RoutedEventArgs e)
    {
        var madeCategoryWindow = _serviceProvider.GetRequiredService<MadeCategory>();
        madeCategoryWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        madeCategoryWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }

    private void MadeSubCategoryDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        MadeSubCategoryDataGrid.DeleteActionShow = _viewModel.CanAccessMadeSubCategoryDelete;
        MadeSubCategoryDataGrid.EditActionShow = _viewModel.CanAccessMadeSubCategoryEdit;
        
        MadeSubCategoryDataGrid.DeleteAction = async obj =>
        {
            if (obj is MadeSubCategoryDto madeSubCategory)
            {
                await _viewModel.DeleteAsync(madeSubCategory.Id);
            }
        };
        
        MadeSubCategoryDataGrid.EditAction = async obj =>
        {
            if (obj is MadeSubCategoryDto madeSubCategory)
            {
                await _viewModel.EditAsync(madeSubCategory.Id);
            }
        };
        
        MadeSubCategoryDataGrid.ColumnConfigKey = $"MadeSubCategoryGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        MadeSubCategoryDataGrid.SetColumns(
            new DataGridTextColumn() { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "دسته بندی", Binding = new Binding("Name"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
}