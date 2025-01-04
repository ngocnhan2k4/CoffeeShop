using CoffeeShop.Views.Settings;
using CoffeeShop.Service;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private readonly ILanguageSelectorService _languageService;

        public SettingsPage()
        {
            this.InitializeComponent();
            _languageService = Ioc.Default.GetService<ILanguageSelectorService>();
            _languageService.LanguageChanged += (s, e) => UpdateNavigationViewContent();
            NavView.SelectedItem = NavView.MenuItems[0];
            UpdateNavigationViewContent();
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;

            switch (selectedItem.Name.ToString())
            {
                case "appearance":
                    content.Navigate(typeof(Settings.AppearancePage));
                    break;
                case "paymentSettings":
                    content.Navigate(typeof(Settings.PaymentSettingsPage));
                    break;
                case "productsManagement":
                    content.Navigate(typeof(Settings.ProductsManagementPage));
                    break;
                case "aboutUs":
                    content.Navigate(typeof(Settings.AboutUsPage));
                    break;
            }
        }

        private void UpdateNavigationViewContent()
        {
            var resources = Application.Current.Resources;
            foreach (NavigationViewItem item in NavView.MenuItems)
            {
                switch (item.Name)
                {
                    case "appearance":
                        item.Content = resources["Appearance"];
                        item.Tag = resources["AppearanceTag"];
                        break;
                    case "paymentSettings":
                        item.Content = resources["PaymentSettings"];
                        item.Tag = resources["PaymentSettingsTag"];
                        break;
                    case "productsManagement":
                        item.Content = resources["ProductsManagement"];
                        item.Tag = resources["ProductsManagementTag"];
                        break;
                    case "aboutUs":
                        item.Content = resources["AboutUs"];
                        item.Tag = resources["AboutUsTag"];
                        break;
                }
            }
        }
    }
}
