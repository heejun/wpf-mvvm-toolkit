using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using WpfMvvmToolkit.Core.Services;
using WpfMvvmToolkit.Core.ViewModels;
using WpfMvvmToolkit.Services;

namespace WpfMvvmToolkit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
        }

        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // add services
            services.AddSingleton<ILogService, ConsoleLogService>();

            // add view-models
            services.AddTransient(typeof(MainViewModel));

            return services.BuildServiceProvider();
        }
    }
}
