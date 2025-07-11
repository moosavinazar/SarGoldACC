using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using SarGoldACC.Core.DTOs.Laboratory;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Laboratory : Window
{
    private readonly LaboratoryViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    public Laboratory(LaboratoryViewModel viewModel, IAuthorizationService authorizationService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _authorizationService = authorizationService;
    }
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
    }
    private void LaboratoryWindow_KeyDown(object sender, KeyEventArgs e)
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
    private async void ClickSaveLaboratory(object sender, RoutedEventArgs e)
    {
        Save();
    }
    private void LaboratoryDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        LaboratoryDataGrid.EditActionShow = _viewModel.CanAccessLaboratoryEdit;

        LaboratoryDataGrid.EditAction = async obj =>
        {
            if (obj is LaboratoryDto laboratory)
            {
                await _viewModel.EditAsync(laboratory.Id);
            }
        };
        
        LaboratoryDataGrid.ColumnConfigKey = $"LaboratoryGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        LaboratoryDataGrid.SetColumns(
            new DataGridTextColumn() { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام", Binding = new Binding("Name"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "تلفن", Binding = new Binding("Phone"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "موبایل", Binding = new Binding("CellPhone"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "تلفن گویا", Binding = new Binding("IVRPhone"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "توضیحات", Binding = new Binding("Description"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
    private void CitySelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.CityId == 0)
        {
            _viewModel.CityId = 1;
        }
    }
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        ClearForm();
    }
    private async void Save()
    {
        if (!_viewModel.CanSave) return;
        await _viewModel.SaveLaboratory();
        ClearForm();
    }
    private void ClearForm()
    {
        _viewModel.Name = "";
        _viewModel.CellPhone = "";
        _viewModel.IVRPhone = "";
        _viewModel.CityId = 1;
        _viewModel.Phone = "";
        _viewModel.WeightBed = 0;
        _viewModel.WeightBes = 0;
        _viewModel.RiyalBed = 0;
        _viewModel.RiyalBes = 0;
        _viewModel.Description = "";
        _viewModel.PhotoPreview = null;
        _viewModel.PhotoFileName = "";
        _viewModel.PhotoBytes = null;
        _viewModel.Clear();
        
    }
    private void ChoosePhotoButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string selectedFilePath = openFileDialog.FileName;

            // بارگذاری تصویر برای پیش‌نمایش
            var bitmap = new BitmapImage(new Uri(selectedFilePath));
            LaboratoryViewModel vm = (LaboratoryViewModel)this.DataContext;
            vm.PhotoPreview = bitmap;

            // خواندن فایل به صورت byte[]
            byte[] fileBytes = File.ReadAllBytes(selectedFilePath);

            // ذخیره در ViewModel
            vm.PhotoBytes = fileBytes;
            vm.PhotoFileName = Path.GetFileName(selectedFilePath);
        }
    }
}