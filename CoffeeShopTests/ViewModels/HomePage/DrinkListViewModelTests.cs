using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeShop.ViewModels.HomePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Models;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.Service;
using CoffeeShop.ViewModels.Settings;

namespace CoffeeShop.ViewModels.HomePage.Tests
{
    [TestClass]
    public class DrinkListViewModelTests
    {
        private DrinkListViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            ServiceFactory.Register(typeof(IDao), typeof(SqlServerDao));
            _viewModel = new DrinkListViewModel();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            Assert.IsNotNull(_viewModel.Categories);
            Assert.AreEqual(8, _viewModel.RowsPerPage);
            Assert.AreEqual(1, _viewModel.CurrentPage);
        }


        [TestMethod]
        public void CategoryID_Setter_ShouldUpdateCategoryIDAndLoadData()
        {
            _viewModel.CategoryID = 5;
            Assert.AreEqual(5, _viewModel.CategoryID);
            Assert.IsNotNull(_viewModel.Drinks);
        }

        [TestMethod]
        public void SortBy_Setter_ShouldUpdateSortOptionsAndLoadData()
        {
            _viewModel.SortBy = "PriceIncrease";
            Assert.AreEqual("PriceIncrease", _viewModel.SortBy);
            Assert.IsNotNull(_viewModel.Drinks);
        }

        [TestMethod]
        public void GoToPage_ShouldUpdateCurrentPageAndLoadData()
        {
            _viewModel.GoToPage(2);
            Assert.AreEqual(2, _viewModel.CurrentPage);
            Assert.IsNotNull(_viewModel.Drinks);
        }

        [TestMethod]
        public void GoToNextPage_ShouldIncrementCurrentPageAndLoadData()
        {
            _viewModel.TotalPages = 3;
            _viewModel.GoToNextPage();
            Assert.AreEqual(2, _viewModel.CurrentPage);
            Assert.IsNotNull(_viewModel.Drinks);
        }

        [TestMethod]
        public void GoToPreviousPage_ShouldDecrementCurrentPageAndLoadData()
        {
            _viewModel.CurrentPage = 2;
            _viewModel.GoToPreviousPage();
            Assert.AreEqual(1, _viewModel.CurrentPage);
            Assert.IsNotNull(_viewModel.Drinks);
        }

        [TestMethod]
        public void Search_ShouldUpdateKeywordAndLoadData()
        {
            _viewModel.Search("coffee");
            Assert.AreEqual("coffee", _viewModel.Keyword);
            Assert.IsNotNull(_viewModel.Drinks);
        }
    }
}