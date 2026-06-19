using System;
using System.IO;

namespace SoundPad.Desktop.Models;

public class PadButton
{
    public Guid Id { get; set; }
    public FileInfo? File { get; set; }
}