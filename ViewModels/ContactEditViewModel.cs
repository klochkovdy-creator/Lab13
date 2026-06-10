using System.Windows;
using System.Windows.Input;
using PhoneBook.Data;
using PhoneBook.Services;

namespace PhoneBook.ViewModels
{
    public class ContactEditViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigation;
        private readonly PhoneBookContext _context;

        private Contact _contact;
        private bool _isNewContact;

        private string _editName = string.Empty;
        public string EditName
        {
            get => _editName;
            set => Set(ref _editName, value);
        }

        private string _editPhone = string.Empty;
        public string EditPhone
        {
            get => _editPhone;
            set => Set(ref _editPhone, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ContactEditViewModel(INavigationService navigation, PhoneBookContext context)
        {
            _navigation = navigation;
            _context = context;

            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => _navigation.NavigateTo<ContactsListViewModel>());
        }

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is Contact contact)
            {
                _contact = contact;
                _isNewContact = false;
                EditName = _contact.Name;
                EditPhone = _contact.Phone;
            }
            else
            {
                _contact = new Contact();
                _isNewContact = true;
                EditName = string.Empty;
                EditPhone = string.Empty;
            }
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(EditName) || string.IsNullOrWhiteSpace(EditPhone))
            {
                MessageBox.Show("Имя и телефон не могут быть пустыми.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _contact.Name = EditName;
                _contact.Phone = EditPhone;

                if (_isNewContact)
                {
                    _context.Contacts.Add(_contact);
                }
                else
                {
                    _context.Entry(_contact).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

                _context.SaveChanges();
                _navigation.NavigateTo<ContactsListViewModel>();
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении контакта.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
