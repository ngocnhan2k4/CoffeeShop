using CoffeeShop.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CoffeeShop.Service.DataAccess.IDao;

namespace CoffeeShop.Service.DataAccess
{
    /// <summary>
    /// This class represents a mock data access object
    /// </summary>
    public class MockDao : IDao
    {
       
        public List<Category> GetCategories()
        {
            var list = new List<Category>()
            {
                new Category() { CategoryID = 0, CategoryName = "Trà sữa" },
                new Category() { CategoryID = 1, CategoryName = "Cà phê" },
                new Category() { CategoryID = 2, CategoryName = "Trà trái cây tươi" },
                new Category() { CategoryID = 3, CategoryName = "Sinh tố" },
                new Category() { CategoryID = 4, CategoryName = "Nước ép" }
            };
            return list;


        }

        public List<DeliveryInvoice> GetDeliveryInvoices()
        {
            var list = new List<DeliveryInvoice>()
            {
                new DeliveryInvoice() { DeliveryInvoiceID = 1, PhoneNumber = "0123456789", ShippingFee = 5000, Address="KTX khu B" },

            };
            return list;


        }

        public List<DetailInvoice> GetDetailInvoices()
        {
            var list = new List<DetailInvoice>()
            {
                new DetailInvoice() { InvoiceID = 1, NameDrink = "Cà phê sữa", Quantity = 1, Size = "M" },
                new DetailInvoice() { InvoiceID = 2, NameDrink = "Cà phê sữa", Quantity = 1, Size = "S" },
                new DetailInvoice() { InvoiceID = 3, NameDrink = "Trà sữa matcha", Quantity = 1, Size = "S" },
                new DetailInvoice() { InvoiceID = 4, NameDrink = "Cà phê sữa", Quantity = 1, Size = "S" },
                new DetailInvoice() { InvoiceID = 5, NameDrink = "Sinh tố xoài", Quantity = 1, Size = "S" },
            };
            return list;


        }

