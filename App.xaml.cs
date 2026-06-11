using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Data;
using PhoneBook.Services;
using PhoneBook.ViewModels;

namespace PhoneBook
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddDbContext<PhoneBookContext>(
                options => options.UseSqlServer(
                    "Data Source=DESKTOP-P1VKMVF\\SQLEXPRESS;Initial Catalog=PhoneBook;Integrated Security=True;TrustServerCertificate=True"),
                ServiceLifetime.Transient);

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<ContactsListViewModel>();

            services.AddTransient<ContactEditViewModel>();

            services.AddTransient<AboutViewModel>();

            services.AddSingleton<MainWindow>(provider =>
            {
                var window = new MainWindow();
                window.DataContext = provider.GetRequiredService<MainWindowViewModel>();
                return window;
            });

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<MainWindow>().Show();
        }
    }
}
