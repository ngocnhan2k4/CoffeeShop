using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service.BusinessLogic;
using CoffeeShop.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels
{
    /// <summary>
    /// This class represents the view model for the Sales Dashboard
    /// </summary>
    /// 
    public class MonthSalesData
    {
        public int Month { get; set; }
        public double Revenue { get; set; }
    }

    public class ProductSalesData
    {
        public string Category { get; set; }
        public double SalesAmount { get; set; }
    }

    public class CartesianChartViewModel
    {
        public ObservableCollection<MonthSalesData> Data { get; set; }
        public SalesService salesService = new SalesService(new MockDao(), DateTime.Now.Year);
        public CartesianChartViewModel()
        {

            Data = new ObservableCollection<MonthSalesData>();
            for (int i = 0; i < 12; i++)
            {
                Data.Add(new MonthSalesData { Month = i + 1, Revenue = salesService.MonthlyRevenue[i] });
            }
        }
    }

    public class PieChartViewModel
    {
        public ObservableCollection<ProductSalesData> Data { get; set; }
        public SalesService salesService = new SalesService(new MockDao(), DateTime.Now.Year);

        public PieChartViewModel()
        {
            foreach (var categoryData in salesService.RevenueByCategory)
            {

                Data.Add(new ProductSalesData
                {
                    Category = categoryData.Key,
                    SalesAmount = categoryData.Value
                });
            }
        }
    }

    public class SalesDashboardViewModel : INotifyPropertyChanged
    {
        public SalesService SaleService { get; set; }
        public FullObservableCollection<Drink> Drinks { get; set; }
        public FullObservableCollection<Category> Categories { get; set; }
        public FullObservableCollection<Invoice> Invoices { get; set; }
        public FullObservableCollection<DetailInvoice> DetailInvoices { get; set; }
        public FullObservableCollection<DeliveryInvoice> DeliveryInvoices { get; set; }
        public ObservableCollection<string> TopDrink
        {
            get { return _topDrink; }
            set
            {
                if (_topDrink != value)
                {
                    _topDrink = value;
                    OnPropertyChanged(nameof(TopDrink)); 
                }
            }
        }
        private ObservableCollection<string> _topDrink;

        public event PropertyChangedEventHandler PropertyChanged;
       
        public SalesDashboardViewModel(int year)
        {
            IDao dao = new MockDao();
            Drinks = new FullObservableCollection<Drink>(dao.GetDrinks());
            Categories = new FullObservableCollection<Category>(dao.GetCategories());
            Invoices = new FullObservableCollection<Invoice>(dao.GetInvoices());
            DetailInvoices = new FullObservableCollection<DetailInvoice>(dao.GetDetailInvoices());
            DeliveryInvoices = new FullObservableCollection<DeliveryInvoice>(dao.GetDeliveryInvoices());
            SaleService = new SalesService(dao,year);
            TopDrink = new ObservableCollection<string>(SaleService.CalculateTopDrinks(dao, year));
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   
}