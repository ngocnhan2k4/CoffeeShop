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
using System.Net.Http;
using System.Text.Json;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.Service;
using System.Reflection;
using Microsoft.UI;

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

        private async void cart_DeliveryClick(string recipientEmail, string message)
        {
            //send email
            try
            {
                string emailBody = message;
                EmailProgressRing.IsActive = true;
                EmailProgressRing.Visibility = Visibility.Visible;
                ViewModel.invoice = cart.AddInvoice();
                if (ViewModel.invoice.PaymentMethod == "Bank")
                {
                    string qrCodeUrl = ViewModel.GetQrURL(true);
                    PollPaymentStatus(ViewModel.apiRequest.addInfo, ViewModel.invoice, true);
                    // Embed the QR code image in the email body
                    emailBody = $@"
                    <html>
                    <body>
                        <p style='color: black; font-size: 0.9rem;'>{message}</p>
                        <img src='{qrCodeUrl}' alt='QR Code' />
                    </body>
                    </html>";
                }
                await Task.Run(() => SendEmailHelper.SendEmail(recipientEmail, emailBody));
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
            bool result = false;
            try
            {
                EmailProgressRing.IsActive = true;
                EmailProgressRing.Visibility = Visibility.Visible;
                ViewModel.invoice =cart.AddInvoice();
                if (ViewModel.invoice.PaymentMethod == "Bank")
                {
                    result = await ShowQrCodeDialog(ViewModel.invoice);
                }
                else result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");

            }
            finally
            {
                EmailProgressRing.IsActive = false;
                EmailProgressRing.Visibility = Visibility.Collapsed;
                string statusSuccess = Application.Current.Resources["Success"] as string;
                string statusFail = Application.Current.Resources["Fail"] as string;
                string orderSuccess = Application.Current.Resources["OrderSuccess"] as string;
                string orderFail = Application.Current.Resources["OrderFail"] as string;
                if (result) await ShowResultDialog(statusSuccess, orderSuccess);
                else await ShowResultDialog(statusFail, orderFail);
            }
        }

        private async Task<bool> PollPaymentStatus(string content, Invoice invoice, bool isDelivery = false)
        {
            using (HttpClient client = new HttpClient())
            {
                string token = Environment.GetEnvironmentVariable("TOKEN");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                while (true)
                {
                    int totalAmount = isDelivery ? invoice.TotalAmount + 10000 : invoice.TotalAmount;
                    try
                    {
                        //fake api
                        //var response = await client.GetAsync("http://localhost:3000");
                        var response = await client.GetAsync("https://my.sepay.vn/userapi/transactions/list?account_number=0915680152&limit=20");
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var statusResult = JsonSerializer.Deserialize<TransactionRes>(responseContent);
                        if (statusResult.transactions.Count != 0)
                        {
                            bool has = false;
                            try
                            {
                                int money_in = 0;
                                var transaction = new List<Transaction>(statusResult.transactions);
                                statusResult.transactions.ToList().ForEach((transaction) =>
                                {
                                    if (transaction.transaction_content.Contains(content))
                                    {           
                                       money_in += (int)Convert.ToDouble(transaction.amount_in);      
                                    }
                                });
                                if(money_in >= totalAmount)
                                {
                                    StatusMessage.Text = Application.Current.Resources["SuccessPayment"] as string;
                                    StatusMessage.Foreground = new SolidColorBrush(Colors.Green);
                                    has = true;
                                }
                                if (has)
                                {
                                    ViewModel.changeTThai(invoice);
                                    QrCodeDialog.Closing -= QrCodeDialog_Closing;
                                    QrCodeDialog.Hide();
                                    return true;
                                }
                                else if (!has && ViewModel.checkTThai(invoice.InvoiceID))
                                {
                                    QrCodeDialog.Closing -= QrCodeDialog_Closing;
                                    QrCodeDialog.Hide();
                                    return false;
                                }
                            }catch (Exception ex)
                            {
                                StatusMessage.Text = $"Error: {ex.Message}";
                                return false;
                            }

                        }
                        else
                        {
                            StatusMessage.Foreground = new SolidColorBrush(Colors.Gray);
                            StatusMessage.Text = Application.Current.Resources["NoPayment"] as string;
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        StatusMessage.Text = $"Error: {ex.Message}";
                        return false;
                    }

                    await Task.Delay(10000); // Wait for 10 seconds before polling again
                }
            }
        }
        private async Task ShowResultDialog(string title, string content)
        {
            ResultDialog.Title = title;
            ResultDialogContent.Text = content;
            await ResultDialog.ShowAsync();
        }
        private void QrCodeDialog_CancelClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Disable the primary buttons
            sender.IsPrimaryButtonEnabled = false;

            ViewModel._dao.UpdateInvoiceStatus(ViewModel.invoice.InvoiceID, "Cancel");
            // Update the status message
            StatusMessage.Text = Application.Current.Resources["Canceling"] as string;
            StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void QrCodeDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            // Prevent the dialog from closing
            args.Cancel = true;
        }
        private async Task<bool> ShowQrCodeDialog(Invoice invoice)
        {
            QRCodeImage.Source = new BitmapImage(new Uri(ViewModel.GetQrURL()));
            // Start polling for payment status
            var pollingTask = PollPaymentStatus(ViewModel.apiRequest.addInfo, invoice);
            // Show the dialog
            await QrCodeDialog.ShowAsync();
            bool res = await pollingTask;
            return res;
        }
        private void ResultDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            // Navigate to the Invoice page
            this.Frame.Navigate(typeof(InvoicePage));
            var mainWindow = App.m_window;
            mainWindow.UpdateNavigationBar("invoices");
        }

    }

}
