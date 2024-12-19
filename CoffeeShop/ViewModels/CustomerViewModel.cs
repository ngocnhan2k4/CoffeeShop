using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service;
using CoffeeShop.Service.DataAccess;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        IDao _dao;
        public FullObservableCollection<Customer> Customers
        {
            get; set;
        }
        public CustomerViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            //Customers = new FullObservableCollection<Customer>(_dao.GetCustomers());
            RowsPerPage = 8;
            CurrentPage = 1;
            LoadData();
        }
        public ObservableCollection<PageInfo> PageInfos { get; set; }
        public int SelectedPageIndex { get; set; } = 0;
        public string Keyword { get; set; } = "";
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCustomers { get; set; }
        public int RowsPerPage { get; set; }
        public void LoadData()
        {
            var (items, count) = _dao.GetCustomers(CurrentPage, RowsPerPage, Keyword);
            Customers = new FullObservableCollection<Customer>(items);

            TotalCustomers = count;
            TotalPages = (TotalCustomers / RowsPerPage) + (((TotalCustomers % RowsPerPage) == 0) ? 0 : 1);

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

        public void AddCustomer(Customer customer)
        {
            if (_dao.AddCustomer(customer))
            {
                LoadData();
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            if (_dao.UpdateCustomer(customer))
            {
                LoadData();
            }
        }

        public void DeleteCustomer(int id)
        {
            if (_dao.DeleteCustomer(id))
            {
                LoadData();
            }
        }
    }
}
