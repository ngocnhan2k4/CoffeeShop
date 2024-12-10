using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.ViewModels;
using CoffeeShop.Views;
using Microsoft.Extensions.DependencyInjection;
using CoffeeShop.Service;
using CommunityToolkit.Mvvm.DependencyInjection;
using dotenv.net;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        //public static IServiceProvider Services { get; private set; }
        public App()
        {
            this.InitializeComponent();
            Ioc.Default.ConfigureServices(ConfigureServices());
            // Add Syncfusion Community License
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzUzMTEwNkAzMjM3MmUzMDJlMzBSN1dwZm5TQ2xIdUgzMXZFbXV1Q01wQzJFRkdpVXo0SVh0MWo4cXJoYXA0PQ==");
            
            ServiceFactory.Register(typeof(IDao), typeof(SqlServerDao));
            ConfigureServices();

            // Loading env 
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            MainWindow = new Window();
            m_window.Activate();


/*            // Set the window to full screen with close and minimize buttons
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(m_window);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.SetPresenter(AppWindowPresenterKind.Overlapped);

            // Get the display area size
            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            var displayAreaWorkArea = displayArea.WorkArea;
            var displayAreaWidth = displayAreaWorkArea.Width;
            var displayAreaHeight = displayAreaWorkArea.Height;

            // Set the window size to the display area size
            appWindow.Resize(new SizeInt32(displayAreaWidth, displayAreaHeight));

            // Center the window on the screen
            var displayAreaX = displayAreaWorkArea.X;
            var displayAreaY = displayAreaWorkArea.Y;
            appWindow.Move(new PointInt32(displayAreaX, displayAreaY));*/

            // Maximize the window
            // Assuming you have a reference to your window (currentWindow)
            if (m_window.AppWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.Maximize();
            }
        }
        public Window MainWindow { get; private set; }
        public static MainWindow m_window { get; set; }
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<MainViewModel>();
            return services.BuildServiceProvider();
        }
    }
}
