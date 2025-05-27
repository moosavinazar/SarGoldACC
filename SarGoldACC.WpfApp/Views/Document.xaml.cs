using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.Enums;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Document : Window
{
    private readonly DocumentViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    private readonly IServiceProvider _serviceProvider;
    
    public Document(DocumentViewModel viewModel, IAuthorizationService authorizationService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _authorizationService = authorizationService;
        _serviceProvider = serviceProvider;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
    }

    private void DocumentWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }

        if (e.Key == Key.OemPlus)
        {
            RcvOredrButton.Visibility = Visibility.Visible;
            PayOredrButton.Visibility = Visibility.Hidden;
            RcvMeltedButton.Visibility = Visibility.Visible;
            PayMeltedButton.Visibility = Visibility.Hidden;
        }
        
        if (e.Key == Key.OemMinus)
        {
            RcvOredrButton.Visibility = Visibility.Hidden;
            PayOredrButton.Visibility = Visibility.Visible;
            RcvMeltedButton.Visibility = Visibility.Hidden;
            PayMeltedButton.Visibility = Visibility.Visible;
        }
    }

    private async void ClickAddCustomer(object sender, RoutedEventArgs e)
    {
    }

    private async void ClickReport(object sender, RoutedEventArgs e)
    {
    }
    
    private async void ClickTenDeal(object sender, RoutedEventArgs e)
    {
    }
    
    private void ClickRcvOrder(object sender, RoutedEventArgs e)
    {
        var rcvOrderWindow = _serviceProvider.GetRequiredService<RcvOrder>();
        rcvOrderWindow.Owner = this;

        bool? result = rcvOrderWindow.ShowDialog();
        if (result == true && rcvOrderWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(rcvOrderWindow.ResultItem);
        }
    }
    
    private void ClickPayOrder(object sender, RoutedEventArgs e)
    {
        var payOrderWindow = _serviceProvider.GetRequiredService<PayOrder>();
        payOrderWindow.Owner = this;

        bool? result = payOrderWindow.ShowDialog();
        if (result == true && payOrderWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(payOrderWindow.ResultItem);
        }
    }
    
    private void ClickRcvMelted(object sender, RoutedEventArgs e)
    {
        var rcvMeltedWindow = _serviceProvider.GetRequiredService<RcvMelted>();
        rcvMeltedWindow.Owner = this;

        bool? result = rcvMeltedWindow.ShowDialog();
        if (result == true && rcvMeltedWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(rcvMeltedWindow.ResultItem);
        }
    }
    
    private void ClickPayMelted(object sender, RoutedEventArgs e)
    {
        /*var payMeltedWindow = _serviceProvider.GetRequiredService<PayMelted>();
        payMeltedWindow.Owner = this;

        bool? result = payMeltedWindow.ShowDialog();
        if (result == true && payMeltedWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(payMeltedWindow.ResultItem);
        }*/
    }

    private async void ClickSaveDocument(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveDocument(DocumentType.FINAL);
    }
    
    private async void ClickSaveTemporaryDocument(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveDocument(DocumentType.TEMPORARY);
    }
}