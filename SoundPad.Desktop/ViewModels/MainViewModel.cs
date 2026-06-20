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
    private void SelectView(object? view)
    {
        if (view is string a && int.TryParse(a, out var b))
        {
            SelectedContent = b switch
            {
                0 => new PadViewModel(),
                1 => new SettingsViewModel(),
                2 => new AboutViewModel(),
                _ => SelectedContent
            };
        }
        
    }
}
