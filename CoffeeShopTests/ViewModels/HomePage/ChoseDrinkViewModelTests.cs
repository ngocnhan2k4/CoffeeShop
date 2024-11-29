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

namespace CoffeeShop.ViewModels.HomePage.Tests
{
    [TestClass()]
    public class ChoseDrinkViewModelTests
    {
        [TestMethod()]
        public void ChoseDrinkViewModelTest()
        {
            ServiceFactory.Register(typeof(IDao), typeof(SqlServerDao));
            var viewModel = new ChoseDrinkViewModel();
            Assert.IsNotNull(viewModel.ChosenDrinks);
            Assert.AreEqual(0, viewModel.TotalPrice);
        }

        [TestMethod()]
        public void AddDrinkTest()
        {
            var viewModel = new ChoseDrinkViewModel();
            var drink = new Drink { Name = "Latte" };
            var size = new Size { Name = "Medium", Price = 5000 };

            viewModel.AddDrink(drink, size);

            Assert.AreEqual(1, viewModel.ChosenDrinks.Count);
            Assert.AreEqual("Latte", viewModel.ChosenDrinks[0].NameDrink);
            Assert.AreEqual("Medium", viewModel.ChosenDrinks[0].Size);
            Assert.AreEqual(1, viewModel.ChosenDrinks[0].Quantity);
            Assert.AreEqual(5000, viewModel.TotalPrice);

            // Add the same drink again
            viewModel.AddDrink(drink, size);

            Assert.AreEqual(1, viewModel.ChosenDrinks.Count);
            Assert.AreEqual(2, viewModel.ChosenDrinks[0].Quantity);
            Assert.AreEqual(10000, viewModel.TotalPrice);
        }

        [TestMethod()]
        public void RemoveDrinkTest()
        {
            var viewModel = new ChoseDrinkViewModel();
            var drink = new Drink { Name = "Latte" };
            var size = new Size { Name = "Medium", Price = 5000 };

            viewModel.AddDrink(drink, size);
            var detailInvoice = viewModel.ChosenDrinks[0];

            viewModel.RemoveDrink(detailInvoice);

            Assert.AreEqual(0, viewModel.ChosenDrinks.Count);
            Assert.AreEqual(0, viewModel.TotalPrice);
        }
    }
}