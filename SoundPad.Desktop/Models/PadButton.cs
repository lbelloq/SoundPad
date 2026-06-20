using System;
using System.IO;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SoundPad.Desktop.Models;

public class PadButton : ObservableObject
{
    public int ButtonId { get; set; }

    private string? _path;
    public string? Path
    {
        get => _path;
        set
        {
            _path = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PathString));
        }
    }

    public string PathString => Path ?? "Unassigned";

    public override string ToString() => Path is null ? "Unassigned" : System.IO.Path.GetFileName(Path);
}