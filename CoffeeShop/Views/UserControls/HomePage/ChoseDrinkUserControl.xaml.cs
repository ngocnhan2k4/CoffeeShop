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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views.UserControls.HomePage
{
    public sealed partial class ChoseDrinkUserControl : UserControl
    {
        public delegate void EventHandler(string recipientEmail, string message);
        public event EventHandler DeliveryClick;

        public ChoseDrinkViewModel ViewModel { get; set; }

        public ChoseDrinkUserControl()
        {
            ViewModel = new ChoseDrinkViewModel();
            this.InitializeComponent();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is not ToggleButton checkedToggleButton)
            {
                return;
            }

            foreach (ToggleButton toggleButton in ToggeButtons.Children.OfType<ToggleButton>())
            {
                toggleButton.IsChecked = toggleButton == checkedToggleButton;
                toggleButton.IsHitTestVisible = toggleButton != checkedToggleButton;
            }
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
            // Get the checked ToggleButton content
            var checkedToggleButton = ToggeButtons.Children.OfType<ToggleButton>().FirstOrDefault(tb => tb.IsChecked == true);
            var checkedContent = checkedToggleButton?.Content.ToString();

            // Get the total price
            var totalPrice = ViewModel.TotalPrice;

            // Order details message
            var message = $"Order: {checkedContent}, Total Price: {totalPrice}";

            // Update the ContentDialog content
            OrderDetailsTextBlock.Text = message;

            // Show the ContentDialog
            await OrderDetailsDialog.ShowAsync();
        }
        private async void DeliveryButton_Click(object sender, RoutedEventArgs e)
        {
        //    totalPriceTextBlock.Text = ViewModel.TotalPrice.ToString("C", new CultureInfo("vi-VN"));
            await DeliveryDialog.ShowAsync();
       /*     string message = $"Thông tin đơn hàng:<br>Tổng tiền: {ViewModel.TotalPrice.ToString("C", new CultureInfo("vi-VN"))}<br>Chi tiết:<br>";
            int cnt = 1;
            foreach (var item in ViewModel.ChosenDrinks)
            {
                message += $"{cnt}. {item.NameDrink} - {item.Size} - {item.Quantity} - {item.Price}<br>";
            }
            message += $"Trạng thái: {statusComboBox.SelectedItem}<br>";
            message += $"<br>Xác nhận địa chỉ giao hàng: {addressTextBox.Text}<br>";

            string email = emailTextBox.Text;
            SendEmail(email, message);*/
        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            bool check;
            if (string.IsNullOrWhiteSpace(email))
                check = false;
            check = Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (!check)
            {
                emailErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                emailErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }



        private void DeliveryDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            totalPriceTextBlock.Text = ViewModel.TotalPrice.ToString("C", new CultureInfo("vi-VN"));
            string message = $"Thông tin đơn hàng:<br>Tổng tiền: {ViewModel.TotalPrice.ToString("C", new CultureInfo("vi-VN"))}<br>Chi tiết:<br>";
            int cnt = 1;
            foreach (var item in ViewModel.ChosenDrinks)
            {
                message += $"{cnt}. {item.NameDrink} - {item.Size} - {item.Quantity} - {item.Price}<br>";
            }
            message += $"Trạng thái: {statusComboBox.SelectedItem}<br>";
            message += $"<br>Xác nhận địa chỉ giao hàng: {addressTextBox.Text}<br>";
            string email = emailTextBox.Text;
            if (DeliveryClick != null)
            {
                DeliveryClick.Invoke(email, message);
            }
        }
    }
}
