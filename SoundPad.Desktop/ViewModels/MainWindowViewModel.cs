using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoundFlow.Abstracts.Devices;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Providers;
using SoundFlow.Structs;
using SoundPad.Desktop.Models;

namespace SoundPad.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IDisposable
{
    private static readonly AudioFormat _FORMAT = AudioFormat.DvdHq;
    
    private readonly MiniAudioEngine _engine;
    private readonly AudioPlaybackDevice _device;

    private Stream? _stream;
    private StreamDataProvider? _provider;
    private SoundPlayer? _player;

    [ObservableProperty] 
    private ObservableCollection<PadButton> _buttons;
    
    public MainWindowViewModel()
    {
        _engine = new MiniAudioEngine();
        _engine.UpdateAudioDevicesInfo();
        var default_device = _engine.PlaybackDevices.FirstOrDefault(x => x.IsDefault);
        _device = _engine.InitializePlaybackDevice(default_device, _FORMAT);
    }

    private void DisposeTransientResources()
    {
        Debug.WriteLine("DisposeTransientResources");
        if (_player != null)
        {
            _player.Stop();
            _device.MasterMixer.RemoveComponent(_player);
            _player.Dispose();
            _player = null;
        }
        _provider?.Dispose();
        _provider = null;
        _stream?.Dispose();
        _stream = null;
    }

    [RelayCommand]
    private void Play()
    {
        DisposeTransientResources();

        var assembly = Assembly.GetExecutingAssembly();
        _stream = assembly.GetManifestResourceStream("SoundPad.Desktop.Resources.biggamehunter.wav");
        if (_stream == null)
        {
            throw new InvalidOperationException("Embedded resource not found.");
        }

        _provider = new StreamDataProvider(_engine, _FORMAT, _stream);
        _player = new SoundPlayer(_engine, _FORMAT, _provider);
        _device.MasterMixer.AddComponent(_player);
        _device.Start();
        //_player.PlaybackEnded += (_, _) => Stop();
        _player.Play();
    }

    [RelayCommand]
    private void Stop()
    {
        DisposeTransientResources();
    }
    
    public void Dispose()
    {
        DisposeTransientResources();
        _device.Stop();
        _device.Dispose();
        _engine.Dispose();
    }
}