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
        public List<Discount> GetDiscounts()
        {
            var categories = GetCategories();
            var list = new List<Discount>()
            {
                new ()
                {
                    Name = "Khuyến mãi Trà sữa",
                    DiscountPercent = 10,
                    ValidUntil = DateTime.Now.AddDays(30),
                    CategoryID = 0,
                    CategoryName = categories.Find(c => c.CategoryID == 0).CategoryName,
                    IsActive = true
                },
                new ()
                {
                    Name = "Khuyến mãi Cà phê",
                    DiscountPercent = 15,
                    ValidUntil = DateTime.Now.AddDays(30),
                    CategoryID = 1,
                    CategoryName = categories.Find(c => c.CategoryID == 1).CategoryName,
                    IsActive = true
                },
                new ()
                {
                    Name = "Khuyến mãi Nước ép",
                    DiscountPercent = 5,
                    ValidUntil = DateTime.Now.AddDays(30),
                    CategoryID = 4,
                    CategoryName = categories.Find(c => c.CategoryID == 4).CategoryName,
                    IsActive = true
                }
            };
            return list;
        }

        public bool AddDiscounts(List<Discount> Discounts)
        {
            return true;
        }

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

        public bool AddCategories(List<Category> categories)
        {
            return true;
        }

        public List<DeliveryInvoice> GetDeliveryInvoices()
        {
            var list = new List<DeliveryInvoice>()
            {
                new DeliveryInvoice() { DeliveryInvoiceID = 1, PhoneNumber = "0123456789", ShippingFee = 15000, Address="KTX khu B ĐHQG" },
                new DeliveryInvoice() { DeliveryInvoiceID = 2, PhoneNumber = "0987654321", ShippingFee = 20000, Address="Chung cư Sunview" },
                new DeliveryInvoice() { DeliveryInvoiceID = 3, PhoneNumber = "0369852147", ShippingFee = 25000, Address="Đại học Công nghệ Thông tin" }
            };
            return list;
        }

       public List<Drink> GetDrinks()
        {
            DiscountManager _discountManager = new DiscountManager(GetDiscounts());
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
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 1, 
                            Discount = _discountManager.GetDiscountForCategory(1)
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
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 2,
                            Discount = _discountManager.GetDiscountForCategory(2)
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
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 3,
                            Discount = _discountManager.GetDiscountForCategory(3)
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
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 4,
                            Discount = _discountManager.GetDiscountForCategory(4)
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
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 3,
                                Discount = _discountManager.GetDiscountForCategory(3)
                        },
                        new ()
                        {
                            Name = "Cà Phê Đen",
                            Sizes = new List<Size>
                            {
                                new () { Name = "S", Price = 20000, Stock = 30 }
                            },
                            Description = "Cà phê đen đậm đà.",
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 1,
                            Discount = _discountManager.GetDiscountForCategory(1)
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
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 2,
                            Discount = _discountManager.GetDiscountForCategory(2)
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
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 3,
                            Discount = _discountManager.GetDiscountForCategory(3)
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
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 4,
                            Discount = _discountManager.GetDiscountForCategory(4)
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
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 2,
                            Discount = _discountManager.GetDiscountForCategory(2)
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
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 1,
                            Discount = _discountManager.GetDiscountForCategory(1)
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
                            ImageString = "ms-appx:///Assets/images.jpeg",
                            CategoryID = 4,
                            Discount = _discountManager.GetDiscountForCategory(4)
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
                            ImageString = "ms-appx:///Assets/default.jpg",
                            CategoryID = 3,
                            Discount = _discountManager.GetDiscountForCategory(3)
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
                CategoryID = 0,
                Discount = _discountManager.GetDiscountForCategory(0)
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
                CategoryID = 0,
                Discount = _discountManager.GetDiscountForCategory(0)
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
                CategoryID = 0,
                Discount = _discountManager.GetDiscountForCategory(0)
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
            var drinks = GetDrinks();

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
                        query = query.OrderBy(e => e.GetDiscountedPrice(e.Sizes[0]));
                    }
                    else
                    {
                        query = query.OrderByDescending(e => e.GetDiscountedPrice(e.Sizes[0]));
                    }
                }
                if (option.Key == "Stock")
                {
                    if (option.Value == SortType.Descending)
                    {
                        query = query.OrderByDescending(e => {
                            int sum = 0;
                            foreach (var size in e.Sizes)
                            {
                                sum += size.Stock;
                            }
                            return sum;
                        });
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

        public bool AddDrinks(List<Drink> drinks)
        {
            return true;
        }

        public List<Invoice> GetInvoices()
        {
            var list = new List<Invoice>()
            {
                new(){InvoiceID=1, CreatedAt="2024-10-16",TotalAmount=18000, PaymentMethod="Credit Card", CustomerName="Nguyễn Văn A", Status="Paid"},
                new(){InvoiceID=3, CreatedAt="2024-10-15",TotalAmount=27000, PaymentMethod="Bank Transfer", CustomerName="Phạm Văn C", Status="Wait"},
                new(){InvoiceID=2, CreatedAt="2024-10-16",TotalAmount=15000, PaymentMethod="Cash", CustomerName="Trần Thị B", Status="Paid"},
                new(){InvoiceID=4, CreatedAt="2023-10-16",TotalAmount=15000, PaymentMethod="Cash", CustomerName="Trần Thị B", Status="Paid"},
                new(){InvoiceID=5, CreatedAt="2023-9-16",TotalAmount=27000, PaymentMethod="Cash", CustomerName="Trần Thị C", Status="Paid"}
            };
            return list;
        }

        public List<DetailInvoice> GetDetailInvoices()
        {
            var list = new List<DetailInvoice>()
            {
                new DetailInvoice() { InvoiceID = 1, NameDrink = "Cà Phê Sữa Đá", Quantity = 2, Size = "M" },
                new DetailInvoice() { InvoiceID = 1, NameDrink = "Trà Sữa Trân Châu", Quantity = 1, Size = "L" },
                new DetailInvoice() { InvoiceID = 2, NameDrink = "Sinh Tố Bơ", Quantity = 3, Size = "M" },
                new DetailInvoice() { InvoiceID = 3, NameDrink = "Trà Sữa", Quantity = 2, Size = "L" },
                new DetailInvoice() { InvoiceID = 4, NameDrink = "Nước Ép Cam", Quantity = 3, Size = "M" }
            };
            return list;
        }

        public int CalculateNumberOrders(int year)
        {
            int total = 0;
            var invoices = GetInvoices();
            foreach (var invoice in invoices)
            {
                if (Convert.ToDateTime(invoice.CreatedAt).Year == year && invoice.Status!="Cancel")
                {
                    total++;
                }
            }
            return total;
        }

        public int CalculateTotalCost()
        {
            var drinks = GetDrinks();
            int result = 0;
            foreach (var drink in drinks)
            {
                foreach (var size in drink.Sizes)
                {
                    result += size.Price * size.Stock;
                }
            }
            return result;
        }

        public int CalculateRevenue(int year)
        {
            var invoices = GetInvoices();
            int result = 0;
            foreach (var invoice in invoices)
            {
                if (Convert.ToDateTime(invoice.CreatedAt).Year == year && invoice.Status != "Cancel")
                    result += invoice.TotalAmount;
            }
            return result;
        }

        public int CalculateProfit(int year)
        {
            return CalculateRevenue(year) - CalculateTotalCost();
        }

        public List<int> CalculateYears()
        {
            List<int> years = new List<int>();
            var invoices = GetInvoices();
            foreach (var invoice in invoices)
            {
                int year = Convert.ToDateTime(invoice.CreatedAt).Year;
                if (!years.Contains(year))
                {
                    years.Add(year);
                }
            }
            return new() { years.Max(), years.Min() };
        }

        public List<int> CalculateMonthlyRevenue(int year)
        {
            List<int> result = new();
            var invoices = GetInvoices();
            for (int i = 1; i <= 12; i++)
            {
                int revenue = 0;
                foreach (var invoice in invoices)
                {
                    if (Convert.ToDateTime(invoice.CreatedAt).Month == i && (Convert.ToDateTime(invoice.CreatedAt).Year == year) && invoice.Status != "Cancel")
                    {
                        revenue += invoice.TotalAmount;
                    }
                }
                result.Add(revenue);
            }
            return result;
        }

        public List<string> CalculateTopDrinks(int year)
        {
            List<string> result = new List<string>();
            var detailInvoices = GetDetailInvoices();
            var invoices = GetInvoices();
            var drinks = GetDrinks();
            Dictionary<string, int> drinkSold = new Dictionary<string, int>();

            var invoicesInYear = invoices.Where(invoice => Convert.ToDateTime(invoice.CreatedAt).Year == year && invoice.Status != "Cancel").ToList();

            foreach (var invoice in invoicesInYear)
            {
                var detailInvoiceList = detailInvoices.Where(di => di.InvoiceID == invoice.InvoiceID);

                foreach (var detailInvoice in detailInvoiceList)
                {
                    var drinkName = drinks.Find(x => x.Name == detailInvoice.NameDrink)?.Name;
                    if (drinkName != null)
                    {
                        if (drinkSold.ContainsKey(drinkName))
                        {
                            drinkSold[drinkName] += detailInvoice.Quantity;
                        }
                        else
                        {
                            drinkSold.Add(drinkName, detailInvoice.Quantity);
                        }
                    }
                }
            }
            var sortedDrinkSold = drinkSold.OrderByDescending(x => x.Value).Take(5);
            foreach (var drink in sortedDrinkSold)
            {
                result.Add(drink.Key);
            }
            return result;
        }

        public Dictionary<string, int> CalculateRevenueCategory(int year)
        {
            var detailInvoices = GetDetailInvoices();
            var drinks = GetDrinks();
            var categories = GetCategories();
            var invoices = GetInvoices();

            Dictionary<string, int> revenueByCategory = new Dictionary<string, int>();
            foreach (var category in categories)
            {
                revenueByCategory.Add(category.CategoryName, 0);
            }
            var invoicesInYear = invoices.Where(invoice => Convert.ToDateTime(invoice.CreatedAt).Year == year && invoice.Status != "Cancel").ToList();

            for (int i = 0; i < invoicesInYear.Count; i++)
            {
                for (int j = 0; j < detailInvoices.Count; j++)
                {
                    if (invoicesInYear[i].InvoiceID == detailInvoices[j].InvoiceID)
                    {
                        var drink = drinks.Find(x => x.Name == detailInvoices[j].NameDrink);
                        var price = drink.Sizes.Find(x => x.Name == detailInvoices[j].Size).Price;
                        var category = categories.Find(x => x.CategoryID == drink.CategoryID);
                        revenueByCategory[category.CategoryName] += detailInvoices[j].Quantity * price;
                    }
                }
            }
            return revenueByCategory;
        }




        public Tuple<List<DetailInvoice>, DeliveryInvoice> GetDetailInvoicesOfId(int invoiceId)
        {
            var invoiceDetails = new List<DetailInvoice>
            {
                //create 5 tiems of Detail Invoice
                new DetailInvoice {
                    InvoiceID = 1,
                    NameDrink = "Cà Phê Sữa Đá",
                    Quantity = 1,
                    Size = "M",
                    Price = 25000
                },
                new DetailInvoice {
                    InvoiceID = 1,
                    NameDrink = "Trà Sữa",
                    Quantity = 1,
                    Size = "M",
                    Price = 25000
                },
                new DetailInvoice {
                    InvoiceID = 1,
                    NameDrink = "Sinh Tố Bơ",
                    Quantity = 1,
                    Size = "S",
                    Price = 25000
                },
                new DetailInvoice {
                    InvoiceID = 1,
                    NameDrink = "Nước Ép Cam",
                    Quantity = 1,
                    Size = "M",
                    Price = 25000
                },
                new DetailInvoice {
                    InvoiceID = 1,
                    NameDrink = "Soda Chanh",
                    Quantity = 1,
                    Size = "M",
                    Price = 25000
                },
                new DetailInvoice {
                    InvoiceID = 2,
                    NameDrink = "Cà Phê Sữa Đá",
                    Quantity = 1,
                    Size = "S",
                    Price = 20000
                },
                new DetailInvoice {
                    InvoiceID = 2,
                    NameDrink = "Trà Sữa",
                    Quantity = 1,
                    Size = "S",
                    Price = 20000
                },
                new DetailInvoice {
                    InvoiceID = 2,
                    NameDrink = "Sinh Tố Bơ",
                    Quantity = 1,
                    Size = "S",
                    Price = 20000
                },
                new DetailInvoice {
                    InvoiceID = 2,
                    NameDrink = "Nước Ép Cam",
                    Quantity = 1,
                    Size = "M",
                    Price = 20000
                },
                new DetailInvoice {
                    InvoiceID = 2,
                    NameDrink = "Soda Chanh",
                    Quantity = 1,
                    Size = "M",
                    Price = 20000
                }
             };

            var deliveryInvoice = new DeliveryInvoice
            {
                DeliveryInvoiceID = invoiceId,
                PhoneNumber = "0123456789",
                ShippingFee = 5000,
                Address = "KTX khu B"
            };
            return new Tuple<List<DetailInvoice>, DeliveryInvoice>(invoiceDetails.Where(x => x.InvoiceID == invoiceId).ToList(), deliveryInvoice);
        }
        public void UpdateInvoiceStatus(int invoiceId, string status)
        {
            
        }
        public Invoice AddInvoice(Invoice invoice, List<DetailInvoice> detailInvoices, DeliveryInvoice deliveryInvoice)
        {
            return invoice;
        }

        public List<Invoice> GetRecentInvoice(int year)
        {
            var invoices = GetInvoices();
            var list = new List<Invoice>();

            list = invoices.Where(invoice => DateTime.Parse(invoice.CreatedAt).Year == year)
                  .OrderByDescending(invoice => DateTime.Parse(invoice.CreatedAt))
                  .Take(5)
                  .ToList();

            return list;
        }
    }

}