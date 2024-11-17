using CoffeeShop.Service.DataAccess;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Service.BusinessLogic
{
    /// <summary>
    /// This class is used to calculate the sales statistics of the coffee shop
    /// </summary>
    public class SalesService
    {
        public double TotalCost { get; set; } // This variable is used to store the total cost of all drinks in the shop
        public double Revenue { get; set; } // This variable is used to store the total revenue of the shop
        public double Profit { get; set; } // This variable is used to store the profit of the shop
        public List<int> Years { get; set; } // This variable is used to store the years that the shop has sales
        public List<int> MonthlyRevenue { get; set; } // This variable is used to store the revenue of the shop in each month
        public List<string> TopDrinks { get; set; } // This variable is used to store the top 5 drinks that are sold the most
        public int NumberOrders { get; set; } // This variable is used to store the number of orders in a year
        public Dictionary<string, int> RevenueByCategory { get; set; } // This variable is used to store the revenue of the shop by category
        public IDao dao { get; set; } // This variable is used to access the data access layer

        
        public SalesService(IDao dao, int Year)// Constructor of the class
        {
            this.dao = dao;
            TotalCost = dao.CalculateTotalCost();
            Revenue = dao.CalculateRevenue(Year);
            Profit = dao.CalculateProfit(Year);
            Years = dao.CalculateYears();
            MonthlyRevenue = dao.CalculateMonthlyRevenue(Year);
            TopDrinks = dao.CalculateTopDrinks(Year);
            RevenueByCategory = dao.CalculateRevenueCategory(Year);
            NumberOrders = dao.CalculateNumberOrders(Year);

        }



    }

}

