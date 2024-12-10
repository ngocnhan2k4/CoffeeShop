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
    public class SqlServerDaoTests
    {
        private SqlServerDao dao;

        [TestInitialize]
        public void Setup()
        {
            dao = new SqlServerDao();
        }

        [TestMethod]
        public void GetCategories_ReturnsValidCategories()
        {
            var categories = dao.GetCategories();
            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.Count > 0);
        }

        [TestMethod]
        public void AddCategories_WithValidData_ShouldSucceed()
        {
            var l = dao.GetCategories();
            var categories = new List<Category>
            {
                new Category { CategoryID = l.Count, CategoryName = "Test cate" },
            };
            var result = dao.AddCategories(categories);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetDeliveryInvoices_ReturnsValidDeliveryInvoices()
        {
            var deliveryInvoices = dao.GetDeliveryInvoices();
            Assert.IsNotNull(deliveryInvoices);
            Assert.IsTrue(deliveryInvoices.Count > 0);
        }

        [TestMethod]
        public void GetDetailInvoices_ReturnsValidDetailInvoices()
        {
            var detailInvoices = dao.GetDetailInvoices();
            Assert.IsNotNull(detailInvoices);
            Assert.IsTrue(detailInvoices.Count > 0);
        }

        [TestMethod]
        public void GetDrinks_NoFilters_ReturnsAllDrinks()
        {
            var drinks = dao.GetDrinks();
            Assert.IsNotNull(drinks);
            Assert.IsTrue(drinks.Count > 0);
        }

        [TestMethod]
        public void GetDrinks_WithFilters_ReturnsFilteredDrinks()
        {
            int page = 1;
            int rowsPerPage = 2;
            string keyword = "c";
            int categoryID = 1;
            Dictionary< string, IDao.SortType > sortOptions = new Dictionary<string, IDao.SortType>
            {
                { "Price", IDao.SortType.Descending }
            };
            var drinks = dao.GetDrinks(page,rowsPerPage,keyword,categoryID,sortOptions);
            Assert.IsNotNull(drinks);
            Assert.IsTrue(drinks.Item2 >= 0);
        }

        [TestMethod]
        public void GetInvoices_ReturnsValidInvoices()
        {
            var invoices = dao.GetInvoices();
            Assert.IsNotNull(invoices);
            Assert.IsTrue(invoices.Count > 0);
        }

        [TestMethod]
        public void AddDrinks_ValidInput_ReturnsTrue()
        {
            var drinks = new List<Drink>
            {
                new Drink
                {
                    Name = "Test Drink",
                    CategoryID = 1,
                    Description = "Test Description",
                    ImageString = "Test Image",
                    Sizes = new List<Size>
                    {
                        new Size { Name = "S", Price = 100, Stock = 10 },
                       
                    }
                }
            };
            var result = dao.AddDrinks(drinks);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CalculateNumberOrders_Year2023_ReturnsValidOrderCount()
        {
            var year = 2023;
            var result = dao.CalculateNumberOrders(year);
            Assert.IsTrue(result >= 0);
        }

        [TestMethod]
        public void CalculateTotalCost_ReturnsValidTotalCost()
        {
            var result = dao.CalculateTotalCost();
            Assert.IsTrue(result >= 0);
        }

        [TestMethod]
        public void CalculateRevenue_Year2023_ReturnsValidRevenue()
        {
            var year = 2023;
            var result = dao.CalculateRevenue(year);
            Assert.IsTrue(result >= 0);
        }

        [TestMethod]
        public void CalculateProfit_Year2023_ReturnsValidProfit()
        {
            var year = 2023;
            var result = dao.CalculateProfit(year);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CalculateYears_ReturnsValidYears()
        {
            var years = dao.CalculateYears();
            Assert.IsNotNull(years);
            Assert.IsTrue(years.Count > 0);
        }

        [TestMethod]
        public void CalculateMonthlyRevenue_Year2023_ReturnsMonthlyRevenueForAllMonths()
        {
            var year = 2023;
            var monthlyRevenue = dao.CalculateMonthlyRevenue(year);
            Assert.IsNotNull(monthlyRevenue);
            Assert.AreEqual(12, monthlyRevenue.Count);
        }

        [TestMethod]
        public void CalculateTopDrinks_Year2023_ReturnsTopSellingDrinks()
        {
            var year = 2023;
            var topDrinks = dao.CalculateTopDrinks(year);
            Assert.IsNotNull(topDrinks);
            Assert.IsTrue(topDrinks.Count > 0);
        }

        [TestMethod]
        public void CalculateRevenueCategory_Year2023_ReturnsRevenueByCategory()
        {
            var year = 2023;
            var revenueCategory = dao.CalculateRevenueCategory(year);
            Assert.IsNotNull(revenueCategory);
            Assert.IsTrue(revenueCategory.Count > 0);
        }


        [TestMethod]
        public void GetDetailInvoicesOfId_ValidInvoiceId_ReturnsInvoiceDetails()
        {
            var invoiceId = 1; 
            var result = dao.GetDetailInvoicesOfId(invoiceId);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Item1);
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void UpdateInvoiceStatus_ValidInvoiceIdAndStatus_UpdatesSuccessfully()
        {
            var invoiceId = 1; 
            var status = "Paid";
            dao.UpdateInvoiceStatus(invoiceId, status);

        }

        [TestMethod]
        public void AddInvoice_ValidData_ShouldSucceed()
        {
            var invoice = new Invoice
            {
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd"),
                TotalAmount = 1000,
                PaymentMethod = "Cash",
                Status = "Pending",
                CustomerName = "Test Customer",
                HasDelivery = "N"
            };
            var detailInvoices = new List<DetailInvoice>
            {
                new DetailInvoice { DrinkId = 1, Quantity = 2, Price = 500 } 
            };
            var deliveryInvoice = new DeliveryInvoice
            {
                Address = "Test Address",
                PhoneNumber = "1234567890",
                ShippingFee = 50
            };
            dao.AddInvoice(invoice, detailInvoices, deliveryInvoice);
        }
    }
}
