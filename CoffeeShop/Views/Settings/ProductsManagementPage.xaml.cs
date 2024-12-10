using CoffeeShop.Models;
using CoffeeShop.ViewModels.Settings;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.Storage;
using System;
using Microsoft.UI.Xaml.Media.Imaging;
using WinRT.Interop;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls.Primitives;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductsManagementPage : Page
    {

        public ProductsManagementViewModel ViewModel { get; set; }   

        public ProductsManagementPage()
        {
            this.InitializeComponent();

            ViewModel = new ProductsManagementViewModel();
        }

        // Handle event on main UI 
        private async void EditDishButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var drink = (sender as FrameworkElement).DataContext as Drink;
            ViewModel.SelectedEditDrink = new(drink);
            await EditDrinkDialog.ShowAsync();
        }

        private async void ShowDialogAddDrink_ButttonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NewDrinkAdded = new();
            await NewDrinkDialog.ShowAsync();
        }

        private async void ShowDialogManageCategory_ButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NewCategories = new(ViewModel.Categories.Select(c => new Category(c)));
            await ManageCategoriesDialog.ShowAsync();
        }

        private async void SaveChanges_ButtonClick(object sender, RoutedEventArgs e)
        {
            bool result = ViewModel.UpdateDrinksAndCategoriesIntoDB();
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Content = new TextBlock()
                {
                    Text = result ? "Save changes successfully" : "Save changes failed", 
                    FontSize = 20
                },
                CloseButtonText = "Close"
            }.ShowAsync();
        }

        private void DiscardChanges_ButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadData();
        }

        // Handle event on Dialog EditDrink
        private void EditDrinkDialog_SaveButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = !ViewModel.EditDrink();
        }

        private void EditDrinkDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.ClearError();
        }

        // Handle event on Dialog NewDrink
        private void NewDrinkDialog_SaveButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Khi AddDrink không thành công thì không cho tắt dialog
           args.Cancel = !ViewModel.AddDrink(); 
        }

        private void NewDrinkDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.ClearError();
        }

        // Handle event on Dialog ManageCategory
        private void ManageCategoriesDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ManageCategoriesDialog_SaveButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            List<Category> categories = new();
            foreach (Category category in ViewModel.NewCategories)
            {
                if(ViewModel.Categories.FirstOrDefault(c => c.CategoryID == category.CategoryID) == null)
                {
                    categories.Add(category);
                    ViewModel.Categories.Add(category);
                }
            }
            ViewModel.NewCategories.Clear();
            ViewModel.NewCategories = new(categories); // mảng NewCategories sẽ lưu những category mới được thêm vào
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var category = NewCategoryNameTextBox.Text;
            if(ViewModel.AddCategory(category))
                NewCategoryNameTextBox.Text = "";
        }

        private async void ChooseImageEditDrink_buttonClick(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            var window = (Application.Current as App)?.MainWindow;
            if (window != null)
            {
                var windowHandle = WindowNative.GetWindowHandle(window);

                InitializeWithWindow.Initialize(openPicker, windowHandle);

                // Đặt các bộ lọc để chỉ hiển thị các loại tệp hình ảnh
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");

                // Chọn file từ giao diện
                StorageFile file = await openPicker.PickSingleFileAsync();

                if (file != null)
                {
                    using (var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        var bitmapImage = new BitmapImage();
                        await bitmapImage.SetSourceAsync(stream);
                        this.ImageEditDrink.Source = bitmapImage;
                        string fileUri = file.Path;
                        this.ViewModel.SelectedEditDrink.ImageString = fileUri;
                    }
                }
            }
        }

        private async void ChooseImageAddDrink_buttonClick(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            var window = (Application.Current as App)?.MainWindow;
            if (window != null)
            {
                var windowHandle = WindowNative.GetWindowHandle(window);

                InitializeWithWindow.Initialize(openPicker, windowHandle);

                // Đặt các bộ lọc để chỉ hiển thị các loại tệp hình ảnh
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");

                // Chọn file từ giao diện
                StorageFile file = await openPicker.PickSingleFileAsync();

                if (file != null)
                {
                    using (var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        var bitmapImage = new BitmapImage();
                        await bitmapImage.SetSourceAsync(stream);
                        this.ImageNewDrink.Source = bitmapImage;
                        string fileUri = file.Path;
                        this.ViewModel.NewDrinkAdded.ImageString = fileUri;
                    }
                }
            }
        }

        private async void ShowDiscountDialog_ButtonClick(object sender, RoutedEventArgs e)
        {
            await ManageDiscountsDialog.ShowAsync();
        }

        private void ManageDiscountsDialog_SaveButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = !ViewModel.ApplyDiscounts();
        }

        private void ManageDiscountsDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void DeleteDiscount_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button?.Tag is Discount discountToDelete)
            {
                // Gọi hàm xóa trong ViewModel
                ViewModel.DeleteDiscount(discountToDelete);
            }
        }

        private void AddDiscount_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddDiscount();
        }
    }

}
