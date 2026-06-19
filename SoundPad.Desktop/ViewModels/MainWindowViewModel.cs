using System;
using System.IO;
using CommunityToolkit.Mvvm.Input;
using LibVLCSharp.Shared;

namespace SoundPad.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IDisposable
{
    private readonly LibVLC _libvlc;
    private readonly MediaPlayer _mediaPlayer;
    
    private Media? _media;
    private MemoryStream? _stream;
    private StreamMediaInput? _input;
    
    public MainWindowViewModel()
    {
        Core.Initialize();
        _libvlc = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libvlc);
        _media = null;
        _stream = null;
        _input = null;
    }

    [RelayCommand]
    private void Play()
    {
        _stream = new MemoryStream(Sounds.Assassin);
        _input = new StreamMediaInput(_stream);
        var media = new Media(_libvlc, _input, ":no-video");
        _mediaPlayer.Play(media);
    }

    [RelayCommand]
    private void Stop()
    {
        _mediaPlayer?.Stop();
        _media?.Dispose();
        _input?.Dispose();
        _stream?.Dispose();
    }
    
    public void Dispose()
    {
        _mediaPlayer.Dispose();
        _libvlc.Dispose();
    }
}