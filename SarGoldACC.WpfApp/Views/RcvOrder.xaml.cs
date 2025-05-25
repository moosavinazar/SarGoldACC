using System.Windows;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Enums;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class RcvOrder : Window
{
    private readonly RcvOrderViewModel _viewModel;
    public DocumentItemDto ResultItem { get; private set; }
    
    public RcvOrder(RcvOrderViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
    }
    private void RcvOrderWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    
    private async void ClickAddCustomer(object sender, RoutedEventArgs e)
    {
    }

    private void ClickSaveRcvOrder(object sender, RoutedEventArgs e)
    {
        ResultItem = new DocumentItemDto
        {
            CounterpartySideTwoId = (int)(DataContext as RcvOrderViewModel).CounterpartyId,
            WeightBes = (DataContext as RcvOrderViewModel).WeightBes,
            RiyalBes = (DataContext as RcvOrderViewModel).RiyalBes,
            Description = "TEST",
            Type = DocumentItemType.ORDER
            // مقداردهی بقیه فیلدهای لازم
        };

        this.DialogResult = true;
        this.Close();
    }
}