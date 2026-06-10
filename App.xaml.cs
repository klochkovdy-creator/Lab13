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

            // Transient — новый экземпляр DbContext при каждом обращении.
            // DbContext не является потокобезопасным, поэтому Transient предпочтительнее Singleton.
            services.AddDbContext<PhoneBookContext>(
                options => options.UseSqlServer(
                    "Data Source=DESKTOP-P1VKMVF\\SQLEXPRESS;Initial Catalog=PhoneBook;Integrated Security=True;TrustServerCertificate=True"),
                ServiceLifetime.Transient);

            // Singleton — сервис навигации должен быть единственным,
            // чтобы все ViewModels работали с одним CurrentViewModel и одним ContentControl.
            services.AddSingleton<INavigationService, NavigationService>();

            // Singleton — ViewModel главного окна (Shell) живёт столько же, сколько приложение.
            services.AddSingleton<MainWindowViewModel>();

            // Transient — новый экземпляр при каждом навигационном переходе,
            // что гарантирует актуальный список контактов при каждом возврате на экран.
            services.AddTransient<ContactsListViewModel>();

            // Transient — новый экземпляр при каждом открытии формы редактирования,
            // чтобы поля не содержали данные от предыдущего редактирования.
            services.AddTransient<ContactEditViewModel>();

            // Transient — экран «О программе» пересоздаётся при каждом переходе.
            services.AddTransient<AboutViewModel>();

            // Singleton — главное окно создаётся один раз; DataContext устанавливается
            // вручную через фабричный делегат, чтобы передать ViewModel из контейнера.
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
