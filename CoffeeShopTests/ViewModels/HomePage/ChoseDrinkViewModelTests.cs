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
        public void ChoseDrinkViewModel_Initialization_SetsDefaultValues()
        {
            var viewModel = new ChoseDrinkViewModel();
            Assert.IsNotNull(viewModel.ChosenDrinks);
            Assert.AreEqual(0, viewModel.TotalPrice);
        }

        [TestMethod()]
        public void AddDrink_WithValidDrinkAndSize_AddsDrinkToChosenDrinks()
        {
            var viewModel = new ChoseDrinkViewModel();
            var drink = new Drink { Name = "Latte" };
            var size = new Size { Name = "Medium", Price = 5000, Stock = 2 };

            viewModel.AddDrink(drink, size);

            Assert.AreEqual(1, viewModel.ChosenDrinks.Count);
            Assert.AreEqual("Latte", viewModel.ChosenDrinks[0].NameDrink);
            Assert.AreEqual("Medium", viewModel.ChosenDrinks[0].Size);
            Assert.AreEqual(1, viewModel.ChosenDrinks[0].Quantity);
            Assert.AreEqual(5000, viewModel.TotalPrice);
        }

        [TestMethod()]
        public void AddDrink_WithZeroStock_DoesNotAddDrink()
        {
            var viewModel = new ChoseDrinkViewModel();
            var drink = new Drink { Name = "Latte" };
            var size = new Size { Name = "Medium", Price = 5000, Stock = 0 };

            viewModel.AddDrink(drink, size);

            Assert.AreEqual(0, viewModel.ChosenDrinks.Count);
            Assert.AreEqual(0, viewModel.TotalPrice);
        }

        [TestMethod()]
        public void AddDrink_WithExistingDrink_IncreasesQuantity()
        {
            var viewModel = new ChoseDrinkViewModel();
            var drink = new Drink { Name = "Latte" };
            var size = new Size { Name = "Medium", Price = 5000, Stock = 2 };

            viewModel.AddDrink(drink, size);
            viewModel.AddDrink(drink, size);

            Assert.AreEqual(1, viewModel.ChosenDrinks.Count);
            Assert.AreEqual(2, viewModel.ChosenDrinks[0].Quantity);
            Assert.AreEqual(10000, viewModel.TotalPrice);
        }

        [TestMethod()]
        public void AddDrink_WithExistingDrinkAndMaxStock_DoesNotIncreaseQuantity()
        {
            var viewModel = new ChoseDrinkViewModel();
            var drink = new Drink { Name = "Latte" };
            var size = new Size { Name = "Medium", Price = 5000, Stock = 1 };

            viewModel.AddDrink(drink, size);
            viewModel.AddDrink(drink, size);

            Assert.AreEqual(1, viewModel.ChosenDrinks.Count);
            Assert.AreEqual(1, viewModel.ChosenDrinks[0].Quantity);
            Assert.AreEqual(5000, viewModel.TotalPrice);
        }

        [TestMethod()]
        public void RemoveDrink_WithValidDetail_RemovesDrinkFromChosenDrinks()
        {
            var viewModel = new ChoseDrinkViewModel();
            var drink = new Drink { Name = "Latte" };
            var size = new Size { Name = "Medium", Price = 5000, Stock = 2 };

            viewModel.AddDrink(drink, size);
            var detailInvoice = viewModel.ChosenDrinks[0];

            viewModel.RemoveDrink(detailInvoice);

            Assert.AreEqual(0, viewModel.ChosenDrinks.Count);
            Assert.AreEqual(0, viewModel.TotalPrice);
        }

        
        [TestMethod()]
        public void GetMemberCards_ReturnValidNumberMemberCards()
        {
            var viewModel = new ChoseDrinkViewModel();
            var num = viewModel.GetMemberCards().Count;
            Assert.AreEqual(3, num);
        }

        
        [TestMethod()]
        public void GetCustomers_ReturnValidCustomers()
        {
            var viewModel = new ChoseDrinkViewModel();
            var customers = viewModel.GetCustomers();
            Assert.IsTrue(customers.Count > 0);
        }

       

        [TestMethod()]
        public void CalcTotal_WithMultipleDrinks_CalculatesCorrectTotal()
        {
            var viewModel = new ChoseDrinkViewModel();
            var drink1 = new Drink { Name = "Latte" };
            var size1 = new Size { Name = "Medium", Price = 5000, Stock = 2 };
            var drink2 = new Drink { Name = "Espresso" };
            var size2 = new Size { Name = "Small", Price = 3000, Stock = 3 };

            viewModel.AddDrink(drink1, size1);
            viewModel.AddDrink(drink2, size2);
            viewModel.AddDrink(drink2, size2);

            Assert.AreEqual(2, viewModel.ChosenDrinks.Count);
            Assert.AreEqual(11000, viewModel.TotalPrice);
        }

        

        
    }

}