using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SoundPad.Desktop.Models;

namespace SoundPad.Desktop.Services;

public class PadButtonsService
{
    private const int _TOTAL_PAD_COUNT = 64;
    private const string _PAD_FOLDER_NAME = "Wave (HL compatible)";

    public List<PadButton> GetButtons()
    {
        var folder_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", _PAD_FOLDER_NAME);

        var buttons = Directory
            .GetFiles(folder_path, "*.wav")
            .OrderBy(f => Path.GetFileNameWithoutExtension(f))
            .Select(f => new PadButton { Id = Guid.NewGuid(), File = new FileInfo(f) })
            .ToList();

        while (buttons.Count < _TOTAL_PAD_COUNT)
        {
            buttons.Add(new PadButton { Id = Guid.NewGuid(), File = null });
        }

        return buttons;
    }
}
