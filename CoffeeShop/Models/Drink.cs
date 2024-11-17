using CoffeeShop.Helper;
using Microsoft.UI.Xaml.Media.Imaging;
using PropertyChanged;
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System;

using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{

    public class Size : INotifyPropertyChanged
    {
        public string Name { get; set; } // S M L 
        public int Price { get; set; }
        public int Stock { get; set; }

        public Size() {}

        public Size(Size other)
        {
            Name = other.Name;
            Price = other.Price;
            Stock = other.Stock;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class Drink : INotifyPropertyChanged
    {
        public int ID {  get; set; }
        public string Name { get; set; }
        public List<Size> Sizes { get; set; }
        public string Description { get; set; }
        public string ImageString { get; set; }
        [DoNotNotify]
        public BitmapImage Image
        {
            get
            {
                if (!string.IsNullOrEmpty(ImageString))
                {
                    try
                    {
                        var bitmapImage = new BitmapImage(new Uri(ImageString));
                        return bitmapImage;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                return null; 
            }
            set
            {
                if (value != null)
                {
                    ImageString = value.UriSource?.ToString();
                }
                else
                {
                    ImageString = null;
                }
            }
        }
        public decimal Discount { get; set; } 
        public int CategoryID { get; set; }

        public Drink()
        {
            ID = 0;
            Name = "";
            Sizes = [
                new () { Name = "S", Price = 0, Stock = 0 },
                    new () { Name = "M", Price = 0, Stock = 0 },
                    new () { Name = "L", Price = 0, Stock = 0 }
            ];
            Description = "";
            ImageString = "ms-appx:///Assets/default.jpg";
            Discount = 0;
            CategoryID = 0;
        }

        public Drink(Drink other)
        {
            ID = other.ID;
            Name = other.Name;
            Description = other.Description;
            Sizes = new (other.Sizes.Select(size => new Size(size))); 
            ImageString = other.ImageString;
            Discount = other.Discount;
            CategoryID = other.CategoryID;
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
