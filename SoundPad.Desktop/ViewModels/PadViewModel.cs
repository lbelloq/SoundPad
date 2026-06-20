using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoundPad.Desktop.Models;
using SoundPad.Desktop.Services;

namespace SoundPad.Desktop.ViewModels;

public partial class PadViewModel : ViewModelBase, IDisposable
{
    private readonly AudioService _audioService;
    private readonly PadButtonsService _buttonsService;

    [ObservableProperty] 
    private ObservableCollection<PadButton> _buttons;

    public PadViewModel()
    {
        _audioService = new AudioService();
        _buttonsService = new PadButtonsService();
        Buttons = new(_buttonsService.GetButtons());
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
