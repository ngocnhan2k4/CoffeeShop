using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.Service;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static CoffeeShop.Service.DataAccess.IDao;
using Size = CoffeeShop.Models.Size;
using CoffeeShop.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoffeeShop.ViewModels.HomePage;
using Windows.UI.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views.UserControls.HomePage
{
    public sealed partial class ChoseDrinkUserControl : UserControl
    {
        public delegate void EventHandler(string recipientEmail, string message);
        public event EventHandler DeliveryClick;
        public delegate void OrderHandler();
        public event OrderHandler OrderClick;
        public ChoseDrinkViewModel ViewModel { get; set; }
        public Invoice invoice { get; set; }
        public DeliveryInvoice delivery { get; set; }
        public ChoseDrinkUserControl()
        {
            ViewModel = new ChoseDrinkViewModel();
            this.InitializeComponent();
        }

        public void AddDrink(Drink drink, Size size)
        {
            ViewModel.AddDrink(drink, size);
        }
        private void TrashButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var detailInvoice = button.DataContext as DetailInvoice;
            if (detailInvoice != null)
                ViewModel.RemoveDrink(detailInvoice);
        }

        private async void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if(ViewModel.ChosenDrinks.Count == 0)
            {
               await OrderErrorDialog.ShowAsync();
               return;
            }
            totalPriceTextBlock.Text = ViewModel.TotalPrice.ToString("C", new CultureInfo("vi-VN"));
            await OrderDetailsDialog.ShowAsync();
            
        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            bool check = IsValidEmail(email);
            if (!check)
            {
                errorTextBlock.Visibility = Visibility.Visible;
                errorTextBlock.Text = "Invalid email format.";
            }
            else
            {
                errorTextBlock.Visibility = Visibility.Collapsed;
            }
        }
        private bool IsValidEmail(string email)
        {
            bool check;
            if (string.IsNullOrWhiteSpace(email))
                check = false;
            check = Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return check;
        }
        private bool IsValidPhone(string phone)
        {
            bool check;
            if (string.IsNullOrWhiteSpace(phone))
                check = false;
            check = Regex.IsMatch(phone,
                @"0[0-9]{9}",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return check;
        }
        private void OrderDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string email = emailTextBox.Text;
            string name = nameTextBox.Text;
            string phone = phoneTextBox.Text;
            string address = addressTextBox.Text;
            var selectedPaymentMethodItem = paymentMethodComboBox.SelectedItem as ComboBoxItem;
            string paymentMethod = selectedPaymentMethodItem?.Tag.ToString();
            var selectedShippingMethodItem = shippingMethodComboBox.SelectedItem as ComboBoxItem;
            string shippingMethod = selectedShippingMethodItem?.Tag as string;
            string memberID = idTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) || paymentMethod == null || shippingMethod == null || memberID==null)
            {
                errorTextBlock.Text = Application.Current.Resources["ErrorType"] as string;
                errorTextBlock.Visibility = Visibility.Visible;
                args.Cancel = true;
                return;
            }
            // If shipping method is Delivery, check if email, phone, and address are filled and valid
            if (shippingMethod == "Delivery")
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(address))
                {
                    errorTextBlock.Text = Application.Current.Resources["ErrorType"] as string;
                    errorTextBlock.Visibility = Visibility.Visible;
                    args.Cancel = true;
                    return;
                }
                // Validate email and phone number formats
                if (!IsValidEmail(email))
                {
                    errorTextBlock.Text = Application.Current.Resources["ErrorEmail"] as string;
                    errorTextBlock.Visibility = Visibility.Visible;
                    args.Cancel = true;
                    return;
                }

                if (!IsValidPhone(phone))
                {
                    errorTextBlock.Text = Application.Current.Resources["ErrorPhone"] as string;
                    errorTextBlock.Visibility = Visibility.Visible;
                    args.Cancel = true;
                    return;
                }
            }

            if (!int.TryParse(memberID, out int memberId) && applyMemberInfoSwitch.IsOn)
            {
                errorTextBlock.Text = Application.Current.Resources["ErrorID"] as string;
                errorTextBlock.Visibility = Visibility.Visible;
                args.Cancel = true;
                return;
            }

            List<Customer> customers = ViewModel.GetCustomers();
            List<int> validMemberIds = customers.Select(c => c.customerID).ToList();

            if (!validMemberIds.Contains(memberId) && applyMemberInfoSwitch.IsOn)
            {
                errorTextBlock.Text = Application.Current.Resources["ErrorInvalidID"] as string;
                errorTextBlock.Visibility = Visibility.Visible;
                args.Cancel = true;
                return;
            }
            ViewModel.SetCustomerId(memberId);

            args.Cancel = false;
            //close Dialog
            OrderDetailsDialog.Hide();
            invoice = new Invoice
            {
                TotalAmount = ViewModel.TotalPrice,
                Status = "Wait",
                CreatedAt = (DateTime.Now).ToString(),
                PaymentMethod = paymentMethod,
                CustomerName = name,
                HasDelivery = shippingMethod== "Delivery" ? "Y" : "N",
            };
            delivery = new DeliveryInvoice
            {
                Address = address,
                PhoneNumber = phone,
                ShippingFee = 10000
            };
            if (shippingMethod== "Delivery" && DeliveryClick != null)
            {
                string message = $"Thông tin đơn hàng:<br>Tên khách hàng: {name}<br><br>Tổng tiền: {ViewModel.TotalPrice.ToString("C", new CultureInfo("vi-VN"))} + {(10000).ToString("C", new CultureInfo("vi-VN"))} = {(ViewModel.TotalPrice + 10000).ToString("C", new CultureInfo("vi-VN"))}<br>Chi tiết:<br>";
                int cnt = 1;
                foreach (var item in ViewModel.ChosenDrinks)
                {
                    message += $"{cnt}. {item.NameDrink} - {item.Size} - {item.Quantity} - {item.Price}<br>";
                }
                message += $"<br>Phương thức thanh toán: {paymentMethod}<br>";
                message += $"Trạng thái: Đợi giao hàng<br>";
                message += $"<br>Số điện thoại: {phone}<br>";
                message += $"<br>Xác nhận địa chỉ giao hàng: {address}<br>";
                DeliveryClick.Invoke(email, message);
            }
            else
            {
                OrderClick.Invoke();
            }
            
        }

        public Invoice AddInvoice(string status = "Wait")
        {
            return ViewModel.AddInvoice(invoice, delivery);
        }
        private void ShippingMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (shippingMethodComboBox.SelectedItem != null)
            {
                var selectedShippingMethodItem = shippingMethodComboBox.SelectedItem as ComboBoxItem;
                string shippingMethod = selectedShippingMethodItem?.Tag as string;
                bool isDelivery = shippingMethod == "Delivery";
                emailTextBox.IsEnabled = isDelivery;
                addressTextBox.IsEnabled = isDelivery;
                phoneTextBox.IsEnabled = isDelivery;
            }
        }

        private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            ViewModel.CalcTotal();
        }

        private void ApplyMemberInfoSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (applyMemberInfoSwitch.IsOn)
            {
                idTextBox.IsEnabled= true;
                totalPriceTextBlock.TextDecorations = TextDecorations.Strikethrough;
                discountTextBlock.Visibility = Visibility.Visible;
                ViewModel.TotalPriceAfterDiscount = ViewModel.TotalPrice - ViewModel.Discount;
                discountTextBlock.Text = ViewModel.TotalPriceAfterDiscount.ToString("C", new CultureInfo("vi-VN"));
                //discountTextBlock.Text = ViewModel.Discount.ToString("C", new CultureInfo("vi-VN"));
            }
            else
            {
                idTextBox.IsEnabled = false;
                idTextBox.Text = "";
                totalPriceTextBlock.TextDecorations = TextDecorations.None;
                discountTextBlock.Visibility = Visibility.Collapsed;

            }
        }

        private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(idTextBox.Text))
            {
                ViewModel.SetCustomerId(0);
            }
            if (int.TryParse(idTextBox.Text, out int memberId))
            {
                ViewModel.SetCustomerId(memberId);
                totalPriceTextBlock.TextDecorations = TextDecorations.Strikethrough;
            }
            else
            {
                ViewModel.SetCustomerId(0); 
            }
        }
    }
}
