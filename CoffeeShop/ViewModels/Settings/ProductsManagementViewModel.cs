using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WinRT;

namespace CoffeeShop.ViewModels.Settings
{
    public class ProductsManagementViewModel : INotifyPropertyChanged
    {
        // Dùng để theo dõi những Drink nào người dùng đã edit hoặc thêm mới
        public List<Drink> NewDrinks { get; set; }
        public FullObservableCollection<Category> NewCategories { get; set; }

        public List<Drink> Drinks { get; set; }
        public FullObservableCollection<Drink> DrinksByCategoryID { get; set; }
        public FullObservableCollection<Category> Categories { get; set; }
        public List<string> NameSizes { get; set; }

        // Dùng để theo dõi tabs category đang được chọn
        private int _selectedCategoryIndex;
        public int SelectedCategoryIndex
        {
            get => _selectedCategoryIndex;
            set
            {
                _selectedCategoryIndex = value;
                DrinksByCategoryID = new(FilterDrinksByCategoryID(SelectedCategoryIndex + 1));
            }
        }

        // Dùng để theo dõi Drink đang được edit 
        public Drink SelectedEditDrink {  get; set; }

        public Drink NewDrinkAdded { get; set; }
        public int NewDrinkCategoryID {  get; set; }

        public string Error {  get; set; }

        public ProductsManagementViewModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            _selectedCategoryIndex = 0;
            NameSizes = ["S", "M", "L"];
            IDao dao = new MockDao();
            Drinks = dao.GetAllDrinks();
            Categories = new (dao.GetCategories());
            NewDrinks = [];
            NewCategories = [];
            DrinksByCategoryID = new(FilterDrinksByCategoryID(SelectedCategoryIndex + 1));
        }

        public List<Drink> FilterDrinksByCategoryID(int CategoryID)
        {
            return Drinks.Where(drink => drink.CategoryID == CategoryID).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool EditDrink()
        {
            if (!ValidateDrink(SelectedEditDrink)) return false;         

            int IDOfDrinkEdited = SelectedEditDrink.ID;
            Drink DrinkEditedInList = Drinks.FirstOrDefault(Drink => Drink.ID == IDOfDrinkEdited);
            Drink DrinkEditedInListByCategoryID = DrinksByCategoryID.FirstOrDefault(Drink => Drink.ID == IDOfDrinkEdited);

            if (DrinkEditedInListByCategoryID == null) return false;

            // Edit ở List DrinksByCategoryID và Drinks
            DrinkEditedInListByCategoryID.Name = DrinkEditedInList.Name = SelectedEditDrink.Name;
            DrinkEditedInListByCategoryID.Description = DrinkEditedInList.Description = SelectedEditDrink.Description;
            DrinkEditedInListByCategoryID.Sizes = DrinkEditedInList.Sizes = SelectedEditDrink.Sizes;
            DrinkEditedInListByCategoryID.ImageString = DrinkEditedInList.ImageString = SelectedEditDrink.ImageString;
            DrinkEditedInListByCategoryID.Image = DrinkEditedInList.Image = SelectedEditDrink.Image;

            // Thêm drink dã edit vào list để sau cập nhật trên database
            NewDrinks.Add(DrinkEditedInList);

            return true;
        }

        public bool AddDrink()
        {
            if (!ValidateDrink(NewDrinkAdded)) return false;

            // Thêm ơ cả DrinksByCategoryID và Drinks 
            NewDrinkAdded.CategoryID = SelectedCategoryIndex + 1;
            DrinksByCategoryID.Add(NewDrinkAdded);
            Drinks.Add(NewDrinkAdded);

            // Thêm Drink vừa Add vào mảng NewDrinks đẻ sau cập nhật database
            NewDrinks.Add(NewDrinkAdded);

            return true;
        }

        public bool AddCategory(string CategoryName)
        {
            if (string.IsNullOrWhiteSpace(CategoryName))
            {
                Error = "Category cannot be empty.";
                return false;
            }

            Category category = new()
            {
                CategoryID = Categories.Count + 1,
                CategoryName = CategoryName,
            };

            // Thêm Categories vừa Add vào mảng NewCategories đẻ sau cập nhật database
            NewCategories.Add(category);
            ClearError();
            return true;
        }

        public void UpdateDrinksIntoDB()
        {
            // call API to update drinks and categories
        }

        private bool ValidateDrink(Drink drink)
        {
            if (string.IsNullOrWhiteSpace(drink.Name))
            {
                Error = "Name cannot be empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(drink.Description))
            {
                Error = "Description cannot be empty.";
                return false;
            }

            foreach (var size in drink.Sizes)
            {
                if (size.Price < 0)
                {
                    Error = $"Price for size '{size.Name}' must be greater than 0.";
                    return false;
                }

                if (size.Stock < 0)
                {
                    Error = $"Stock for size '{size.Name}' cannot be negative.";
                    return false;
                }

                if (!int.TryParse(size.Price.ToString(), out _) || !int.TryParse(size.Stock.ToString(), out _))
                {
                    Error = $"Price and Stock for size '{size.Name}' must be valid numbers.";
                    return false;
                }
            }

            ClearError();
            return true;
        }

        public void ClearError()
        {
            Error = "";
        }
    }
}
