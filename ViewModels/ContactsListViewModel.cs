using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PhoneBook.Data;
using PhoneBook.Services;

namespace PhoneBook.ViewModels
{
    public class ContactsListViewModel : ObservableObject
    {
        private readonly INavigationService _navigation;
        private readonly PhoneBookContext _context;

        private Contact _selectedContact;
        public Contact SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                Set(ref _searchText, value);
                FilterContacts();
            }
        }

        public ObservableCollection<Contact> Contacts { get; } = new ObservableCollection<Contact>();

        public ICommand AddContactCommand { get; }
        public ICommand EditContactCommand { get; }
        public ICommand DeleteContactCommand { get; }

        public ContactsListViewModel(INavigationService navigation, PhoneBookContext context)
        {
            _navigation = navigation;
            _context = context;

            AddContactCommand = new RelayCommand(
                _ => _navigation.NavigateTo<ContactEditViewModel>(null));

            EditContactCommand = new RelayCommand(
                _ => _navigation.NavigateTo<ContactEditViewModel>(SelectedContact),
                _ => SelectedContact != null);

            DeleteContactCommand = new RelayCommand(
                _ => DeleteContact(),
                _ => SelectedContact != null);

            LoadContacts();
        }

        private void LoadContacts()
        {
            Contacts.Clear();
            foreach (var c in _context.Contacts)
                Contacts.Add(c);
        }

        private void FilterContacts()
        {
            Contacts.Clear();
            foreach (var c in _context.Contacts)
            {
                if (string.IsNullOrWhiteSpace(_searchText) ||
                    c.Name.ToLower().Contains(_searchText.ToLower()) ||
                    c.Phone.Contains(_searchText))
                {
                    Contacts.Add(c);
                }
            }
        }

        private void DeleteContact()
        {
            if (SelectedContact == null) return;

            try
            {
                _context.Contacts.Remove(SelectedContact);
                _context.SaveChanges();
                Contacts.Remove(SelectedContact);
                SelectedContact = null;
            }
            catch
            {
                MessageBox.Show("Ошибка при удалении контакта.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
