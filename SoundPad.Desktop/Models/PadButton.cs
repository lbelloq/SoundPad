using System;
using System.IO;
using System.Text.Json.Serialization;

namespace SoundPad.Desktop.Models;

public class PadButton
{
    public int ButtonId { get; set; }
    
    public string? Path { get; set; }
    
    public override string ToString()
    {
        return Path is null ?
            "Unassigned":
            System.IO.Path.GetFileName(Path);
    }
}