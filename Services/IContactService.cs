using System.Collections.Generic;
using PhoneBook.Models;

namespace PhoneBook.Services
{
    public interface IContactService
    {
        List<Contact> GetAllContacts();
        void UpdateContact(Contact contact);
        void AddContact(Contact contact);
        void DeleteContact(int id);
    }
}
