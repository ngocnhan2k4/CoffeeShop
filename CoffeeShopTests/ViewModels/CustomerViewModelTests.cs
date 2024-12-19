using CoffeeShop.Service.DataAccess;
using CoffeeShop.Service;
using CoffeeShop.ViewModels;
using CoffeeShop.ViewModels.HomePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Models;

namespace CoffeeShopTests.ViewModels
{
    [TestClass]
    public class CustomerViewModelTests
    {
        private CustomerViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            ServiceFactory.Register(typeof(IDao), typeof(SqlServerDao));
            _viewModel = new CustomerViewModel();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            Assert.AreEqual(8, _viewModel.RowsPerPage);
            Assert.AreEqual(1, _viewModel.CurrentPage);
        }

        [TestMethod]
        public void GoToPage_ShouldUpdateCurrentPageAndLoadData()
        {
            _viewModel.GoToPage(1);
            Assert.AreEqual(1, _viewModel.CurrentPage);
            Assert.IsNotNull(_viewModel.Customers);
        }

        [TestMethod]
        public void GoToNextPage_ShouldIncrementCurrentPageAndLoadData()
        {
            _viewModel.TotalPages = 2;
            _viewModel.GoToNextPage();
            Assert.AreEqual(2, _viewModel.CurrentPage);
            Assert.IsNotNull(_viewModel.Customers);
        }

        [TestMethod]
        public void GoToPreviousPage_ShouldDecrementCurrentPageAndLoadData()
        {
            _viewModel.CurrentPage = 2;
            _viewModel.GoToPreviousPage();
            Assert.AreEqual(1, _viewModel.CurrentPage);
            Assert.IsNotNull(_viewModel.Customers);
        }

        [TestMethod]
        public void Search_ShouldUpdateKeywordAndLoadData()
        {
            _viewModel.Search("Test");
            Assert.AreEqual("Test", _viewModel.Keyword);
            Assert.IsNotNull(_viewModel.Customers);
        }

        [TestMethod]
        public void AddCustomer_ShouldAddCustomerAndLoadData()
        {
            var newCustomer = new Customer { customerName = "New Customer", totalMonney=0, totalPoint=0, type="Thẻ thành viên" };
            var count = _viewModel.Customers.Count;

            _viewModel.AddCustomer(newCustomer);

            Assert.AreEqual(count + 1, _viewModel.Customers.Count);
            Assert.IsNotNull(_viewModel.Customers);
        }

        [TestMethod]
        public void UpdateCustomer_ShouldUpdateCustomerAndLoadData()
        {
            var customer = new Customer { customerName = "Updated Customer", customerID = _viewModel.Customers[_viewModel.Customers.Count-1].customerID  };

            _viewModel.UpdateCustomer(customer);

            Assert.AreEqual("Updated Customer", _viewModel.Customers[_viewModel.Customers.Count - 1].customerName);
            Assert.IsNotNull(_viewModel.Customers);
        }

        [TestMethod]
        public void DeleteCustomer_ShouldDeleteCustomerAndLoadData()
        {
            var id = _viewModel.Customers[_viewModel.Customers.Count - 1].customerID;

            var count = _viewModel.Customers.Count;

            _viewModel.DeleteCustomer(id);

            Assert.AreEqual(count - 1, _viewModel.Customers.Count);
            Assert.IsNotNull(_viewModel.Customers);
        }
    }
}
