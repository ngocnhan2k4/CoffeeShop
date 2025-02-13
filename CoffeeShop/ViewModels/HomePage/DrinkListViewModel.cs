﻿using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CoffeeShop.Service.DataAccess.IDao;

namespace CoffeeShop.ViewModels.HomePage
{
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
            if (page != CurrentPage && page > 0 && page <= TotalPages)
            {
                CurrentPage = page;
                LoadData();
            }
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

}
