using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Enums;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class RcvMelted : Window
{
    private readonly RcvMeltedViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;
    public DocumentItemDto ResultItem { get; private set; }
    
    public RcvMelted(RcvMeltedViewModel viewModel, IServiceProvider serviceProvide)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _serviceProvider = serviceProvide;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
        LaboratoryComboBox.ServiceProvider = _serviceProvider;
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
            WeightBes = (DataContext as RcvMeltedViewModel).Weight,
            Weight = (DataContext as RcvMeltedViewModel).Weight,
            Weight750 = (DataContext as RcvMeltedViewModel).Weight750,
            Ayar = (DataContext as RcvMeltedViewModel).Ayar,
            Certain = (DataContext as RcvMeltedViewModel).Certain,
            Ang = (DataContext as RcvMeltedViewModel).Ang,
            LaboratoryId = (DataContext as RcvMeltedViewModel).LaboratoryId,
            BoxId = (DataContext as RcvMeltedViewModel).BoxId,
            Description = (DataContext as RcvMeltedViewModel).Description,
            Type = DocumentItemType.MELTED,
            TypeTitle = "دریافت آبشده"
            // مقداردهی بقیه فیلدهای لازم
        };

        this.DialogResult = true;
        this.Close();
    }
    private void LaboratorySelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.LaboratoryId == 0)
        {
            _viewModel.LaboratoryId = 1;
        }
    }
    private void BoxSelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.BoxId == 0)
        {
            _viewModel.BoxId = 1;
        }
    }
}