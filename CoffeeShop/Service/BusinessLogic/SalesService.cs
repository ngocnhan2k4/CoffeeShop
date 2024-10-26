using CoffeeShop.Service.DataAccess;
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
        public List<double> MonthlyRevenue { get; set; } // This variable is used to store the revenue of the shop in each month
        public List<string> TopDrinks { get; set; } // This variable is used to store the top 5 drinks that are sold the most
        public int NumberOrders { get; set; } // This variable is used to store the number of orders in a year
        public Dictionary<string, double> RevenueByCategory { get; set; } // This variable is used to store the revenue of the shop by category
        public IDao dao { get; set; } // This variable is used to access the data access layer

        
        public SalesService(IDao dao, int Year)// Constructor of the class
        {
            this.dao = dao;
            TotalCost = CalculateTotalCost(dao);
            Revenue = CalculateRevennue(dao,Year);
            Profit = CalculateProfit(dao, Year);
            Years = CalculateYears(dao);
            MonthlyRevenue = CalculateMonthlyRevenue(dao, Year);
            TopDrinks = CalculateTopDrinks(dao,Year);
            RevenueByCategory = CalculateRevenueCategory(dao, Year);
            NumberOrders = CalculateNumberOrders(dao,Year);
        }

        
        public static int CalculateNumberOrders(IDao dao, int year)// This method is used to calculate the number of orders in a year
        {
            int total = 0;
            var invoices = dao.GetInvoices();
            foreach (var invoice in invoices)
            {
                if (Convert.ToDateTime(invoice.CreatedAt).Year == year)
                {
                    total++;
                }
            }
            return total;
        }

        public static double CalculateTotalCost(IDao dao) // This method is used to calculate the total cost of all drinks in the shop
        {
            var drinks = dao.GetDrinks();
            double result = 0;
            //foreach (var drink in drinks)
            //{
            //    result += drink.OriginalPrice*drink.Stock;
            //}
            return result;
        }
        public static double CalculateRevennue(IDao dao, int year) // This method is used to calculate the revenue of the shop in a year
        {
            var invoices = dao.GetInvoices();
            double result = 0;
            foreach (var invoice in invoices)
            {
                if (Convert.ToDateTime(invoice.CreatedAt).Year == year)
                    result += invoice.TotalAmount;
            }
            return result;
        }
        public static double CalculateProfit(IDao dao,int year)// This method is used to calculate the profit of the shop in a year
        {
            return CalculateRevennue(dao,year) - CalculateTotalCost(dao);
        }

        public static List<int> CalculateYears(IDao dao)// This method is used to calculate the years that the shop has sales
        {
            List<int> years = new List<int>();
            var invoices = dao.GetInvoices();
            foreach (var invoice in invoices)
            {
                int year = Convert.ToDateTime(invoice.CreatedAt).Year;
                if (!years.Contains(year))
                {
                    years.Add(year);
                }
            }
            return  new() { years.Max(), years.Min() };
        }

        public static List<double> CalculateMonthlyRevenue(IDao dao, int year) // This method is used to calculate the revenue of the shop in each month
        {
            List<double> result = new();
            var invoices = dao.GetInvoices();
            for (int i = 1; i <= 12; i++)
            {
                double revenue = 0;
                foreach (var invoice in invoices)
                {
                    if (Convert.ToDateTime(invoice.CreatedAt).Month == i && (Convert.ToDateTime(invoice.CreatedAt).Year == year))
                    {
                        revenue += invoice.TotalAmount;
                    }
                }
                result.Add(revenue);
            }
            return result;
        }
        public List<string> CalculateTopDrinks(IDao dao, int year)  // This method is used to calculate the top 5 drinks that are sold the most
        {
            List<string> result = new List<string>();
            var detailInvoices = dao.GetDetailInvoices();
            var invoices = dao.GetInvoices(); 
            var drinks = dao.GetDrinks();
            Dictionary<string, int> drinkSold = new Dictionary<string, int>();

         
            var invoicesInYear = invoices.Where(invoice => Convert.ToDateTime(invoice.CreatedAt).Year == year).ToList();

            foreach (var invoice in invoicesInYear)
            {
                var detailInvoiceList = detailInvoices.Where(di => di.InvoiceID == invoice.InvoiceID);

                foreach (var detailInvoice in detailInvoiceList)
                {
                    var drinkName = drinks.Find(x => x.Name == detailInvoice.NameDrink)?.Name;
                    if (drinkName != null)
                    {
                        if (drinkSold.ContainsKey(drinkName))
                        {
                            drinkSold[drinkName] += detailInvoice.Quantity;
                        }
                        else
                        {
                            drinkSold.Add(drinkName, detailInvoice.Quantity);
                        }
                    }
                }
            }
            var sortedDrinkSold = drinkSold.OrderByDescending(x => x.Value).Take(5);
            foreach (var drink in sortedDrinkSold)
            {
                result.Add(drink.Key);
            }
            return result;
        }


        public static Dictionary<string, double> CalculateRevenueCategory(IDao dao, int year)// This method is used to calculate the revenue of the shop by category
        {
            var detailInvoices = dao.GetDetailInvoices();
            var drinks = dao.GetDrinks();
            var categories = dao.GetCategories();
            var invoices = dao.GetInvoices();

           
            Dictionary<string, double> revenueByCategory = new Dictionary<string, double>();
            foreach(var category in categories)
            {
                revenueByCategory.Add(category.CategoryName, 0);
            }
            //var invoicesInYear = invoices.Where(invoice => Convert.ToDateTime(invoice.CreatedAt).Year == year).ToList();

            //for (int i = 0; i < invoicesInYear.Count; i++)
            //{
            //    for (int j = 0; j < detailInvoices.Count; j++)
            //    {
            //        if (invoicesInYear[i].InvoiceID == detailInvoices[j].InvoiceID)
            //        {
            //            var drink = drinks.Find(x => x.Name == detailInvoices[j].NameDrink && x.Size == detailInvoices[j].Size);
            //            var category = categories.Find(x => x.CategoryID == drink.CategoryID);
            //            revenueByCategory[category.CategoryName] += detailInvoices[j].Quantity * drink.SalePrice;
            //        }
            //    }
            //}
                return revenueByCategory;
        }

    }

}

