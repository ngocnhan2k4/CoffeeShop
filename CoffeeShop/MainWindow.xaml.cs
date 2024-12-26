using CoffeeShop.ViewModels;
using CoffeeShop.Views;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.PushNotifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
     //   public MainViewModel ViewModel { get; set; }
        public MainWindow()
        {
            
            this.InitializeComponent();

       //     ViewModel = Ioc.Default.GetService<MainViewModel>();

            config();
        }

        private void config()
        {
            this.dashboard.Tag = typeof(DashboardPage);
            //var dashboardPage = App.Services.GetService<DashboardPage>();
            //this.Content = dashboardPage;
            this.products.Tag = typeof(HomePage);
            this.settings.Tag = typeof(SettingsPage);
            this.invoices.Tag = typeof(InvoicePage);
            this.customer.Tag = typeof(CustomerPage);
            NavView.SelectedItem = NavView.MenuItems[1];
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;
            switch (selectedItem.Name.ToString())
            {
                case "dashboard":
                case "products":
                case "settings":
                case "invoices":
                case "customer":
                    var type = (Type)(selectedItem).Tag;

                    content.Navigate(type);
                    break;
            }
        }
        public void UpdateNavigationBar(string pageName)
        {
            NavView.SelectedItem = NavView.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Name.ToString() == pageName);
        }
    }
}
