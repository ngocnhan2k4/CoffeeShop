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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Size = CoffeeShop.Models.Size;

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
            ViewModel.Search();
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
        private void OrderButton_Click(object sender, RoutedEventArgs e)
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

        private async void ThemeToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Application.Current != null)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        // Change to light theme
                        Application.Current.RequestedTheme = ApplicationTheme.Light;
                    });
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
                Debug.WriteLine($"Error setting theme: {ex.Message}");
            }
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