        public List<Drink> GetDrinks()
        {
            List<Drink> list = new List<Drink>()
                    {
                        new ()
                        {
                            Name = "Cà Phê Sữa Đá",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 20000, Stock = 20 },
                                new () { Name = "M", Price = 25000, Stock = 15 },
                                new () { Name = "L", Price = 30000, Stock = 10 }
                            },
                            Description = "Cà phê sữa đá thơm ngon.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 1
                        },
                        new ()
                        {
                            Name = "Trà Sữa",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 30000, Stock = 25 },
                                new () { Name = "M", Price = 35000, Stock = 20 },
                                new () { Name = "L", Price = 40000, Stock = 15 }
                            },
                            Description = "Trà sữa thơm ngon, mát lạnh.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 2
                        },
                        new ()
                        {
                            Name = "Sinh Tố Bơ",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 25000, Stock = 10 },
                                new () { Name = "M", Price = 30000, Stock = 8 },
                                new () { Name = "L", Price = 35000, Stock = 5 }
                            },
                            Description = "Sinh tố bơ tươi ngon.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 3
                        },
                        new ()
                        {
                            Name = "Nước Ép Cam",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 20000, Stock = 12 },
                                new () { Name = "M", Price = 25000, Stock = 9 }
                            },
                            Description = "Nước ép cam tươi mát.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 4
                        },
                        new ()
                        {
                            Name = "Soda Chanh",
                            Sizes = new List<Size>
                            {
                                new () { Name = "M", Price = 15000, Stock = 18 },
                                new () { Name = "L", Price = 20000, Stock = 12 }
                            },
                            Description = "Soda chanh thơm ngon, sảng khoái.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 3
                        },
                        new ()
                        {
                            Name = "Cà Phê Đen",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 20000, Stock = 30 }
                            },
                            Description = "Cà phê đen đậm đà.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 1
                        },
                        new ()
                        {
                            Name = "Matcha Latte",
                            Sizes = new List<Size>
                            {
                                new () { Name = "M", Price = 35000, Stock = 10 },
                                new () { Name = "L", Price = 40000, Stock = 8 }
                            },
                            Description = "Trà matcha hòa quyện với sữa.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 2
                        },
                        new ()
                        {
                            Name = "Cacao",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 30000, Stock = 12 },
                                new () { Name = "M", Price = 35000, Stock = 9 }
                            },
                            Description = "Cacao nóng thơm ngon.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 3
                        },
                        new ()
                        {
                            Name = "Nước Dừa",
                            Sizes = new List<Size>
                            {
                                new () { Name = "M", Price = 20000, Stock = 15 },
                                new () { Name = "L", Price = 25000, Stock = 10 }
                            },
                            Description = "Nước dừa tươi mát.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 4
                        },
                        new ()
                        {
                            Name = "Trà Đào",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 25000, Stock = 20 },
                                new () { Name = "M", Price = 30000, Stock = 15 }
                            },
                            Description = "Trà đào thơm ngon.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 2
                        },
                        new ()
                        {
                            Name = "Sữa Chua",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 15000, Stock = 18 },
                                new () { Name = "M", Price = 20000, Stock = 12 }
                            },
                            Description = "Sữa chua tươi mát.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 1
                        },
                        new ()
                        {
                            Name = "Nước Mía",
                            Sizes = new List<Size>
                            {
                                new () { Name = "M", Price = 25000, Stock = 22 },
                                new () { Name = "L", Price = 30000, Stock = 18 }
                            },
                            Description = "Nước mía thơm ngon.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 4
                        },
                        new ()
                        {
                            Name = "Nước Chanh Muối",
                            Sizes = new List<Size>
                            {
                                new () { Name = "M", Price = 20000, Stock = 15 },
                                new () { Name = "L", Price = 25000, Stock = 10 }
                            },
                            Description = "Nước chanh muối giải khát.",
                            ImageString = "ms-appx:///Assets/default.jpg", // Changed here
                            Discount = 0,
                            CategoryID = 3
                        },
                         new ()
            {
                Name = "Trà Sữa Trân Châu",
                Sizes = new List<Size>
                {
                    new () { Name = "S", Price = 35000, Stock = 30 },
                    new () { Name = "M", Price = 40000, Stock = 25 },
                    new () { Name = "L", Price = 45000, Stock = 20 }
                },
                Description = "Trà sữa với trân châu mềm mịn.",
                ImageString = "ms-appx:///Assets/images.jpeg",
                Discount = 0,
                CategoryID = 0
            },
            new ()
            {
                Name = "Trà Sữa Matcha Trân Châu",
                Sizes = new List<Size>
                {
                    new () { Name = "S", Price = 38000, Stock = 15 },
                    new () { Name = "M", Price = 43000, Stock = 10 },
                    new () { Name = "L", Price = 48000, Stock = 5 }
                },
                Description = "Trà sữa matcha kết hợp với trân châu.",
                ImageString = "ms-appx:///Assets/images.jpeg",
                Discount = 0,
                CategoryID = 0
            },
            new ()
            {
                Name = "Trà Sữa Hoa Quả",
                Sizes = new List<Size>
                {
                    new () { Name = "S", Price = 36000, Stock = 20 },
                    new () { Name = "M", Price = 41000, Stock = 15 },
                    new () { Name = "L", Price = 46000, Stock = 10 }
                },
                Description = "Trà sữa với hương vị hoa quả tươi ngon.",
                ImageString = "ms-appx:///Assets/images.jpeg",
                Discount = 0,
                CategoryID = 0
            },
                    };

            return list;


        }
    
    public Tuple<List<Drink>, int> GetDrinks(
             int page, int rowsPerPage,
             string keyword, int categoryID,
             Dictionary<string, SortType> sortOptions
         )
        {
            List<Drink> drinks = new List<Drink>()
                    {
                        new ()
                        {
                            Name = "Cà Phê Sữa Đá",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 20000, Stock = 20 },
                                new () { Name = "M", Price = 25000, Stock = 15 },
                                new () { Name = "L", Price = 30000, Stock = 10 }
                            },
                            Description = "Cà phê sữa đá thơm ngon.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 1
                        },
                        new ()
                        {
                            Name = "Trà Sữa",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 30000, Stock = 25 },
                                new () { Name = "M", Price = 35000, Stock = 20 },
                                new () { Name = "L", Price = 40000, Stock = 15 }
                            },
                            Description = "Trà sữa thơm ngon, mát lạnh.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 2
                        },
                        new ()
                        {
                            Name = "Sinh Tố Bơ",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 25000, Stock = 10 },
                                new () { Name = "M", Price = 30000, Stock = 8 },
                                new () { Name = "L", Price = 35000, Stock = 5 }
                            },
                            Description = "Sinh tố bơ tươi ngon.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 3
                        },
                        new ()
                        {
                            Name = "Nước Ép Cam",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 20000, Stock = 12 },
                                new () { Name = "M", Price = 25000, Stock = 9 }
                            },
                            Description = "Nước ép cam tươi mát.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 4
                        },
                        new ()
                        {
                            Name = "Soda Chanh",
                            Sizes = new List<Size>
                            {
                                new () { Name = "M", Price = 15000, Stock = 18 },
                                new () { Name = "L", Price = 20000, Stock = 12 }
                            },
                            Description = "Soda chanh thơm ngon, sảng khoái.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 3
                        },
                        new ()
                        {
                            Name = "Cà Phê Đen",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 20000, Stock = 30 }
                            },
                            Description = "Cà phê đen đậm đà.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 1
                        },
                        new ()
                        {
                            Name = "Matcha Latte",
                            Sizes = new List<Size>
                            {
                                new () { Name = "M", Price = 35000, Stock = 10 },
                                new () { Name = "L", Price = 40000, Stock = 8 }
                            },
                            Description = "Trà matcha hòa quyện với sữa.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 2
                        },
                        new ()
                        {
                            Name = "Cacao",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 30000, Stock = 12 },
                                new () { Name = "M", Price = 35000, Stock = 9 }
                            },
                            Description = "Cacao nóng thơm ngon.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 3
                        },
                        new ()
                        {
                            Name = "Nước Dừa",
                            Sizes = new List<Size>
                            {
                                new () { Name = "M", Price = 20000, Stock = 15 },
                                new () { Name = "L", Price = 25000, Stock = 10 }
                            },
                            Description = "Nước dừa tươi mát.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 4
                        },
                        new ()
                        {
                            Name = "Trà Đào",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 25000, Stock = 20 },
                                new () { Name = "M", Price = 30000, Stock = 15 }
                            },
                            Description = "Trà đào thơm ngon.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 2
                        },
                        new ()
                        {
                            Name = "Sữa Chua",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 15000, Stock = 18 },
                                new () { Name = "M", Price = 20000, Stock = 12 }
                            },
                            Description = "Sữa chua tươi mát.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 1
                        },
                        new ()
                        {
                            Name = "Nước Mía",
                            Sizes = new List<Size>
                            {
                                new () { Name = "M", Price = 25000, Stock = 22 },
                                new () { Name = "L", Price = 30000, Stock = 18 }
                            },
                            Description = "Nước mía thơm ngon.",
                            ImageString = "ms-appx:///Assets/images.jpeg", // Changed here
                            Discount = 0,
                            CategoryID = 4
                        },
                        new ()
                        {
                            Name = "Nước Chanh Muối",
                            Sizes = new List<Size>
                            {
                                new () { Name = "M", Price = 20000, Stock = 15 },
                                new () { Name = "L", Price = 25000, Stock = 10 }
                            },
                            Description = "Nước chanh muối giải khát.",
                            ImageString = "ms-appx:///Assets/default.jpg", // Changed here
                            Discount = 0,
                            CategoryID = 3
                        },
                         new ()
            {
                Name = "Trà Sữa Trân Châu",
                Sizes = new List<Size>
                {
                    new () { Name = "S", Price = 35000, Stock = 30 },
                    new () { Name = "M", Price = 40000, Stock = 25 },
                    new () { Name = "L", Price = 45000, Stock = 20 }
                },
                Description = "Trà sữa với trân châu mềm mịn.",
                ImageString = "ms-appx:///Assets/images.jpeg",
                Discount = 0,
                CategoryID = 0
            },
            new ()
            {
                Name = "Trà Sữa Matcha Trân Châu",
                Sizes = new List<Size>
                {
                    new () { Name = "S", Price = 38000, Stock = 15 },
                    new () { Name = "M", Price = 43000, Stock = 10 },
                    new () { Name = "L", Price = 48000, Stock = 5 }
                },
                Description = "Trà sữa matcha kết hợp với trân châu.",
                ImageString = "ms-appx:///Assets/images.jpeg",
                Discount = 0,
                CategoryID = 0
            },
            new ()
            {
                Name = "Trà Sữa Hoa Quả",
                Sizes = new List<Size>
                {
                    new () { Name = "S", Price = 36000, Stock = 20 },
                    new () { Name = "M", Price = 41000, Stock = 15 },
                    new () { Name = "L", Price = 46000, Stock = 10 }
                },
                Description = "Trà sữa với hương vị hoa quả tươi ngon.",
                ImageString = "ms-appx:///Assets/images.jpeg",
                Discount = 0,
                CategoryID = 0
            },
                    };
            // Search
            var query = from e in drinks
                        where e.Name.ToLower().Contains(keyword.ToLower())
                        select e;

            // Filter
            if (categoryID != -1)
            {
                query = query.Where(e => e.CategoryID == categoryID);
            }
            // Sort
            foreach (var option in sortOptions)
            {
                if (option.Key == "Price")
                {
                    if (option.Value == SortType.Ascending)
                    {
                        query = query.OrderBy(e => e.Sizes[0].Price*(1-e.Discount));
                    }
                    else
                    {
                        query = query.OrderByDescending(e => e.Sizes[0].Price * (1 - e.Discount));
                    }
                }
                if(option.Key == "Stock")
                {
                    if(option.Value == SortType.Descending)
                    {
                        query = query.OrderByDescending(e => {
                            int sum = 0;
                            foreach (var size in e.Sizes)
                            {
                                sum += size.Stock;
                            }
                            return sum;
                        }
                        );
                    }
                }
            }

            var result = query
                .Skip((page - 1) * rowsPerPage)
                .Take(rowsPerPage);

            return new Tuple<List<Drink>, int>(
                result.ToList(),
                query.Count()
            );
        }
        public List<Invoice> GetInvoices()
        {
            var list = new List<Invoice>()
            {
                new(){InvoiceID=1, CreatedAt="2024-10-16",TotalAmount=18000, PaymentMethod="Credit Card", CustomerName="Nguyễn Văn A", Status="Paid"},
                new(){InvoiceID=3, CreatedAt="2024-10-15",TotalAmount=27000, PaymentMethod="Bank Transfer", CustomerName="Phạm Văn C", Status="Pending"},
                new(){InvoiceID=2, CreatedAt="2024-10-16",TotalAmount=15000, PaymentMethod="Cash", CustomerName="Trần Thị B", Status="UnPaid"},
                new(){InvoiceID=4, CreatedAt="2023-10-16",TotalAmount=15000, PaymentMethod="Cash", CustomerName="Trần Thị B", Status="UnPaid"},
                new(){InvoiceID=5, CreatedAt="2023-9-16",TotalAmount=27000, PaymentMethod="Cash", CustomerName="Trần Thị C", Status="UnPaid"}
            };
            return list;


        }
    }
    
}