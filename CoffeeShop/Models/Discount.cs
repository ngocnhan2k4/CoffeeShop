using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace CoffeeShop.Models
{
    public class Discount : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public double DiscountPercent { get; set; }
        public DateTime ValidUntil { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } 
        public bool IsActive { get; set; }


        public Discount()
        {
            IsActive = false;
            ValidUntil = DateTime.Now;
            DiscountPercent = 0;
            Name = "";
            CategoryID = -1;
        }

        public void Reset()
        {
            IsActive = false;
            ValidUntil = DateTime.Now;
            DiscountPercent = 0;
            Name = "";
            CategoryID = 0;
        }

        public bool IsValid()
        {
            return IsActive && DateTime.Now <= ValidUntil && DiscountPercent != 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
