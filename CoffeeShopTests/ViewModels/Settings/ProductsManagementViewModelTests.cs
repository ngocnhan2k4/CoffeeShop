using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeShop.ViewModels.Settings;
using CoffeeShop.Models;
using CoffeeShop.Service.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace CoffeeShopTests.ViewModels.Settings
{
    [TestClass]
    public class ProductsManagementViewModelTests
    {
        private ProductsManagementViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {

            _viewModel = new ProductsManagementViewModel();
        }

        [TestMethod]
        public void LoadData_InitializesPropertiesCorrectly()
        {
            // Assert
            Assert.AreEqual(0, _viewModel.SelectedCategoryIndex);
            Assert.IsNotNull(_viewModel.NameSizes);
            Assert.IsNotNull(_viewModel.Drinks);
            Assert.IsNotNull(_viewModel.Categories);
            Assert.IsNotNull(_viewModel.NewDrinks);
            Assert.IsNotNull(_viewModel.NewCategories);
            Assert.IsNotNull(_viewModel.DrinksByCategoryID);
        }

       

        [TestMethod]
        public void AddDrink_WithValidDrink_ReturnsTrue()
        {
            // Arrange
            var drink = new Drink
            {
                Name = "Test Drink",
                Description = "Test Description",
                Sizes = new List<Size>
                {
                    new Size { Name = "S", Price = 10, Stock = 10 }
                }
            };
            _viewModel.NewDrinkAdded = drink;

            // Act
            bool result = _viewModel.AddDrink();

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(_viewModel.Drinks.Contains(drink));
            Assert.IsTrue(_viewModel.DrinksByCategoryID.Contains(drink));
            Assert.IsTrue(_viewModel.NewDrinks.Contains(drink));
        }

        [TestMethod]
        public void AddDrink_WithInvalidDrink_ReturnsFalse()
        {
            // Arrange
            var drink = new Drink
            {
                Name = "", // Invalid name
                Description = "Test Description",
                Sizes = new List<Size>
                {
                    new Size { Name = "S", Price = 10, Stock = 10 }
                }
            };
            _viewModel.NewDrinkAdded = drink;

            // Act
            bool result = _viewModel.AddDrink();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Name cannot be empty.", _viewModel.Error);
        }

        [TestMethod]
        public void AddCategory_WithValidName_ReturnsTrue()
        {
            // Arrange
            string categoryName = "Test Category";

            // Act
            bool result = _viewModel.AddCategory(categoryName);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(_viewModel.NewCategories.Any(c => c.CategoryName == categoryName));
        }

        [TestMethod]
        public void AddCategory_WithEmptyName_ReturnsFalse()
        {
            // Act
            bool result = _viewModel.AddCategory("");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Category cannot be empty.", _viewModel.Error);
        }

        [TestMethod]
        public void EditDrink_WithValidDrink_ReturnsTrue()
        {
            // Arrange
            var drink = new Drink
            {
                ID = 1,
                Name = "Original Drink",
                Description = "Original Description",
                Sizes = new List<Size>
                {
                    new Size { Name = "S", Price = 10, Stock = 10 }
                }
            };
            _viewModel.Drinks.Add(drink);
            _viewModel.DrinksByCategoryID.Add(drink);

            var editedDrink = new Drink
            {
                ID = 1,
                Name = "Edited Drink",
                Description = "Edited Description",
                Sizes = new List<Size>
                {
                    new Size { Name = "S", Price = 15, Stock = 15 }
                }
            };
            _viewModel.SelectedEditDrink = editedDrink;

            // Act
            bool result = _viewModel.EditDrink();

            // Assert
            Assert.IsTrue(result);
            var updatedDrink = _viewModel.Drinks.First(d => d.ID == 1);
            Assert.AreEqual("Edited Drink", updatedDrink.Name);
            Assert.AreEqual("Edited Description", updatedDrink.Description);
        }

        [TestMethod]
        public void EditDrink_WithInvalidDrink_ReturnsFalse()
        {
            // Arrange
            var drink = new Drink
            {
                ID = 1,
                Name = "", // Invalid name
                Description = "Test Description",
                Sizes = new List<Size>
                {
                    new Size { Name = "S", Price = 10, Stock = 10 }
                }
            };
            _viewModel.SelectedEditDrink = drink;

            // Act
            bool result = _viewModel.EditDrink();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Name cannot be empty.", _viewModel.Error);
        }

        [TestMethod]
        public void EditDrink_WithNonExistentDrink_ReturnsFalse()
        {
            // Arrange
            var drink = new Drink
            {
                ID = 999, // Non-existent ID
                Name = "Test Drink",
                Description = "Test Description",
                Sizes = new List<Size>
                {
                    new Size { Name = "S", Price = 10, Stock = 10 }
                }
            };
            _viewModel.SelectedEditDrink = drink;

            // Act
            bool result = _viewModel.EditDrink();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateDrink_WithInvalidPrice_ReturnsFalse()
        {
            // Arrange
            var drink = new Drink
            {
                Name = "Test Drink",
                Description = "Test Description",
                Sizes = new List<Size>
                {
                    new Size { Name = "S", Price = -10, Stock = 10 } // Invalid price
                }
            };
            _viewModel.NewDrinkAdded = drink;

            // Act
            bool result = _viewModel.AddDrink();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Price for size 'S' must be greater than 0.", _viewModel.Error);
        }

        [TestMethod]
        public void ValidateDrink_WithInvalidStock_ReturnsFalse()
        {
            // Arrange
            var drink = new Drink
            {
                Name = "Test Drink",
                Description = "Test Description",
                Sizes = new List<Size>
                {
                    new Size { Name = "S", Price = 10, Stock = -5 } // Invalid stock
                }
            };
            _viewModel.NewDrinkAdded = drink;

            // Act
            bool result = _viewModel.AddDrink();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Stock for size 'S' cannot be negative.", _viewModel.Error);
        }

        [TestMethod]
        public void ValidateDrink_WithEmptyDescription_ReturnsFalse()
        {
            // Arrange
            var drink = new Drink
            {
                Name = "Test Drink",
                Description = "", // Empty description
                Sizes = new List<Size>
                {
                    new Size { Name = "S", Price = 10, Stock = 10 }
                }
            };
            _viewModel.NewDrinkAdded = drink;

            // Act
            bool result = _viewModel.AddDrink();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Description cannot be empty.", _viewModel.Error);
        }

        [TestMethod]
        public void UpdateDrinksAndCategoriesIntoDB_Success_ReturnsTrue()
        {
            // Arrange
            var drink = new Drink
            {
                ID = 1,
                Name = "Test Drink",
                Description = "Test Description",
                CategoryID = 0,
                Sizes = new List<Size> { new Size { Name = "S", Price = 10, Stock = 10 } }
            };
            _viewModel.NewDrinks.Add(drink);
            _viewModel.Drinks.Add(drink);
            _viewModel.DrinksByCategoryID.Add(drink);

            int categoryID = _viewModel.Categories.Count;
            var category = new Category { CategoryID = categoryID, CategoryName = "Test Category" };
            _viewModel.NewCategories.Add(category);
            _viewModel.Categories.Add(category);

            // Act
            bool result = _viewModel.UpdateDrinksAndCategoriesIntoDB();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, _viewModel.NewDrinks.Count);
            Assert.AreEqual(0, _viewModel.NewCategories.Count);
            Assert.IsTrue(_viewModel.Drinks.Contains(drink));
            Assert.IsTrue(_viewModel.DrinksByCategoryID.Contains(drink));
            Assert.IsTrue(_viewModel.Categories.Contains(category));
        }

        [TestMethod]
        public void UpdateDrinksAndCategoriesIntoDB_FailedToAddDrinks_ReturnsFalse()
        {
            // Arrange
            var drink = new Drink
            {
                Name = "Test Drink",
                Description = "Test Description",
                CategoryID = -1, // Invalid category ID to force failure since it references category table
                Sizes = new List<Size> { new Size { Name = "S", Price = 10, Stock = 10 } },
                ImageString = ""
            };
            _viewModel.Drinks.Add(drink);
            _viewModel.DrinksByCategoryID.Add(drink);
            _viewModel.NewDrinks.Add(drink);

            // Act
            bool result = _viewModel.UpdateDrinksAndCategoriesIntoDB();

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(_viewModel.Drinks.Contains(drink));
            Assert.IsFalse(_viewModel.DrinksByCategoryID.Contains(drink));
            Assert.AreEqual(0, _viewModel.NewDrinks.Count);
        }

        [TestMethod]
        public void UpdateDrinksAndCategoriesIntoDB_FailedToAddCategories_ReturnsFalse()
        {
            // Arrange
            var category = new Category { CategoryID = int.MinValue, CategoryName = "Test Category" }; // Use MinValue instead of MaxValue+1
            _viewModel.Categories.Add(category);
            _viewModel.NewCategories.Add(category);

            // Act
            bool result = _viewModel.UpdateDrinksAndCategoriesIntoDB();

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(_viewModel.Categories.Contains(category));
            Assert.AreEqual(0, _viewModel.NewCategories.Count);
        }

        [TestMethod]
        public void FilterDrinksByCategoryID_ReturnsCorrectDrinks()
        {
            // Arrange
            var drink1 = new Drink { ID = 1, CategoryID = 0, Name = "Drink 1" };
            var drink2 = new Drink { ID = 2, CategoryID = 1, Name = "Drink 2" };
            _viewModel.Drinks = new List<Drink> { drink1, drink2 };

            // Act
            var result = _viewModel.FilterDrinksByCategoryID(0);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Drink 1", result[0].Name);
        }

        [TestMethod]
        public void SelectedCategoryIndex_UpdatesDrinksByCategoryID()
        {
            // Arrange
            var drink1 = new Drink { ID = 1, CategoryID = 0, Name = "Drink 1" };
            var drink2 = new Drink { ID = 2, CategoryID = 1, Name = "Drink 2" };
            _viewModel.Drinks = new List<Drink> { drink1, drink2 };

            // Act
            _viewModel.SelectedCategoryIndex = 1;

            // Assert
            Assert.AreEqual(1, _viewModel.DrinksByCategoryID.Count);
            Assert.AreEqual("Drink 2", _viewModel.DrinksByCategoryID[0].Name);
        }

        [TestMethod]
        public void ClearError_ResetsErrorMessage()
        {
            // Arrange
            _viewModel.Error = "Some error";

            // Act
            _viewModel.ClearError();

            // Assert
            Assert.AreEqual("", _viewModel.Error);
        }
    }
}
