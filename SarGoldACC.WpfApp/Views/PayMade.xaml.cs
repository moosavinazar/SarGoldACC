using System.Windows;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.DTOs.Made;
using SarGoldACC.Core.Enums;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class PayMade : Window
{
    private readonly PayMadeViewModel _viewModel;
    public List<DocumentItemDto> ResultItems { get; private set; } = new List<DocumentItemDto>();
    public PayMade(PayMadeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
    }
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
    }
    private async void Window_Activated(object sender, EventArgs e)
    {
    }

    private void ClickSave(object sender, RoutedEventArgs e)
    {
        var selectedItems = MadeDataGrid.SelectedItems.Cast<MadePayDataGridDto>().ToList();

        // تبدیل به DocumentItemDto یا مدل موردنظر شما
        foreach (var item in selectedItems)
        {
            ResultItems.Add(new DocumentItemDto
            {
                MadeId = item.Id,
                WeightBed = item.Weight,
                Weight = item.Weight,
                Weight750 = item.Weight750,
                Name = item.Name,
                Ayar = item.Ayar,
                Barcode = item.Barcode,
                Photo = item.Photo,
                OjratP = item.OjratP,
                OjratR = item.OjratR,
                MadeSubCategoryId = item.MadeCategoryId,
                BoxId = item.BoxId,
                Description = _viewModel.Description,
                Type = DocumentItemType.MADE,
                TypeTitle = "پرداخت ساخته"
                // سایر فیلدهای مورد نیاز را هم اضافه کن
            });
        }

        this.DialogResult = true;
        this.Close();
    }
}