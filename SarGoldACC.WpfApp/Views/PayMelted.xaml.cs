using System.Windows;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.DTOs.SubMeltedDto;
using SarGoldACC.Core.Enums;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class PayMelted : Window
{
    private readonly PayMeltedViewModel _viewModel;
    public List<DocumentItemDto> ResultItems { get; private set; } = new List<DocumentItemDto>();
    public PayMelted(PayMeltedViewModel viewModel)
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
        var selectedItems = SubMeltedDataGrid.SelectedItems.Cast<SubMeltedPayDataGridDto>().ToList();

        // تبدیل به DocumentItemDto یا مدل موردنظر شما
        foreach (var item in selectedItems)
        {
            ResultItems.Add(new DocumentItemDto
            {
                SubMeltedId = item.Id,
                WeightBed = item.Weight,
                Ang = item.Ang,
                Ayar = item.Ayar,
                Certain = item.Certain,
                LaboratoryId = item.LaboratoryId,
                BoxId = item.BoxId,
                Description = _viewModel.Description,
                Type = DocumentItemType.MELTED
                // سایر فیلدهای مورد نیاز را هم اضافه کن
            });
        }

        this.DialogResult = true;
        this.Close();
    }

}