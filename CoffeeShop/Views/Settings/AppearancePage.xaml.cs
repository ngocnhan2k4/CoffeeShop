using CoffeeShop.ViewModels;
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

namespace CoffeeShop.Views.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppearancePage : Page
    {
        public AppearanceViewModel ViewModel { get; set; }
        public AppearancePage()
        {
            ViewModel = Ioc.Default.GetService<AppearanceViewModel>();
            this.InitializeComponent();
            UpdateUI();
        }
        public void UpdateUI()
        {
            LightThemeRadioButton.IsChecked = (ViewModel.GetTheme() == ElementTheme.Light);
            DarkThemeRadioButton.IsChecked = (ViewModel.GetTheme() == ElementTheme.Dark);
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetSettings();
            UpdateUI();
        }

        private async void ButtonSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveChanges();
            UpdateUI();

            // Force refresh current page with delay to ensure resource loading
            if (Frame != null)
            {
                var currentPage = Frame.Content;
                var pageType = currentPage.GetType();
                
                // Navigate to the same page to force a complete refresh
                Frame.Navigate(pageType);

                // Show success dialog after navigation
                var resources = Application.Current.Resources;
                await new ContentDialog()
                {
                    XamlRoot = XamlRoot,
                    Content = new TextBlock()
                    {
                        Text = resources["SaveChangesSuccess"]?.ToString() ?? "Save changes successfully",
                        FontSize = 20
                    },
                    CloseButtonText = resources["Close"]?.ToString() ?? "Close"
                }.ShowAsync();
            }
        }

        //private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (sender is ComboBox comboBox && comboBox.SelectedItem is string selectedLanguage)
        //    {
        //        ViewModel.SetLanguageCommand.Execute(selectedLanguage);
        //    }
        //}
    }
}
