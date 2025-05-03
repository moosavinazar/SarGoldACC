using System.IO;
using System.Text.Json;

namespace SarGoldACC.WpfApp.Helpers;

public class GridSettingsFileService
{
    private readonly string _filePath;

    public GridSettingsFileService(string userId)
    {
        var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SarGoldAcc", userId);
        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, "grid-settings.json");
    }

    public async Task<List<GridColumnSetting>> LoadSettingsAsync()
    {
        if (!File.Exists(_filePath)) return new List<GridColumnSetting>();
        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<GridColumnSetting>>(json) ?? new();
    }

    public async Task SaveSettingsAsync(List<GridColumnSetting> settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, json);
    }
}