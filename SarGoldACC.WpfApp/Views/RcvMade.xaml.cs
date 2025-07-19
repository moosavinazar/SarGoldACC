using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Enums;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class RcvMade : Window
{
    private readonly RcvMadeViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;
    public DocumentItemDto ResultItem { get; private set; }
    
    public RcvMade(RcvMadeViewModel viewModel, IServiceProvider serviceProvide)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _serviceProvider = serviceProvide;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
        MadeSubCategoryComboBox.ServiceProvider = _serviceProvider;
        BoxComboBox.ServiceProvider = _serviceProvider;
        await ReloadListsAsync();
    }
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    private bool _isFirstActivation = true;
    private async void Window_Activated(object sender, EventArgs e)
    {
        if (_isFirstActivation)
        {
            _isFirstActivation = false;
            return; // بار اول هنگام Load انجام شده است
        }

        await ReloadListsAsync();
    }
    private async Task ReloadListsAsync()
    {
        await _viewModel.ReloadAllAsync();
    }
    private void ClickSave(object sender, RoutedEventArgs e)
    {
        ResultItem = new DocumentItemDto
        {
            WeightBes = (DataContext as RcvMadeViewModel).Weight,
            Weight = (DataContext as RcvMadeViewModel).Weight,
            Weight750 = (DataContext as RcvMadeViewModel).Weight750,
            Ayar = (DataContext as RcvMadeViewModel).Ayar,
            Barcode = (DataContext as RcvMadeViewModel).Barcode,
            Name = (DataContext as RcvMadeViewModel).Name,
            Photo = (DataContext as RcvMadeViewModel).Photo,
            OjratR = (DataContext as RcvMadeViewModel).OjratR,
            OjratP = (DataContext as RcvMadeViewModel).OjratP,
            MadeSubCategoryId = (DataContext as RcvMadeViewModel).MadeSubCategoryId,
            BoxId = (DataContext as RcvMadeViewModel).BoxId,
            Description = (DataContext as RcvMadeViewModel).Description,
            Type = DocumentItemType.MADE,
            TypeTitle = "دریافت ساخته"
            // مقداردهی بقیه فیلدهای لازم
        };

        this.DialogResult = true;
        this.Close();
    }
    private void MadeSubCategorySelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.MadeSubCategoryId == 0)
        {
            _viewModel.MadeSubCategoryId = 1;
        }
    }
    private void BoxSelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.BoxId == 0)
        {
            _viewModel.BoxId = 1;
        }
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
            RcvMadeViewModel vm = (RcvMadeViewModel)this.DataContext;
            vm.PhotoPreview = bitmap;

            // خواندن فایل به صورت byte[]
            byte[] fileBytes = File.ReadAllBytes(selectedFilePath);

            // ذخیره در ViewModel
            vm.PhotoBytes = fileBytes;
            vm.PhotoFileName = Path.GetFileName(selectedFilePath);
        }
    }
}