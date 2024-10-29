using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Service.DataAccess
{
    /// <summary>
    /// This class represents a mock data access object
    /// </summary>
    public class MockDao : IDao
    {
        public List<Category> GetCategories()
        {
            List<Category> list = [
                new()
                {
                    CategoryID = 1,
                    CategoryName = "Hot Dishes",
                },
                new()
                {
                    CategoryID = 2,
                    CategoryName = "Cold Dishes",
                },
                new()
                {
                    CategoryID = 3,
                    CategoryName = "Soup",
                },
                new()
                {
                    CategoryID = 4,
                    CategoryName = "Grill",
                }
            ];
            return list;
        }


        public List<Drink> GetAllDrinks()
        {
            List<Drink> list = new List<Drink>
    {
        new Drink
        {
            ID = 1,
            Name = "Cà Phê Sữa Đá",
            Sizes = [
                new () { Name = "S", Price = 20000, Stock = 20 },
                new () { Name = "M", Price = 25000, Stock = 15 },
                new () { Name = "L", Price = 30000, Stock = 10 }
            ],
            Description = "Cà phê sữa đá thơm ngon.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 1
        },
        new Drink
        {
            ID = 2,
            Name = "Trà Sữa",
            Sizes = [
                new () { Name = "S", Price = 30000, Stock = 25 },
                new () { Name = "M", Price = 35000, Stock = 20 },
                new () { Name = "L", Price = 40000, Stock = 15 }
            ],
            Description = "Trà sữa thơm ngon, mát lạnh.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 2
        },
        new Drink
        {
            ID = 3,
            Name = "Sinh Tố Bơ",
            Sizes = [
                new () { Name = "S", Price = 25000, Stock = 10 },
                new () { Name = "M", Price = 30000, Stock = 8 },
                new () { Name = "L", Price = 35000, Stock = 5 }
            ],
            Description = "Sinh tố bơ tươi ngon.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 3
        },
        new Drink
        {
            ID = 4,
            Name = "Nước Ép Cam",
            Sizes = [
                new () { Name = "S", Price = 20000, Stock = 12 },
                new () { Name = "M", Price = 25000, Stock = 9 }
            ],
            Description = "Nước ép cam tươi mát.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 4
        },
        new Drink
        {
            ID = 5,
            Name = "Soda Chanh",
            Sizes = [
                new () { Name = "M", Price = 15000, Stock = 18 },
                new () { Name = "L", Price = 20000, Stock = 12 }
            ],
            Description = "Soda chanh thơm ngon, sảng khoái.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 3
        },
        new Drink
        {
            ID = 6,
            Name = "Cà Phê Đen",
            Sizes = [
                new () { Name = "S", Price = 20000, Stock = 30 }
            ],
            Description = "Cà phê đen đậm đà.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 1
        },
        new Drink
        {
            ID = 7,
            Name = "Matcha Latte",
            Sizes = [
                new () { Name = "M", Price = 35000, Stock = 10 },
                new () { Name = "L", Price = 40000, Stock = 8 }
            ],
            Description = "Trà matcha hòa quyện với sữa.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 2
        },
        new Drink
        {
            ID = 8,
            Name = "Cacao",
            Sizes = [
                new () { Name = "S", Price = 30000, Stock = 12 },
                new () { Name = "M", Price = 35000, Stock = 9 }
            ],
            Description = "Cacao nóng thơm ngon.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 3
        },
        new Drink
        {
            ID = 9,
            Name = "Nước Dừa",
            Sizes = [
                new () { Name = "M", Price = 20000, Stock = 15 },
                new () { Name = "L", Price = 25000, Stock = 10 }
            ],
            Description = "Nước dừa tươi mát.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 4
        },
        new Drink
        {
            ID = 10,
            Name = "Trà Đào",
            Sizes = [
                new () { Name = "S", Price = 25000, Stock = 20 },
                new () { Name = "M", Price = 30000, Stock = 15 }
            ],
            Description = "Trà đào thơm ngon.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 2
        },
        new Drink
        {
            ID = 11,
            Name = "Sữa Chua",
            Sizes = [
                new () { Name = "S", Price = 15000, Stock = 18 },
                new () { Name = "M", Price = 20000, Stock = 12 }
            ],
            Description = "Sữa chua tươi mát.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 1
        },
        new Drink
        {
            ID = 12,
            Name = "Nước Mía",
            Sizes = [
                new () { Name = "M", Price = 25000, Stock = 22 },
                new () { Name = "L", Price = 30000, Stock = 18 }
            ],
            Description = "Nước mía thơm ngon.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 4
        },
        new Drink
        {
            ID = 13,
            Name = "Nước Chanh Muối",
            Sizes = [
                new () { Name = "M", Price = 20000, Stock = 15 },
                new () { Name = "L", Price = 25000, Stock = 10 }
            ],
            Description = "Nước chanh muối giải khát.",
            ImageString = "ms-appx:///Assets/download.jpg",
            Discount = 0,
            CategoryID = 3
        }
    };

            return list;
        }


    }
}