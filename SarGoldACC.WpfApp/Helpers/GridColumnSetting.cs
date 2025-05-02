namespace SarGoldACC.WpfApp.Helpers;

public class GridColumnSetting
{
    public string GridName { get; set; }
    public Dictionary<string, bool> ColumnVisibility { get; set; } = new();
}