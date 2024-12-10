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
using Microsoft.UI.Xaml.Documents;
using Size = CoffeeShop.Models.Size;
using CoffeeShop.ViewModels.HomePage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views.UserControls.HomePage
{
    public sealed partial class DrinkListUserControl : UserControl
    {
        public delegate void EventHandler(Drink drink, Size size);
        public event EventHandler ItemClick;

        public DrinkListViewModel ViewModel { get; set; }
        public DrinkListUserControl()
        {
            this.InitializeComponent();
            ViewModel = new DrinkListViewModel();
            DateText.Text = DateTime.Now.ToString("dddd, d MMMM yyyy");
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
                if (ViewModel != null)
                {
                    ViewModel.SortBy = tag;
                }
            }
            if (pagesComboBox != null)
            {
                pagesComboBox.SelectedIndex = ViewModel?.SelectedPageIndex ?? 0;
            }
        }
        private void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs e)
        {
            var selectedItem = (SelectorBarItem)SelectorBar.SelectedItem;
            if (selectedItem != null)
            {
                var selectedCategoryID = int.Parse(selectedItem.Tag.ToString());
                ViewModel.CategoryID = selectedCategoryID;
            }
            if (pagesComboBox != null)
            {
                pagesComboBox.SelectedIndex = ViewModel.SelectedPageIndex;
            }
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Search(SearchTextBox.Text);
            if (pagesComboBox != null)
            {
                pagesComboBox.SelectedIndex = ViewModel.SelectedPageIndex;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var drink = button.DataContext as Drink;
            var stackPanel = button.Parent as StackPanel;
            var sizeComboBox = stackPanel.FindName("SizeComboBox") as ComboBox;
            var size = sizeComboBox.SelectedItem as Size;
            //  ViewModel.AddDrink(drink, size);
            if (ItemClick != null)
            {
                ItemClick.Invoke(drink, size);
            }
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
    }
}