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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views.UserControls.HomePage
{
    public sealed partial class DrinkListUserControl : UserControl
    {
        public delegate void EventHandler(Drink drink, Size size);
        public event EventHandler ItemClick;
        public class DrinkListViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public FullObservableCollection<Drink> Drinks
            {
                get; set;
            }

            public FullObservableCollection<Category> Categories { get; set; }
            public FullObservableCollection<DetailInvoice> ChosenDrinks { get; set; }
            public decimal TotalPrice { get; set; }
            public ObservableCollection<PageInfo> PageInfos { get; set; }
            public int SelectedPageIndex { get; set; } = 0;

            public string Keyword { get; set; } = "";

            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public int TotalItems { get; set; }
            public int RowsPerPage { get; set; }

            public int _categoryID = -1;
            public int CategoryID
            {
                get => _categoryID;
                set
                {
                    _categoryID = value;
                    CurrentPage = 1;
                    LoadData();
                }
            }

            private string _sort = "";
            IDao _dao;
            public string SortBy
            {
                get => _sort;
                set
                {
                    _sort = value;
                    _sortOptions.Clear();
                    if (value == "Stock")
                    {
                        _sortOptions["Stock"] = SortType.Descending;
                    }
                    else if (value == "PriceIncrease")
                    {
                        _sortOptions["Price"] = SortType.Ascending;
                    }
                    else if (value == "PriceDecrease")
                    {
                        _sortOptions["Price"] = SortType.Descending;
                    }
                    LoadData();
                }
            }
            public DrinkListViewModel()
            {
                _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
                Categories = new FullObservableCollection<Category>(_dao.GetCategories());
                RowsPerPage = 8;
                CurrentPage = 1;
                LoadData();
            }

            private Dictionary<string, SortType> _sortOptions = new();
            public void LoadData()
            {
                _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
                var (items, count) = _dao.GetDrinks(CurrentPage, RowsPerPage, Keyword, CategoryID, _sortOptions);
                Drinks = new FullObservableCollection<Drink>(items);

                TotalItems = count;
                TotalPages = (TotalItems / RowsPerPage) + (((TotalItems % RowsPerPage) == 0) ? 0 : 1);

                PageInfos = new();
                for (int i = 1; i <= TotalPages; i++)
                {
                    PageInfos.Add(new PageInfo
                    {
                        Page = i,
                        Total = TotalPages
                    });
                }

                SelectedPageIndex = CurrentPage - 1;
            }
            public void GoToPage(int page)
            {
                CurrentPage = page;
                LoadData();
            }
            public void GoToNextPage()
            {
                if (CurrentPage < TotalPages)
                {
                    CurrentPage++;
                    LoadData();
                }
            }
            public void GoToPreviousPage()
            {
                if (CurrentPage > 1)
                {
                    CurrentPage--;
                    LoadData();
                }
            }
            public void Search(string keyword)
            {
                CurrentPage = 1;
                Keyword = keyword;
                LoadData();
            }
        }

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