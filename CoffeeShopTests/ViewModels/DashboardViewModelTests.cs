using CoffeeShop.Models;
using CoffeeShop.Service;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopTests.ViewModels
{
    [TestClass]
    public class DashboardViewModelTests
    {
        [TestMethod]
        public void DashboardViewModelTest()
        {
            // Arrange
            ServiceFactory.Register(typeof(IDao), typeof(SqlServerDao)); 
            var year = 2024;

            // Act
            var viewModel = new DashboardViewModel(year);

            // Assert
            Assert.IsNotNull(viewModel.Drinks);
            Assert.IsNotNull(viewModel.Categories);
            Assert.IsNotNull(viewModel.Invoices);
            Assert.IsNotNull(viewModel.DetailInvoices);
            Assert.IsNotNull(viewModel.DeliveryInvoices);
            Assert.IsNotNull(viewModel.TopDrink);
        }

        [TestMethod]
        public void TopDrinkPropertyTest()
        {
            // Arrange
            ServiceFactory.Register(typeof(IDao), typeof(MockDao));
            var year = 2024;
            var viewModel = new DashboardViewModel(year);

            // Act
            var topDrink = viewModel.TopDrink;

            // Assert
            Assert.IsNotNull(topDrink);
            Assert.AreEqual(3, topDrink.Count); // Số lượng giả lập từ MockDao
        }

        [TestMethod]
        public void DrinksCollectionTest()
        {
            // Arrange
            ServiceFactory.Register(typeof(IDao), typeof(MockDao));
            var viewModel = new DashboardViewModel(2024);

            // Act
            var drinks = viewModel.Drinks;

            // Assert
            Assert.IsNotNull(drinks);
            Assert.AreEqual(5, drinks.Count); // Số lượng giả lập từ MockDao
            Assert.AreEqual("Espresso", drinks[0].Name); // Kiểm tra giá trị giả lập đầu tiên
        }

        [TestMethod]
        public void CalculateTopDrinksTest()
        {
            // Arrange
            ServiceFactory.Register(typeof(IDao), typeof(MockDao));
            var year = 2024;
            var viewModel = new DashboardViewModel(year);

            // Act
            var topDrinks = viewModel.TopDrink;

            // Assert
            Assert.AreEqual(3, topDrinks.Count);
            Assert.AreEqual("Latte", topDrinks[1]); // Kiểm tra thứ tự giả lập
        }
    }
}