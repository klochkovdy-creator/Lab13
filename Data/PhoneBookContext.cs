using Microsoft.EntityFrameworkCore;

namespace PhoneBook.Data
{

    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext(DbContextOptions<PhoneBookContext> options)
            : base(options)
        {
        }


        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
            });
        }
    }
}
