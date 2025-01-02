using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using CoffeeShop.Models;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using CoffeeShop.ViewModels.Settings;
using Microsoft.UI.Xaml.Documents;
using Windows.System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PaymentSettingsPage : Page
    {
        public PaymentSettingsViewModel ViewModel;
        public PaymentSettingsPage()
        {
            this.InitializeComponent();
            ViewModel = new PaymentSettingsViewModel();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetSettings();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveSettings();
            await ResultDialog.ShowAsync();
        }
        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && int.TryParse(menuItem.Tag.ToString(), out int bankCode))
            {
                if (bankCode ==0)
                {
                    ViewModel.AccountSettings.BankCode = 0;
                    CustomBox.Visibility = Visibility.Visible;
                }
                else
                {
                    ViewModel.AccountSettings.BankCode = bankCode;
                    CustomBox.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void CustomBankCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(CustomBankCodeTextBox.Text, out int customBankCode))
            {
                ViewModel.AccountSettings.BankCode = customBankCode;
            }
        }

    }
}
