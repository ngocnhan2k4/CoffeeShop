using CoffeeShop.Models;
using CoffeeShop.ViewModels;
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
using static System.Net.WebRequestMethods;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerPage : Page
    {
        public CustomerViewModel ViewModel { get; set; }
        public CustomerPage()
        {
            this.InitializeComponent();
            ViewModel = new CustomerViewModel();
            this.DataContext = ViewModel;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Search(searchBox.Text);
            if (pagesComboBox != null)
            {
                pagesComboBox.SelectedIndex = ViewModel.SelectedPageIndex;
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

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            await AddCustomerDialog.ShowAsync();
        }

        private void AddCustomerDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
           if(!string.IsNullOrWhiteSpace(CustomerNameTextBox.Text))
           {
                string customerName = CustomerNameTextBox.Text;

                var newCustomer = new Customer
                {
                    customerName = customerName,
                    customerID = 0,
                    totalMonney = 0,
                    totalPoint = 0,
                    type = "Thẻ thành viên"
                };

               
                ViewModel.AddCustomer(newCustomer);
            }
        }

        private void CustomerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CustomerNameTextBox.Text))
            {
                CustomerNameErrorTextBlock.Visibility = Visibility.Visible;
                AddCustomerDialog.IsPrimaryButtonEnabled = false;
            }
            else
            {
                CustomerNameErrorTextBlock.Visibility = Visibility.Collapsed;
                AddCustomerDialog.IsPrimaryButtonEnabled = true;
            }
        }

        private Customer selectedCustomer;

        private async void editButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            selectedCustomer = button.DataContext as Customer;

            EditCustomerNameTextBox.Text = selectedCustomer.customerName;
           

            await EditCustomerDialog.ShowAsync();
        }

        private void EditCustomerDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(EditCustomerNameTextBox.Text))
            {
                EditCustomerErrorTextBlock.Visibility = Visibility.Visible;
                args.Cancel = true;
            }
            else
            {
                EditCustomerErrorTextBlock.Visibility = Visibility.Collapsed;

                selectedCustomer.customerName = EditCustomerNameTextBox.Text;

                // Update the customer in the ViewModel
                ViewModel.UpdateCustomer(selectedCustomer);
            }
        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove the customer from the ViewModel
            //ViewModel.Customers.Remove(selectedCustomer);
            ViewModel.DeleteCustomer(selectedCustomer.customerID);
            EditCustomerDialog.Hide();
        }

        private async void discountButton_Click(object sender, RoutedEventArgs e)
        {
            await DiscountSettingDialog.ShowAsync();
        }

        private void DiscountSettingDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ErrorTextBlock.Visibility = Visibility.Collapsed;
            ErrorTextBlock.Text = "";

            if (int.TryParse(MemberCardDiscountTextBox.Text, out int memberCardDiscount) &&
                int.TryParse(SilverCardDiscountTextBox.Text, out int silverCardDiscount) &&
                int.TryParse(GoldCardDiscountTextBox.Text, out int goldCardDiscount) && memberCardDiscount >= 0 && memberCardDiscount <= 100 && silverCardDiscount >= 0 && silverCardDiscount <= 100 && goldCardDiscount >= 0 && goldCardDiscount <= 100)
            {
                
                ViewModel.MemberCardDiscount = memberCardDiscount;
                ViewModel.SilverCardDiscount = silverCardDiscount;
                ViewModel.GoldCardDiscount = goldCardDiscount;

                ViewModel.UpdateMemberCard();
            }
            else
            {
                args.Cancel = true;
                ErrorTextBlock.Text = Application.Current.Resources["ErrorPer"] as string;
                ErrorTextBlock.Visibility = Visibility.Visible;
               
            }
        }
    }
}
