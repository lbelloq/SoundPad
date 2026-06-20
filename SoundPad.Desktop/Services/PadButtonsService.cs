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
    private const string _CONFIG_FOLDER = "SoundPad";
    private const string _CONFIG_FILE = "buttons.json";

    private static readonly JsonSerializerOptions _JSON_OPTIONS = new()
    {
        WriteIndented = true
    };

    public List<PadButton> GetButtons()
    {
        EnsureConfigDirectoryExists();
        var config_file = Path.Combine(GetConfigDirectory(), _CONFIG_FILE);

        List<PadButton> buttons;
        if (!File.Exists(config_file))
        {
            buttons = [];
        }
        else
        {
            buttons = JsonSerializer.Deserialize<List<PadButton>>(File.ReadAllText(config_file), _JSON_OPTIONS)?.OrderBy(p=>p.ButtonId).ToList() ?? [];
        }
        if (buttons.Count < 64)
        {
            var idx = buttons.Count;
            while (buttons.Count < _TOTAL_PAD_COUNT)
            {
                buttons.Add(new PadButton { ButtonId = ++idx, Path = null });
            }
        }
        return buttons;
    }

    public void SaveButtonConfig(List<PadButton> buttons)
    {
        EnsureConfigDirectoryExists();
        var json = JsonSerializer.Serialize(buttons, _JSON_OPTIONS);
        var config_path = Path.Combine(GetConfigDirectory(), _CONFIG_FILE);
        File.WriteAllText(config_path, json);
    }

    private static string GetConfigDirectory() =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _CONFIG_FOLDER);

    private static void EnsureConfigDirectoryExists()
    {
        var config_dir = GetConfigDirectory();
        if (!Directory.Exists(config_dir))
        {
            _ = Directory.CreateDirectory(config_dir);
        }
    }
}
