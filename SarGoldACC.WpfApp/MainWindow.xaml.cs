using System.ComponentModel;
using System.Windows.Input;
using Fluent;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : RibbonWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void RibbonWindow_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.D1:
            case Key.NumPad1:
                MainRibbon.SelectedTabItem = TabMain;
                break;
            case Key.D2:
            case Key.NumPad2:
                MainRibbon.SelectedTabItem = TabProducts;
                break;
            case Key.D3:
            case Key.NumPad3:
                MainRibbon.SelectedTabItem = TabBaseInfo;
                break;
            case Key.D4:
            case Key.NumPad4:
                MainRibbon.SelectedTabItem = TabReport;
                break;
            case Key.D5:
            case Key.NumPad5:
                MainRibbon.SelectedTabItem = TabSetting;
                break;
        }
    }
    
}