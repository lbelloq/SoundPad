using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SoundPad.Desktop.Models;

namespace SoundPad.Desktop.Services;

public class PadButtonsService
{
    private const int _TOTAL_PAD_COUNT = 64;
    private const string _PAD_FOLDER_NAME = "Wave (HL compatible)";
    private const string _CONFIG_FOLDER = "SoundPad";
    private const string _CONFIG_FILE = "buttons.json";

    private static readonly JsonSerializerOptions _JSON_OPTIONS = new()
    {
        WriteIndented = true
    };

    public List<PadButton> GetButtons()
    {
        var folder_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", _PAD_FOLDER_NAME);

        var idx = 0;
        var buttons = Directory
            .GetFiles(folder_path, "*.wav")
            .OrderBy(f => Path.GetFileNameWithoutExtension(f))
            .Select(f => new PadButton { ButtonId = ++idx, Path = f })
            .ToList();

        while (buttons.Count < _TOTAL_PAD_COUNT)
        {
            buttons.Add(new PadButton { ButtonId = ++idx, Path = null });
        }

        return buttons;
    }

    public void SaveButtonConfig(List<PadButton> buttons)
    {
        var config_folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _CONFIG_FOLDER);
        _ = Directory.CreateDirectory(config_folder);
        var json = JsonSerializer.Serialize(buttons, _JSON_OPTIONS);
        var config_path = Path.Combine(config_folder, _CONFIG_FILE);
        File.WriteAllText(config_path, json);
    }
}
