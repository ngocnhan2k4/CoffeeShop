using CoffeeShop.Models;
using CoffeeShop.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopTests.Service.DataAccess
{
    [TestClass]
    public class MockDaoTests
    {
        private MockDao dao;

        [TestInitialize]
        public void Setup()
        {
            dao = new MockDao();
        }

        [TestMethod]
        public void GetCategories_ReturnsValidCategories()
        {
            var categories = dao.GetCategories();
            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.Count ==5 );
        }



        [TestMethod]
        public void GetDeliveryInvoices_ReturnsValidDeliveryInvoices()
        {
            var deliveryInvoices = dao.GetDeliveryInvoices();
            Assert.IsNotNull(deliveryInvoices);
            Assert.IsTrue(deliveryInvoices.Count == 3);
        }

        [TestMethod]
        public void GetDetailInvoices_ReturnsValidDetailInvoices()
        {
            var detailInvoices = dao.GetDetailInvoices();
            Assert.IsNotNull(detailInvoices);
            Assert.IsTrue(detailInvoices.Count == 5);
        }

        [TestMethod]
        public void GetDrinks_NoFilters_ReturnsAllDrinks()
        {
            var drinks = dao.GetDrinks();
            Assert.IsNotNull(drinks);
            Assert.IsTrue(drinks.Count == 16);
        }

        [TestMethod]
        public void GetDrinks_WithFilters_ReturnsFilteredDrinks()
        {
            int page = 1;
            int rowsPerPage = 8;
            string keyword = "c";
            int categoryID = 1;
            Dictionary<string, IDao.SortType> sortOptions = new Dictionary<string, IDao.SortType>
            {
                { "Price", IDao.SortType.Descending }
            };
            var drinks = dao.GetDrinks(page, rowsPerPage, keyword, categoryID, sortOptions);
            Assert.IsNotNull(drinks);
            Assert.IsTrue(drinks.Item2 == 3);
        }

        [TestMethod]
        public void GetInvoices_ReturnsValidInvoices()
        {
            var invoices = dao.GetInvoices();
            Assert.IsNotNull(invoices);
            Assert.IsTrue(invoices.Count == 5);
        }

        
        [TestMethod]
        public void CalculateNumberOrders_Year2023_ReturnsValidOrderCount()
        {
            var year = 2023;
            var result = dao.CalculateNumberOrders(year);
            Assert.IsTrue(result == 2);
        }

        [TestMethod]
        public void CalculateTotalCost_ReturnsValidTotalCost()
        {
            var result = dao.CalculateTotalCost();
            Assert.IsTrue(result == 16345000);
        }

        [TestMethod]
        public void CalculateRevenue_Year2023_ReturnsValidRevenue()
        {
            var year = 2023;
            var result = dao.CalculateRevenue(year);
            Assert.IsTrue(result == 42000);
        }

        [TestMethod]
        public void CalculateProfit_Year2023_ReturnsValidProfit()
        {
            var year = 2023;
            var result = dao.CalculateProfit(year);
            Assert.IsTrue(result == -16303000);
        }

        [TestMethod]
        public void CalculateYears_ReturnsValidYears()
        {
            var years = dao.CalculateYears();
            Assert.IsNotNull(years);
            Assert.IsTrue(years.Count == 2);
        }

        [TestMethod]
        public void CalculateMonthlyRevenue_Year2023_ReturnsValidMonthlyRevenue()
        {
            var year = 2023;
            var monthlyRevenue = dao.CalculateMonthlyRevenue(year);
            Assert.IsNotNull(monthlyRevenue);
            Assert.AreEqual(12, monthlyRevenue.Count);
        }

        [TestMethod]
        public void CalculateTopDrinks_Year2023_ReturnsValidTopDrinks()
        {
            var year = 2023;
            var topDrinks = dao.CalculateTopDrinks(year);
            Assert.IsNotNull(topDrinks);
            Assert.IsTrue(topDrinks.Count == 1);
        }

        [TestMethod]
        public void CalculateRevenueCategory_Year2023_ReturnsValidRevenueByCategory()
        {
            var year = 2023;
            var revenueCategory = dao.CalculateRevenueCategory(year);
            Assert.IsNotNull(revenueCategory);
            Assert.IsTrue(revenueCategory.Count == 5);
        }

        [TestMethod]
        public void GetDetailInvoicesOfId_InvoiceId1_ReturnsValidDetails()
        {
            var invoiceId = 1;
            var result = dao.GetDetailInvoicesOfId(invoiceId);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Item1);
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void GetRecentInvoices_ReturnsValidRecentInvoices()
        {
            var recentInvoices = dao.GetRecentInvoice(2024);
            Assert.IsNotNull(recentInvoices);
            Assert.IsTrue(recentInvoices.Count == 3);
        }

    }
}
