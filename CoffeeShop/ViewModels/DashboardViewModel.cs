using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service;
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
        public SalesService salesService = new SalesService(/*new MockDao()*/ServiceFactory.GetChildOf(typeof(IDao)) as IDao, DateTime.Now.Year);
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
        public SalesService salesService = new SalesService(/*new MockDao()*/ServiceFactory.GetChildOf(typeof(IDao)) as IDao, DateTime.Now.Year);

        public PieChartViewModel()
        {
            Data = new ObservableCollection<ProductSalesData>();
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

    public class DashboardViewModel : INotifyPropertyChanged
    {
        public SalesService SaleService { get; set; }
        public FullObservableCollection<Drink> Drinks { get; set; }
        public FullObservableCollection<Category> Categories { get; set; }
        public FullObservableCollection<Invoice> Invoices { get; set; }
        public FullObservableCollection<DetailInvoice> DetailInvoices { get; set; }
        public FullObservableCollection<DeliveryInvoice> DeliveryInvoices { get; set; }
        public FullObservableCollection<Invoice> RecentInvoices { get; set; }

 
        IDao _dao;
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
       
        public DashboardViewModel(int year)
        {
            //IDao dao = new MockDao();
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            Drinks = new FullObservableCollection<Drink>(_dao.GetDrinks());
            Categories = new FullObservableCollection<Category>(_dao.GetCategories());
            Invoices = new FullObservableCollection<Invoice>(_dao.GetInvoices());
            DetailInvoices = new FullObservableCollection<DetailInvoice>(_dao.GetDetailInvoices());
            DeliveryInvoices = new FullObservableCollection<DeliveryInvoice>(_dao.GetDeliveryInvoices());
            SaleService = new SalesService(_dao,year);
         //   TopDrink = new ObservableCollection<string>(SaleService.CalculateTopDrinks(_dao, year));
            TopDrink = new ObservableCollection<string>(_dao.CalculateTopDrinks( year));
            RecentInvoices = new FullObservableCollection<Invoice>(_dao.GetRecentInvoice(year));
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   
}