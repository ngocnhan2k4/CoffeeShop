using System;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeShop.Models
{
    public class DiscountManager
    {
        private List<Discount> _discounts;

        public DiscountManager(List<Discount> discounts)
        {
            _discounts = discounts;
        }

        public Discount GetDiscountForCategory(int categoryId)
        {
            return _discounts
                .Where(d => d.CategoryID == categoryId && d.IsValid())
                .OrderByDescending(d => d.DiscountPercent)
                .FirstOrDefault();
        }

        public decimal GetDiscountedPrice(int categoryId, decimal originalPrice)
        {
            var discount = GetDiscountForCategory(categoryId);
            if (discount != null)
            {
                return Math.Round(originalPrice * (1 - (decimal)discount.DiscountPercent / 100), 2);
            }
            return originalPrice;
        }
    }
}