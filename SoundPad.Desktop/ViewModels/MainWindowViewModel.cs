using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommunityToolkit.Mvvm.Input;
using SoundFlow.Abstracts.Devices;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Providers;
using SoundFlow.Structs;

namespace SoundPad.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IDisposable
{
    private static readonly AudioFormat _FORMAT = AudioFormat.DvdHq;
    
    private readonly MiniAudioEngine _engine;
    private readonly AudioPlaybackDevice _device;

    private Stream? _stream;
    private StreamDataProvider? _provider;
    private SoundPlayer? _player;
    
    public MainWindowViewModel()
    {
        _engine = new MiniAudioEngine();
        _engine.UpdateAudioDevicesInfo();
        var defaultDevice = _engine.PlaybackDevices.FirstOrDefault(x => x.IsDefault);
        _device = _engine.InitializePlaybackDevice(defaultDevice, _FORMAT);
    }

    [RelayCommand]
    private void Play()
    {
        var assembly = Assembly.GetExecutingAssembly();
        _stream = assembly.GetManifestResourceStream("SoundPad.Desktop.Resources.biggamehunter.wav")!;
        _provider = new StreamDataProvider(_engine, _FORMAT, _stream);
        _player = new SoundPlayer(_engine, _FORMAT, _provider);
        _device.MasterMixer.AddComponent(_player);
        _device.Start();
        _player.Play();
    }

    [RelayCommand]
    private void Stop()
    {
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
    
    public void Dispose()
    {
        if (_player != null)
        {
            Stop();
        }
        _device.Stop();
        _device.Dispose();
        _engine.Dispose();
    }
}