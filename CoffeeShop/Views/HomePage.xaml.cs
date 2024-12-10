using CoffeeShop.Models;
using CoffeeShop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Size = CoffeeShop.Models.Size;
using System.Threading.Tasks;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels.HomePage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel { get; set; }
        public HomePage()
        {  
            ViewModel = new HomeViewModel();
            this.InitializeComponent();   
        }

        private void DrinkListUserControl_ItemClick(Drink drink, Size size)
        {
            cart.AddDrink(drink, size);
        }

        private void cart_DeliveryClick(string recipientEmail, string message)
        {
            SendEmail(recipientEmail, message);
        }

        private async void SendEmail(string recipientEmail, string message)
        {
            try
            {
                EmailProgressRing.IsActive = true;
                EmailProgressRing.Visibility = Visibility.Visible;
                cart.AddInvoice();
                await Task.Run(() => SendEmailHelper.SendEmail(recipientEmail, message));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error sending email: {ex.Message}");

            }
            finally
            {
                EmailProgressRing.IsActive = false;
                EmailProgressRing.Visibility = Visibility.Collapsed;
                await ShowResultDialog("Success", "Email sent successfully.");
            }
        }

        private async void cart_OrderClick()
        {
            try
            {
                EmailProgressRing.IsActive = true;
                EmailProgressRing.Visibility = Visibility.Visible;
                cart.AddInvoice();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");

            }
            finally
            {
                EmailProgressRing.IsActive = false;
                EmailProgressRing.Visibility = Visibility.Collapsed;
                await ShowResultDialog("Success", "Order successfully.");
            }
        }
        private async Task ShowResultDialog(string title, string content)
        {
            ResultDialog.Title = title;
            ResultDialogContent.Text = content;
            await ResultDialog.ShowAsync();
        }

        private void ResultDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            // Navigate to the Invoice page
            this.Frame.Navigate(typeof(InvoicePage));
        }

    }

}
