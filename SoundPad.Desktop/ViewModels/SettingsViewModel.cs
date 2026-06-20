using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoundPad.Desktop.Models;
using SoundPad.Desktop.Services;

namespace SoundPad.Desktop.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly PadButtonsService _buttonsService;
    
    [ObservableProperty] 
    private ObservableCollection<PadButton> _buttons;

    public SettingsViewModel()
    {
        _buttonsService = new PadButtonsService();
        Buttons = new ObservableCollection<PadButton>(_buttonsService.GetButtons());
    }

    [RelayCommand]
    private async Task PickFile(PadButton elem)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime lifetime)
        {
            return;
        }

        var top_level = TopLevel.GetTopLevel(lifetime.MainWindow);
        if (top_level?.StorageProvider is null)
        {
            return;
        }

        var open_opts = new FilePickerOpenOptions()
        {
            Title = "Open sound",
            AllowMultiple = false,
            FileTypeFilter = [new FilePickerFileType("Wave format") { Patterns = ["*.wav"] }]
        };
        var file = await top_level.StorageProvider.OpenFilePickerAsync(open_opts);
        if (!file.Any())
        {
            return;
        }
        elem.Path = new FileInfo(file[0].Path.AbsolutePath).FullName;
    }
}
