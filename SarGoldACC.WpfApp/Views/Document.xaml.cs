﻿using System.Windows;
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
            RcvMiscButton.Visibility = Visibility.Visible;
            PayMiscButton.Visibility = Visibility.Hidden;
            RcvMadeButton.Visibility = Visibility.Visible;
            PayMadeButton.Visibility = Visibility.Hidden;
            RcvCoinButton.Visibility = Visibility.Visible;
            PayCoinButton.Visibility = Visibility.Hidden;
        }
        
        if (e.Key == Key.OemMinus)
        {
            RcvOredrButton.Visibility = Visibility.Hidden;
            PayOredrButton.Visibility = Visibility.Visible;
            RcvMeltedButton.Visibility = Visibility.Hidden;
            PayMeltedButton.Visibility = Visibility.Visible;
            RcvMiscButton.Visibility = Visibility.Hidden;
            PayMiscButton.Visibility = Visibility.Visible;
            RcvMadeButton.Visibility = Visibility.Hidden;
            PayMadeButton.Visibility = Visibility.Visible;
            RcvCoinButton.Visibility = Visibility.Hidden;
            PayCoinButton.Visibility = Visibility.Visible;
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
        var payMeltedWindow = _serviceProvider.GetRequiredService<PayMelted>();
        payMeltedWindow.Owner = this;

        bool? result = payMeltedWindow.ShowDialog();
        if (result == true && payMeltedWindow.ResultItems != null && payMeltedWindow.ResultItems.Any())
        {
            foreach (var item in payMeltedWindow.ResultItems)
            {
                _viewModel.DocumentItems.Add(item);
            }
        }
    }

    private void ClickRcvMisc(object sender, RoutedEventArgs e)
    {
        var rcvMiscWindow = _serviceProvider.GetRequiredService<RcvMisc>();
        rcvMiscWindow.Owner = this;

        bool? result = rcvMiscWindow.ShowDialog();
        if (result == true && rcvMiscWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(rcvMiscWindow.ResultItem);
        }
    }

    private void ClickPayMisc(object sender, RoutedEventArgs e)
    {
        var payMiscWindow = _serviceProvider.GetRequiredService<PayMisc>();
        payMiscWindow.Owner = this;

        bool? result = payMiscWindow.ShowDialog();
        if (result == true && payMiscWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(payMiscWindow.ResultItem);
        }
    }
    
    private void ClickRcvMade(object sender, RoutedEventArgs e)
    {
        var rcvMiscWindow = _serviceProvider.GetRequiredService<RcvMade>();
        rcvMiscWindow.Owner = this;

        bool? result = rcvMiscWindow.ShowDialog();
        if (result == true && rcvMiscWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(rcvMiscWindow.ResultItem);
        }
    }
    
    private void ClickPayMade(object sender, RoutedEventArgs e)
    {
        var payMadeWindow = _serviceProvider.GetRequiredService<PayMade>();
        payMadeWindow.Owner = this;

        bool? result = payMadeWindow.ShowDialog();
        if (result == true && payMadeWindow.ResultItems != null && payMadeWindow.ResultItems.Any())
        {
            foreach (var item in payMadeWindow.ResultItems)
            {
                _viewModel.DocumentItems.Add(item);
            }
        }
    }
    
    private void ClickRcvCoin(object sender, RoutedEventArgs e)
    {
        var rcvCoinWindow = _serviceProvider.GetRequiredService<RcvCoin>();
        rcvCoinWindow.Owner = this;

        bool? result = rcvCoinWindow.ShowDialog();
        if (result == true && rcvCoinWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(rcvCoinWindow.ResultItem);
        }
    }
    
    private void ClickPayCoin(object sender, RoutedEventArgs e)
    {
        var payCoinWindow = _serviceProvider.GetRequiredService<PayCoin>();
        payCoinWindow.Owner = this;

        bool? result = payCoinWindow.ShowDialog();
        if (result == true && payCoinWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(payCoinWindow.ResultItem);
        }
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