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
        IDao _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        private Invoice invoice; // Class-level variable to store the invoice

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
                string emailBody = message;
                EmailProgressRing.IsActive = true;
                EmailProgressRing.Visibility = Visibility.Visible;
                invoice = cart.AddInvoice();
                if(invoice.PaymentMethod== "Bank")
                {
                    // Generate the QR code image
                    string accountNo = "0915680152";
                    var apiRequest = new ApiReq
                    {
                        acqId = 970422,
                        accountNo = Convert.ToInt64(accountNo),
                        accountName = "NGUYEN DINH MINH NHAT",
                        addInfo = Utilities.GenerateRandomString(3),
                        amount = invoice.TotalAmount,
                        format = "text",
                        template = "print"
                    };
                    PollPaymentStatus(apiRequest.addInfo, invoice);
                    string jsonString = JsonSerializer.Serialize(apiRequest);

                    // Check if accountNo length in jsonString is less than the original accountNo string
                    if (jsonString.Contains($"\"accountNo\":{apiRequest.accountNo}") && apiRequest.accountNo.ToString().Length < accountNo.Length)
                    {
                        jsonString = jsonString.Replace($"\"accountNo\":{apiRequest.accountNo}", $"\"accountNo\":\"{accountNo}\"");
                    }
                    string qrCodeUrl = $"https://img.vietqr.io/image/{apiRequest.acqId}-{accountNo}-{apiRequest.template}.png?amount={apiRequest.amount}&addInfo={apiRequest.addInfo}&accountName={Uri.EscapeDataString(apiRequest.accountName)}";

                    // Embed the QR code image in the email body
                    emailBody = $@"
                    <html>
                    <body>
                        <p style='color: black; font-size: 1rem;'>{message}</p>
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

        public bool checkTThai(int id)
        {
            //get the current year
            int year = DateTime.Now.Year;
            var dataResult = _dao.GetRecentInvoice(year);
            foreach(var invoice in dataResult)
            {
                if (invoice.InvoiceID == id && invoice.Status == "Cancel") return true;
            }
            return false;
            
        }
        public void changeTThai(Invoice a)
        {
            _dao.UpdateInvoiceStatus(a.InvoiceID, "Paid");
       //     return a;
        }


        private async void cart_OrderClick()
        {
            bool result = false;
            try
            {
                EmailProgressRing.IsActive = true;
                EmailProgressRing.Visibility = Visibility.Visible;
                invoice=cart.AddInvoice();
                if (invoice.PaymentMethod == "Bank")
                {
                    result = await ShowQrCodeDialog(invoice);
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
                if(result) await ShowResultDialog("Success", "Order successfully.");
                else await ShowResultDialog("Fail", "Order fail.");
            }
        }

        private async Task<bool> PollPaymentStatus(string content, Invoice invoice)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer ZBBFN6V9OQ4Y1HPQUYDJHIPZC8RVE3X6MIU4LRCGKAV7XMW8WCK5CYTLRBSXDG0J");
                Console.WriteLine("Polling for payment status...");
                while (true)
                {
                    try
                    {
                        //https://my.sepay.vn/userapi/transactions/list?account_number=0915680152&limit=20
                          var response = await client.GetAsync("http://localhost:3000");
                   //     var response = await client.GetAsync("https://my.sepay.vn/userapi/transactions/list?account_number=0915680152&limit=20");
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var statusResult = JsonSerializer.Deserialize<TransactionRes>(responseContent);
                        if (statusResult.transactions.Count != 0)
                        {
                            bool has = false;
                            try
                            {
                                var transaction = new List<Transaction>(statusResult.transactions);
                                statusResult.transactions.ToList().ForEach((transaction) =>
                                {
                                    if ((int)Convert.ToDouble(transaction.amount_in) == invoice.TotalAmount && transaction.transaction_content == content)
                                    {
                                        StatusMessage.Text = "Payment Success";
                                        has = true;
                                    }
                                });
                                if (has)
                                {
                                    changeTThai(invoice);
                                    QrCodeDialog.Hide();
                                    return true;
                                }
                                else if (!has && checkTThai(invoice.InvoiceID))
                                {
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
                            StatusMessage.Text = "No Payment";
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
     
        private async Task<bool> ShowQrCodeDialog(Invoice invoice)
        {
            string accountNo = "0915680152";
            var apiRequest = new ApiReq
            {
                acqId = 970422,
                accountNo = Convert.ToInt64(accountNo),
                accountName = "NGUYEN DINH MINH NHAT",
                addInfo = Utilities.GenerateRandomString(3),
                amount = invoice.TotalAmount,
                format = "text",
                template = "print"
            };

            string jsonString = JsonSerializer.Serialize(apiRequest);

            // Check if accountNo length in jsonString is less than the original accountNo string
            if (jsonString.Contains($"\"accountNo\":{apiRequest.accountNo}") && apiRequest.accountNo.ToString().Length < accountNo.Length)
            {
                jsonString = jsonString.Replace($"\"accountNo\":{apiRequest.accountNo}", $"\"accountNo\":\"{accountNo}\"");
            }

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.vietqr.io/v2/generate", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var dataResult = JsonSerializer.Deserialize<ApiRes>(responseContent);

                var image = Utilities.Base64ToImage(dataResult.data.qrDataURL.Replace("data:image/png;base64,", ""));
                QRCodeImage.Source = image;

                // Start polling for payment status
                var pollingTask = PollPaymentStatus(apiRequest.addInfo, invoice);

                // Show the dialog
                await QrCodeDialog.ShowAsync();

                bool res = await pollingTask;
                return res;
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
            // Prevent the dialog from closing
            args.Cancel = true;
            _dao.UpdateInvoiceStatus(invoice.InvoiceID, "Cancel");
            // Update the status message
            StatusMessage.Text = "Canceling...";
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
