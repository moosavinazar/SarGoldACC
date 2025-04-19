using System.ComponentModel;
using SarGoldACC.WpfApp.Stores;

namespace SarGoldACC.WpfApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;

    public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

    public MainViewModel()
    {
        _navigationStore = new NavigationStore();
        _navigationStore.CurrentViewModel = new LoginViewModel(NavigateTo);

        _navigationStore.PropertyChanged += OnCurrentViewModelChanged;
    }
    
    private void NavigateTo(ViewModelBase viewModel)
    {
        _navigationStore.CurrentViewModel = viewModel;
    }

    private void OnCurrentViewModelChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NavigationStore.CurrentViewModel))
            OnPropertyChanged(nameof(CurrentViewModel));
    }
}