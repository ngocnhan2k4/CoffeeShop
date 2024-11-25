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
            DateText.Text = DateTime.Now.ToString("dddd, d MMMM yyyy");
            // SamplePage1Item.IsSelected = true;
            LoadCategories();
        }
        private void LoadCategories()
        {
            var categories = ViewModel.Categories;
            foreach (var category in categories)
            {
                SelectorBar.Items.Add(new SelectorBarItem
                {
                    Text = category.CategoryName,
                    Tag = category.CategoryID
                });
            }
        }
        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToPreviousPage();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToNextPage();
        }

        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pagesComboBox.SelectedIndex >= 0 && pagesComboBox.SelectedIndex != ViewModel.SelectedPageIndex)
            {
                var item = pagesComboBox.SelectedItem as PageInfo;
                ViewModel.GoToPage(item.Page);
            }
        }
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)SortComboBox.SelectedItem;
            if (selectedItem != null)
            {
                var tag = selectedItem.Tag.ToString();
                ViewModel.SortBy = tag;
            }
            // load lại chữ dưới page navigation
            if(pagesComboBox!= null)
            {
                pagesComboBox.SelectedIndex = ViewModel.SelectedPageIndex;
            }    
            
        }
        private void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs e)
        {
            var selectedItem = (SelectorBarItem)SelectorBar.SelectedItem;
            if (selectedItem != null)
            {
                var selectedCategoryID = int.Parse(selectedItem.Tag.ToString());
                Console.WriteLine(selectedCategoryID);
                // Handle selection change based on selectedCategoryID
                ViewModel.CategoryID = selectedCategoryID;
            }
            // load lại chữ dưới page navigation
            if (pagesComboBox != null)
            {
                pagesComboBox.SelectedIndex = ViewModel.SelectedPageIndex;
            }
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Search(SearchTextBox.Text);
            // load lại chữ dưới page navigation
            pagesComboBox.SelectedIndex = ViewModel.SelectedPageIndex;
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

        private void BackgroundRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void TrashButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var detailInvoice = button.DataContext as DetailInvoice;
            if (detailInvoice != null)
                ViewModel.RemoveDrink(detailInvoice);
         }

        private void SizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedSize = comboBox?.SelectedItem as Size;
            if (comboBox != null && selectedSize != null)
            {
                var stackPanel = comboBox.Parent as StackPanel;
                if (stackPanel != null)
                {
                    var priceTextBlock = stackPanel.FindName("PriceTextBlock") as TextBlock;
                    var stockTextBlock = stackPanel.FindName("StockTextBlock") as Run;
                    if (priceTextBlock != null)
                    {
                        priceTextBlock.Text = selectedSize.Price.ToString();
                        stockTextBlock.Text = selectedSize.Stock.ToString();
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var drink = button.DataContext as Drink;
            var stackPanel = button.Parent as StackPanel;
            var sizeComboBox = stackPanel.FindName("SizeComboBox") as ComboBox;
            var size = sizeComboBox.SelectedItem as Size;
            ViewModel.AddDrink(drink, size);
        }
        /*    private void OrderButton_Click(object sender, RoutedEventArgs e)
            {
                // Get the checked ToggleButton content
                var checkedToggleButton = ToggeButtons.Children.OfType<ToggleButton>().FirstOrDefault(tb => tb.IsChecked == true);
                var checkedContent = checkedToggleButton?.Content.ToString();

                // Get the total price
                var totalPrice = ViewModel.TotalPrice;

                // Order ...
                var message = $"Order: {checkedContent}, Total Price: {totalPrice}";
                var dialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Order Details",
                    Content = message,
                    CloseButtonText = "OK"
                };
                dialog.ShowAsync();
            }*/
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
        private async void ThemeToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Application.Current != null && Dispatcher != null)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        // Change to dark theme
                        Application.Current.RequestedTheme = ApplicationTheme.Dark;
                    });
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
                Debug.WriteLine($"Error setting theme: {ex.Message}");
            }
        }

        private async void DeliveryButton_Click(object sender, RoutedEventArgs e)
        {

            totalPriceTextBlock.Text = ViewModel.TotalPrice.ToString("C", new CultureInfo("vi-VN"));

            await DeliveryDialog.ShowAsync();
            string message = $"Thông tin đơn hàng:<br>Tổng tiền: {ViewModel.TotalPrice.ToString("C", new CultureInfo("vi-VN"))}<br>Chi tiết:<br>";
            int cnt = 1;
            foreach (var item in ViewModel.ChosenDrinks)
            {
                message += $"{cnt}. {item.NameDrink} - {item.Size} - {item.Quantity} - {item.Price}<br>";
            }
            message += $"Trạng thái: {statusComboBox.SelectedItem}<br>";
            message += $"<br>Xác nhận địa chỉ giao hàng: {addressTextBox.Text}<br>";
            
            string email = emailTextBox.Text;
            SendEmail(email, message);
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


        private async void SendEmail(string recipientEmail, string message)
        {
            try
            {
                EmailProgressRing.IsActive = true;
                EmailProgressRing.Visibility = Visibility.Visible;

                await Task.Run(() => SendEmailHelper.SendEmail(recipientEmail, message));
                await ShowEmailResultDialog("Success", "Email sent successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error sending email: {ex.Message}");
               
            }
            finally
            {
               
                EmailProgressRing.IsActive = false;
                EmailProgressRing.Visibility = Visibility.Collapsed;
            }
        }

        private async Task ShowEmailResultDialog(string title, string content)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }


    }

    //private void StyledGrid_ItemClick(object sender, ItemClickEventArgs e)
    //{
    //    //var clickedDrink = e.ClickedItem as Drink;
    //    //if (clickedDrink != null)
    //    //{
    //    //    ViewModel.AddDrink(clickedDrink);
    //    //}
    //}


    //private void AddButton_Click(object sender, RoutedEventArgs e)
    //    {
    //        var button = sender as Button;
    //        var drink = button.DataContext as Drink;
    //        var stackPanel = button.Parent as StackPanel;
    //        var sizeComboBox = stackPanel.FindName("SizeComboBox") as ComboBox;
    //        var size = sizeComboBox.SelectedItem as Size;
    //        ViewModel.AddDrink(drink, size);
    //    }
    //}

    
}
