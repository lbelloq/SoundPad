using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoundPad.Desktop.Models;
using SoundPad.Desktop.Services;

namespace SoundPad.Desktop.ViewModels;

public partial class PadViewModel : ViewModelBase, IDisposable
{
    private readonly AudioService _audioService;

    [ObservableProperty] 
    private ObservableCollection<PadButton> _buttons;

    public PadViewModel()
    {
        _audioService = new AudioService();
        
        var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Wave (HL compatible)");
        Buttons = new ObservableCollection<PadButton>(
            Directory.GetFiles(folder, "*.wav")
                .OrderBy(f => Path.GetFileNameWithoutExtension(f))
                .Select(f => new PadButton { Id = Guid.NewGuid(), File = new FileInfo(f) })
        );
        if (Buttons.Count < 64)
        {
            var remaining = 64 - Buttons.Count;
            for (var i = 0; i < remaining; i++)
            {
                Buttons.Add(new PadButton { Id = Guid.NewGuid(), File = null });
            }
        }
    }

    [RelayCommand]
    private void Play(PadButton elem)
    {
        _audioService.Play(elem.File);
    }

    [RelayCommand]
    private void Stop()
    {
        _audioService.Stop();
    }
    
    public void Dispose()
    {
        _audioService.Dispose();
    }
}
