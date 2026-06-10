using System.Windows.Input;
using PhoneBook.Services;

namespace PhoneBook.ViewModels
{

    public class MainWindowViewModel : ObservableObject
    {
        private readonly INavigationService _navigation;

        public INavigationService NavigationService => _navigation;

        public ICommand ShowContactsCommand { get; }
        public ICommand ShowAboutCommand { get; }

        public MainWindowViewModel(INavigationService navigation)
        {
            _navigation = navigation;

            _navigation.NavigateTo<ContactsListViewModel>();

            ShowContactsCommand = new RelayCommand(_ => _navigation.NavigateTo<ContactsListViewModel>());
            ShowAboutCommand = new RelayCommand(_ => _navigation.NavigateTo<AboutViewModel>());
        }
    }
}
