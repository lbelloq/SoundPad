using System;
using System.IO;
using System.Linq;
using System.Reflection;
using SoundFlow.Abstracts.Devices;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Providers;
using SoundFlow.Structs;

namespace SoundPad.Desktop.Services;

public sealed class AudioService : IDisposable
{
    private static readonly AudioFormat _FORMAT = AudioFormat.DvdHq;
    
    private readonly MiniAudioEngine _engine;
    private readonly AudioPlaybackDevice _device;
    private Stream? _stream;
    private StreamDataProvider? _provider;
    private SoundPlayer? _player;

    public AudioService()
    {
        _engine = new MiniAudioEngine();
        _engine.UpdateAudioDevicesInfo();
        var default_device = _engine.PlaybackDevices.FirstOrDefault(x => x.IsDefault);
        _device = _engine.InitializePlaybackDevice(default_device, _FORMAT);
    }

    private void DisposeTransientResources()
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

    public void Play(FileInfo? file)
    {
        if (file is null || !file.Exists)
        {
            return;
        }
        DisposeTransientResources();

        _stream = file.OpenRead();
        if (_stream == null)
        {
            throw new InvalidOperationException("Embedded resource not found.");
        }

        _provider = new StreamDataProvider(_engine, _FORMAT, _stream);
        _player = new SoundPlayer(_engine, _FORMAT, _provider);
        _device.MasterMixer.AddComponent(_player);
        _device.Start();
        _player.Play();
    }

    public void Stop()
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
