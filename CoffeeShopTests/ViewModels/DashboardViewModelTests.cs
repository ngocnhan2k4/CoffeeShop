using CoffeeShop.Models;
using CoffeeShop.Service;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.ViewModels;
using CoffeeShop.ViewModels.HomePage;
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
       private DashboardViewModel viewModel;
        [TestInitialize]
        public void Setup()
        {
            ServiceFactory.Register(typeof(IDao), typeof(SqlServerDao));
            var year = 2024;
            viewModel = new DashboardViewModel(year);
        }

        [TestMethod]
        public void TestDashboardViewModel_InitialState_NotNullCollections()
        {

            // Assert
            Assert.IsNotNull(viewModel.Drinks);
            Assert.IsNotNull(viewModel.Categories);
            Assert.IsNotNull(viewModel.Invoices);
            Assert.IsNotNull(viewModel.DetailInvoices);
            Assert.IsNotNull(viewModel.DeliveryInvoices);
            Assert.IsNotNull(viewModel.TopDrink);
        }

        [TestMethod]
        public void TestTopDrinkProperty_Initialized_HasItems()
        {

            // Act
            var topDrink = viewModel.TopDrink;

            // Assert
            Assert.IsNotNull(topDrink);
            Assert.IsTrue(topDrink.Count > 0); 
        }

        [TestMethod]
        public void TestDrinksCollection_Initialized_CorrectValues()
        {
            // Act
            var drinks = viewModel.Drinks;

            // Assert
            Assert.IsNotNull(drinks);
            Assert.IsTrue(drinks.Count > 0); 
            Assert.AreEqual("Cà Phê Đen", drinks[0].Name);
        }

        [TestMethod]
        public void TestCalculateTopDrinks_Initialized_HasTopItems()
        {
            // Act
            var topDrinks = viewModel.TopDrink;

            // Assert
            Assert.IsTrue(topDrinks.Count > 0);
            Assert.IsNotNull(topDrinks[0]); 
        }

        [TestMethod]
        public void TestPropertyChangedEvent_TopDrink_Triggered()
        {
            bool propertyChangedRaised = false;
            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(viewModel.TopDrink))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            viewModel.TopDrink = new ObservableCollection<string> { "Cà Phê Đen"};

            // Assert
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void TestSaleService_Initialized_NotNull()
        {
            // Assert
            Assert.IsNotNull(viewModel.SaleService);
        }
    }

    [TestClass]
    public class PieChartViewModelTests
    {
        private PieChartViewModel viewModel;

        [TestInitialize]
        public void Setup()
        {
            ServiceFactory.Register(typeof(IDao), typeof(SqlServerDao));
            viewModel = new PieChartViewModel();
        }


        [TestMethod]
        public void TestPieChartViewModel_Initialized_NotNullData()
        {
            // Act
            var data = viewModel.Data;

            // Assert
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestPieChartData_ValidState_HasValidProperties()
        {
            // Act
            var data = viewModel.Data;

            // Assert
            Assert.IsNotNull(data);
            Assert.IsTrue( data.Count >= 0);
            Assert.IsNotNull(data[0].Category);
            Assert.IsNotNull(data[0].SalesAmount);
            
        }
    }


    [TestClass]
    public class CartesianChartViewModelTests
    {
        private CartesianChartViewModel viewModel;

        [TestInitialize]
        public void Setup()
        {
            ServiceFactory.Register(typeof(IDao), typeof(SqlServerDao));
            viewModel = new CartesianChartViewModel();
        }

        [TestMethod]
        public void TestCartesianChartViewModel_Initialized_HasMonthlyData()
        {
            // Assert
            Assert.IsNotNull(viewModel.Data);
            Assert.AreEqual(12, viewModel.Data.Count);
        }

        [TestMethod]
        public void TestCartesianChartData_MonthlyRevenue_MatchesExpectedValues()
        {
            // Act
            var data = viewModel.Data;

            // Assert
            Assert.AreEqual(12, data.Count);
            for (int i = 0; i < 12; i++)
            {
                Assert.AreEqual(i + 1, data[i].Month);
                Assert.AreEqual(viewModel.salesService.MonthlyRevenue[i], data[i].Revenue);
            }
        }
    }

 
}