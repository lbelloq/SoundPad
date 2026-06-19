using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoundPad.Desktop.Models;
using SoundPad.Desktop.Services;

namespace SoundPad.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IDisposable
{
    private readonly AudioService _audioService;

    [ObservableProperty] 
    private ObservableCollection<PadButton> _buttons;

    public MainWindowViewModel()
    {
        _audioService = new AudioService();
    }

    [RelayCommand]
    private void Play()
    {
        _audioService.Play();
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
