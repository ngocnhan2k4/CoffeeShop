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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views.UserControls.HomePage
{
    public sealed partial class ChoseDrinkUserControl : UserControl
    {
        public class ChoseDrinkViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public FullObservableCollection<DetailInvoice> ChosenDrinks { get; set; }
            public decimal TotalPrice { get; set; }

            public ChoseDrinkViewModel()
            {
                ChosenDrinks = new FullObservableCollection<DetailInvoice>();
            }
            public void AddDrink(Drink drink, Size size)
            {
                var existingInvoice = ChosenDrinks.FirstOrDefault(di => di.NameDrink == drink.Name && di.Size == size.Name);
                if (existingInvoice != null)
                {
                    existingInvoice.Quantity += 1;
                }
                else
                {
                    var newInvoice = new DetailInvoice
                    {
                        NameDrink = drink.Name,
                        Quantity = 1,
                        Size = size.Name,
                        Price = size.Price
                    };
                    ChosenDrinks.Add(newInvoice);
                }
                TotalPrice = ChosenDrinks.Sum(di => di.Price * di.Quantity);
            }
            public void RemoveDrink(DetailInvoice detail)
            {

                ChosenDrinks.Remove(detail);
                TotalPrice = ChosenDrinks.Sum(di => di.Price * di.Quantity);
            }
        }

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


    }
}
