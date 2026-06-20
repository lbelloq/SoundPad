using System;
using System.IO;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SoundPad.Desktop.Models;

public partial class PadButton : ObservableObject
{
    public int ButtonId { get; set; }

    [ObservableProperty] 
    public partial string? Path { get; set; }

    public string PathString => Path ?? "Unassigned";

    public override string ToString() => Path is null ? "Unassigned" : System.IO.Path.GetFileName(Path);
}