using System.Collections.Generic;
using System.Linq;
using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Services
{

    public class ContactService : IContactService
    {
        private readonly PhoneBookContext _context;

        public ContactService(PhoneBookContext context)
        {
            _context = context;
        }


        public List<Models.Contact> GetAllContacts()
        {
            return _context.Contacts
                .Select(c => new Models.Contact
                {
                    Id    = c.Id,
                    Name  = c.Name,
                    Phone = c.Phone
                })
                .ToList();
        }

        public void UpdateContact(Models.Contact contact)
        {
            var entity = _context.Contacts.Find(contact.Id);
            if (entity != null)
            {
                entity.Name  = contact.Name;
                entity.Phone = contact.Phone;
                _context.SaveChanges();
            }
        }

        public void AddContact(Models.Contact contact)
        {
            var entity = new Data.Contact
            {
                Name  = contact.Name,
                Phone = contact.Phone
            };
            _context.Contacts.Add(entity);
            _context.SaveChanges();
            contact.Id = entity.Id;
        }

        public void DeleteContact(int id)
        {
            var entity = _context.Contacts.Find(id);
            if (entity != null)
            {
                _context.Contacts.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
