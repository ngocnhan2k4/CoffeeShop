using CoffeeShop.Service.BusinessLogic;
using CoffeeShop.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels
{
    /// <summary>
    /// This class represents the view model for the Sales Dashboard
    /// </summary>
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

            Data = new ObservableCollection<MonthSalesData>
            {
                new MonthSalesData { Month = 1, Revenue = salesService.MonthlyRevenue[0] },
                new MonthSalesData { Month = 2, Revenue = salesService.MonthlyRevenue[1] },
                new MonthSalesData { Month = 3, Revenue = salesService.MonthlyRevenue[2] },
                new MonthSalesData { Month = 4, Revenue = salesService.MonthlyRevenue[3] },
                new MonthSalesData { Month = 5, Revenue = salesService.MonthlyRevenue[4] },
                new MonthSalesData { Month = 6, Revenue = salesService.MonthlyRevenue[5] },
                new MonthSalesData { Month = 7, Revenue = salesService.MonthlyRevenue[6] },
                new MonthSalesData { Month = 8, Revenue = salesService.MonthlyRevenue[7]},
                new MonthSalesData { Month = 9, Revenue = salesService.MonthlyRevenue[8] },
                new MonthSalesData { Month = 10,Revenue = salesService.MonthlyRevenue[9] },
                new MonthSalesData { Month = 11,Revenue = salesService.MonthlyRevenue[10]},
                new MonthSalesData { Month = 12,Revenue = salesService.MonthlyRevenue[11] }
            };
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
}