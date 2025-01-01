using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service;
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
        public FullObservableCollection<Discount> Discounts {get;set;}
        public List<string> NameSizes { get; set; }

        // Dùng để theo dõi tabs category đang được chọn
        private int _selectedCategoryIndex;
        public int SelectedCategoryIndex
        {
            get => _selectedCategoryIndex;
            set
            {
                _selectedCategoryIndex = value;
                DrinksByCategoryID = new(FilterDrinksByCategoryID(SelectedCategoryIndex));
            }
        }

        // Dùng để theo dõi Drink đang được edit 
        public Drink SelectedEditDrink {  get; set; }

        public Drink NewDrinkAdded { get; set; }
        public int NewDrinkCategoryID {  get; set; }

        public Discount NewDiscount {get; set;}
        public bool HasDiscounts => Discounts.Count > 0;

        public string Error {  get; set; }
        public IDao _dao { get; set; }

        public ProductsManagementViewModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            _selectedCategoryIndex = 0;
            NameSizes = ["S", "M", "L"];
            //_dao = new SqlServerDao();
             _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            Drinks = _dao.GetDrinks();
            Categories = new (_dao.GetCategories());
            NewDrinks = [];
            NewCategories = [];
            DrinksByCategoryID = new(FilterDrinksByCategoryID(SelectedCategoryIndex));
            Discounts = new(_dao.GetDiscounts());
            NewDiscount = new();
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
            DiscountManager discountManager = new(Discounts.ToList());
            // Thêm ơ cả DrinksByCategoryID và Drinks 
            NewDrinkAdded.CategoryID = SelectedCategoryIndex;
            NewDrinkAdded.Discount = discountManager.GetDiscountForCategory(SelectedCategoryIndex);
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
                CategoryID = Categories.Count,
                CategoryName = CategoryName,
            };

            ClearError();
            // Thêm Categories vừa Add vào mảng NewCategories để sau cập nhật database
            NewCategories.Add(category);
           
            return true;
        }

        public bool UpdateDrinksAndCategoriesIntoDB()
        {
            bool isAddedDrinks = _dao.AddDrinks(NewDrinks);
            bool isAddedCategories = _dao.AddCategories(NewCategories.ToList());
            bool isAddedDiscounts = _dao.AddDiscounts(Discounts.ToList());

            if (!isAddedDrinks)
            {
                foreach (var drink in NewDrinks)
                {
                    DrinksByCategoryID.Remove(drink);
                    Drinks.Remove(drink);
                }
            }

            if (!isAddedCategories)
            {
                foreach (var category in NewCategories)
                {
                    Categories.Remove(category);
                }
            }

            if (!isAddedDiscounts)
            {
                Discounts.Clear();
            }

            NewDrinks.Clear();
            NewCategories.Clear();
            return isAddedDrinks && isAddedCategories  && isAddedDiscounts;
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

        public bool AddDiscount()
        {
            if (!ValidateDiscount(NewDiscount)) return false;
            NewDiscount.CategoryName = Categories.FirstOrDefault(c => c.CategoryID == NewDiscount.CategoryID).CategoryName;
            Discounts.Add(new Discount(NewDiscount));
            NewDiscount.Reset();
            OnPropertyChanged("HasDiscounts");
            return true;
        }

        public bool ValidateDiscount(Discount discount)
        {
            if (string.IsNullOrWhiteSpace(discount.Name))
            {
                Error = "Name cannot be empty.";
                return false;
            }

            if (discount.DiscountPercent < 0 || discount.DiscountPercent > 100)
            {
                Error = "Discount percentage must be between 0 and 100.";
                return false;
            }

            if (discount.ValidUntil < DateTime.Now)
            {
                Error = "Valid until must be greater than current date.";
                return false;
            }

            if (discount.CategoryID == -1) 
            {
                Error = "Category not selected";
                return false;
            }

            ClearError();
            return true;
        }

        public void DeleteDiscount(Discount discount)
        {
            Discounts.Remove(discount);
            OnPropertyChanged("HasDiscounts");
        }

        public bool ApplyDiscounts()
        {
            foreach (var category in Categories)
            {
                int count = 0;
                foreach (var discount in Discounts)
                {
                    if (discount.CategoryID == category.CategoryID && discount.IsActive)
                    {
                        count++;
                    }
                }
                if(count >= 2)
                {
                    Error = "Cannot apply more than 2 discounts for a category";
                    return false;
                }
            }

            ClearError();
            DiscountManager discountManager = new(Discounts.ToList());
            foreach (var drink in Drinks)
            {
                drink.Discount = discountManager.GetDiscountForCategory(drink.CategoryID);
            }
            DrinksByCategoryID = new(FilterDrinksByCategoryID(SelectedCategoryIndex));
            return true;
        }

        public void ClearError()
        {
            Error = "";
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
