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

            // Add Syncfusion Community License
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzUzMTEwNkAzMjM3MmUzMDJlMzBSN1dwZm5TQ2xIdUgzMXZFbXV1Q01wQzJFRkdpVXo0SVh0MWo4cXJoYXA0PQ==");
            ServiceFactory.Register(typeof(IDao), typeof(SqlServerDao));
            //ConfigureServices();

        }
        //private void ConfigureServices()
        //{
        //    var serviceCollection = new ServiceCollection();

        //    // Register services and view models
        //    serviceCollection.AddSingleton<IDao, MockDao>();
        //    serviceCollection.AddTransient<SalesDashboardViewModel>();
        //    serviceCollection.AddTransient<DashboardPage>();

        //    Services = serviceCollection.BuildServiceProvider();
        //}
        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            MainWindow = new Window();
            m_window.Activate();


            // Set the window to full screen with close and minimize buttons
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
        }
        public Window MainWindow { get; private set; }
        private Window m_window;
    }
}
