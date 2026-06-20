using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SoundPad.Desktop.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private object? _selectedContent;

    public MainViewModel()
    {
        SelectedContent = new PadViewModel();
    }

    [RelayCommand]
    private void SelectView(int view)
    {
        SelectedContent = view switch
        {
            0 => new PadViewModel(),
            1 => new SettingsViewModel(),
            2 => new AboutViewModel(),
            _ => SelectedContent
        };
    }
}
