using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    public class Size
    {
        public string Name { get; set; } // S M L 
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
    public class Drink : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public List<Size> Sizes { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Discount { get; set; }
        public int CategoryID { get; set; }
        public string Ingredients { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
