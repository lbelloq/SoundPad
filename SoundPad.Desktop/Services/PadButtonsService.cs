using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using SoundPad.Desktop.Models;

namespace SoundPad.Desktop.Services;

public class PadButtonsService
{
    private const int _TOTAL_PAD_COUNT = 64;
    private const string PadFolderName = "Wave (HL compatible)";

    public List<PadButton> GenerateButtons(string? folderPath = null)
    {
        folderPath ??= GetDefaultFolder();

        var buttons = Directory
            .GetFiles(folderPath, "*.wav")
            .OrderBy(f => Path.GetFileNameWithoutExtension(f))
            .Select(f => new PadButton { Id = Guid.NewGuid(), File = new FileInfo(f) })
            .ToList();

        while (buttons.Count < _TOTAL_PAD_COUNT)
        {
            buttons.Add(new PadButton { Id = Guid.NewGuid(), File = null });
        }

        return buttons;
    }

    private static string GetDefaultFolder()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads",
            PadFolderName
        );
    }
}
