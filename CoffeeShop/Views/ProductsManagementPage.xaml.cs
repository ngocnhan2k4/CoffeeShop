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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductsManagementPage : Page
    {
        public DrinkViewModel ViewModel { get; set; }
       // public DrinkViewModel ChosenViewModel { get; set; }
        public ProductsManagementPage()
        {
            this.InitializeComponent();
            ViewModel = new DrinkViewModel();
            DateText.Text = DateTime.Now.ToString("dddd, d MMMM yyyy");
            SamplePage1Item.IsSelected = true;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ViewModel.ChosenDrinks.Add(new Drink() { Image = "/Assets/images.jpeg", Name = "Cà phê sữa Cà phê sữa Cà phê sữa", Size = "S", Description = "sữa", OriginalPrice = 12000, SalePrice = 15000, Stock = 100, CategoryID = 97 });
        }

        private void BackgroundRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ViewModel.ChosenDrinks[ViewModel.ChosenDrinks.Count - 1].Name = "Cà phêee";
        }

        private void TrashButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var drink = button?.DataContext as Drink;
            if (drink != null)
            {
                ViewModel.AddOrRemoveDrink(drink);
            }
        }
        private void StyledGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedDrink = e.ClickedItem as Drink;
            if (clickedDrink != null)
            {
                ViewModel.AddOrRemoveDrink(clickedDrink);
            }
        }
    }
}
