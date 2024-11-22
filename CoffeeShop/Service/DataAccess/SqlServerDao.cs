using CoffeeShop.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using static CoffeeShop.Service.DataAccess.IDao;

namespace CoffeeShop.Service.DataAccess
{

    public class SqlServerDao : IDao
    {
        private readonly string connectionString = "Server=127.0.0.1;Database=coffee-shop;User ID=sa;Password=SqlServer@123;TrustServerCertificate=True;";
        public List<Category> GetCategories()
        {
            var categories = new List<Category>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT name, id FROM category", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var category = new Category();
                category.CategoryName = reader.GetString(0);
                category.CategoryID = reader.GetInt32(1);
                categories.Add(category);
            }
            return categories;
        }
        /// <summary>
        /// The function to add a list category into database
        /// </summary>
        /// <param name="categories"></param>
        /// <returns>bool</returns>
        public bool AddCategories(List<Category> categories)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                foreach (var category in categories)
                {
                    using var cmd = conn.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = "INSERT INTO category (id, name) VALUES (@id, @name)";
                    cmd.Parameters.AddWithValue("@name", category.CategoryName);
                    cmd.Parameters.AddWithValue("@id", category.CategoryID);
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }
        public List<DeliveryInvoice> GetDeliveryInvoices()
        {
            var list = new List<DeliveryInvoice>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT * FROM delivery_invoice", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var di = new DeliveryInvoice();
                di.DeliveryInvoiceID = reader.GetInt32(0);
                di.Address = reader.GetString(1);
                di.PhoneNumber = reader.GetString(2);
                di.ShippingFee = reader.GetInt32(3);
                list.Add(di);
            }
            return list;
        }

        public List<DetailInvoice> GetDetailInvoices()
        {
            var list = new List<DetailInvoice>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            using var cmd = new SqlCommand("""
                        SELECT invoice_detail.drink_id, drink.name, invoice_detail.quantity, drink.size, invoice_detail.price
                        FROM invoice_detail,drink
                        WHERE invoice_detail.drink_id = drink.id
                        """, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var di = new DetailInvoice();
                di.InvoiceID = reader.GetInt32(0);
                di.NameDrink = reader.GetString(1);
                di.Quantity = reader.GetInt32(2);
                di.Size = reader.GetString(3);
                di.Price = reader.GetInt32(4);
            }
            return list;
        }

        public List<Drink> GetDrinks()
        {
            var drinks = new List<Drink>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                            SELECT name,category_id,description,image FROM drink
                            GROUP BY name,category_id,description,image
                            """;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var drink = new Drink
                {
                    Name = reader.GetString(0),
                    Description = reader.GetString(2),
                    ImageString = reader.GetString(3),
                    CategoryID = reader.GetInt32(1),
                    Discount = 0,
                    Sizes = new List<Size>()
                };
                drinks.Add(drink);
            }
            reader.Close();
            foreach (var drink in drinks)
            {
                var sizeCmd = conn.CreateCommand();
                sizeCmd.CommandText = "SELECT size, price, stock FROM drink WHERE name = @name GROUP BY size, price, stock ORDER BY size DESC";
                sizeCmd.Parameters.AddWithValue("@name", drink.Name);
                using var sizeReader = sizeCmd.ExecuteReader();
                while (sizeReader.Read())
                {
                    drink.Sizes.Add(new Size
                    {
                        Name = sizeReader.GetString(0),
                        Price = sizeReader.GetInt32(1),
                        Stock = sizeReader.GetInt32(2)
                    });
                }
                sizeReader.Close();
            }
            return drinks;
        }

        public Tuple<List<Drink>, int> GetDrinks(int page, int rowsPerPage, string keyword, int categoryID, Dictionary<string, IDao.SortType> sortOptions)
        {
            var drinks = new List<Drink>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string sortClause = "ORDER BY ";
            if (sortOptions.Count == 0)
            {
                sortClause += "name";
            }
            foreach (var option in sortOptions)
            {
                if (option.Key == "Price")
                {
                    sortClause += (option.Value == SortType.Ascending) ? "price ASC" : "price DESC";
                }
                else if (option.Key == "Stock")
                {
                    if (option.Value == SortType.Descending)
                    {
                        sortClause += "totalQuantity DESC";
                    }

                }
            }
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"""
                    SELECT name, category_id, description, image, size, price, stock, (select sum(dd.stock)  from drink dd where d.name = dd.name ) as totalQuantity
                    from drink d
                    WHERE name LIKE @keyword AND (@categoryID = -1 OR category_id = @categoryID)
                    {sortClause}
                    """;
            cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
            cmd.Parameters.AddWithValue("@categoryID", categoryID);
            /*      cmd.Parameters.AddWithValue("@offset", (page - 1) * rowsPerPage);
                  cmd.Parameters.AddWithValue("@rowsPerPage", rowsPerPage);*/

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var drink = drinks.FirstOrDefault(d => d.Name == reader.GetString(0));
                if (drink == null)
                {
                    drink = new Drink
                    {
                        Name = reader.GetString(0),
                        CategoryID = reader.GetInt32(1),
                        Description = reader.GetString(2),
                        ImageString = reader.GetString(3),
                        Discount = 0,
                        Sizes = new List<Size>()
                    };
                    drinks.Add(drink);
                }
                drink.Sizes.Add(new Size
                {
                    Name = reader.GetString(4),
                    Price = reader.GetInt32(5),
                    Stock = reader.GetInt32(6)
                });
            }

            var result = drinks
             .Skip((page - 1) * rowsPerPage)
             .Take(rowsPerPage);

            return new Tuple<List<Drink>, int>(result.ToList(), drinks.Count);
        }

        /// <summary>
        /// The function to add a list drink into database
        /// </summary>
        /// <param name="drinks"></param>
        /// <returns>bool</returns>
        public bool AddDrinks(List<Drink> drinks)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                foreach (var drink in drinks)
                {
                    foreach (var size in drink.Sizes)
                    {
                        if (size.Stock == 0) continue;
                        using var cmd = conn.CreateCommand();
                        cmd.Transaction = transaction;
                        cmd.CommandText = """
                                    INSERT INTO drink (name, category_id, description, image, size, stock, price)
                                    VALUES (@name, @category_id, @description, @image, @size, @stock, @price)
                                    """;
                        cmd.Parameters.AddWithValue("@name", drink.Name);
                        cmd.Parameters.AddWithValue("@category_id", drink.CategoryID);
                        cmd.Parameters.AddWithValue("@description", drink.Description);
                        cmd.Parameters.AddWithValue("@image", drink.ImageString);
                        cmd.Parameters.AddWithValue("@size", size.Name);
                        cmd.Parameters.AddWithValue("@stock", size.Stock);
                        cmd.Parameters.AddWithValue("@price", size.Price);
                        cmd.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }
        public List<Invoice> GetInvoices()
        {
            var list = new List<Invoice>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM invoice";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var invoice = new Invoice();
                invoice.InvoiceID = reader.GetInt32(0);
                invoice.CreatedAt = reader.GetDateTime(1).ToString("yyyy-MM-dd");
                invoice.TotalAmount = reader.GetInt32(2);
                invoice.PaymentMethod = reader.GetString(3);
                invoice.Status = reader.GetString(4);
                invoice.CustomerName = reader.GetString(5);
                invoice.HasDelivery = reader.GetString(6);
                list.Add(invoice);
            }
            return list;
        }

        public int CalculateNumberOrders(int year)
        {
            int total = 0;
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
                SELECT COUNT(*) FROM Invoice WHERE YEAR(Created_At) = @Year
                """, conn);
            cmd.Parameters.AddWithValue("@Year", year);
            total = (int)cmd.ExecuteScalar();
            return total;
        }

        public int CalculateTotalCost()
        {
            int result = 0;
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
                SELECT SUM(price * stock) FROM Drink
                """, conn);
            result = (int)cmd.ExecuteScalar();
            return result;
        }

        public int CalculateRevenue(int year)
        {
            int result = 0;
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
                SELECT SUM(total) FROM Invoice WHERE YEAR(Created_At) = @Year
                """, conn);
            cmd.Parameters.AddWithValue("@Year", year);
            result = (int)cmd.ExecuteScalar();
            return result;
        }

        public int CalculateProfit(int year)
        {
            return CalculateRevenue(year) - CalculateTotalCost();
        }

        public List<int> CalculateYears()
        {
            List<int> years = new List<int>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
                SELECT DISTINCT YEAR(Created_At) FROM Invoice
                """, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                years.Add(reader.GetInt32(0));
            }
            return new() { years.Max(), years.Min() };
        }

        public List<int> CalculateMonthlyRevenue(int year)
        {
            List<int> result = Enumerable.Repeat(0, 12).ToList();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
                SELECT MONTH(Created_At), SUM(Total) FROM Invoice WHERE YEAR(Created_At) = @Year GROUP BY MONTH(Created_At)
                """, conn);
            cmd.Parameters.AddWithValue("@Year", year);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int month = reader.GetInt32(0);
                int reveune = reader.GetInt32(1);
                result[month - 1] = reveune;
            }
            return result;
        }

        public List<string> CalculateTopDrinks(int year)
        {
            List<string> result = new List<string>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
                SELECT TOP 5 d.Name, SUM(di.Quantity) AS TotalQuantity
                FROM invoice_detail di
                JOIN invoice i ON di.Invoice_ID = i.id
                JOIN drink d ON di.drink_id = d.id
                WHERE YEAR(i.Created_At) = @Year
                GROUP BY d.Name
                ORDER BY TotalQuantity DESC
                """, conn);
            cmd.Parameters.AddWithValue("@Year", year);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader.GetString(0));
            }
            return result;
        }

        public Dictionary<string, int> CalculateRevenueCategory(int year)
        {
            Dictionary<string, int> revenueByCategory = new Dictionary<string, int>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
                SELECT c.name, SUM(di.Quantity * d.price) AS TotalRevenue
                FROM invoice_detail di
                JOIN invoice i ON di.Invoice_ID = i.id
                JOIN drink d ON di.drink_id = d.id
                JOIN Category c ON d.Category_ID = c.id
                WHERE YEAR(i.Created_At) = @Year
                GROUP BY c.name
                """, conn);
            cmd.Parameters.AddWithValue("@Year", year);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                revenueByCategory.Add(reader.GetString(0), reader.GetInt32(1));
            }
            return revenueByCategory;
        }


        public List<Invoice> GetListInvoiceId()
        {
            var invoices = new List<Invoice>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
                SELECT id, created_at, total, method, status, customer_name, has_delivery 
                FROM invoice 
                Order by created_at desc
                """, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var invoice = new Invoice
                {
                    InvoiceID = reader.GetInt32(0),
                    CreatedAt = reader.GetDateTime(1).ToString("yyyy-MM-dd"),
                    TotalAmount = reader.GetInt32(2),
                    PaymentMethod = reader.GetString(3),
                    Status = reader.GetString(4),
                    CustomerName = reader.GetString(5),
                    HasDelivery = reader.GetString(6)
                };
                invoices.Add(invoice);
            }
            return invoices;
        }

        public List<DetailInvoice> GetDetailInvoicesOfId(int invoiceId)
        {
            var detailInvoices = new List<DetailInvoice>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
        SELECT invoice_detail.drink_id, drink.name, invoice_detail.quantity, drink.size, invoice_detail.price
        FROM invoice_detail
        JOIN drink ON invoice_detail.drink_id = drink.id
        WHERE invoice_detail.invoice_id = @invoiceId
        """, conn);
            cmd.Parameters.AddWithValue("@invoiceId", invoiceId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var di = new DetailInvoice
                {
                    InvoiceID = reader.GetInt32(0),
                    NameDrink = reader.GetString(1),
                    Quantity = reader.GetInt32(2),
                    Size = reader.GetString(3),
                    Price = reader.GetInt32(4)
                };
                detailInvoices.Add(di);
            }
            return detailInvoices;
        }
        public void UpdateInvoiceStatus(int invoiceId, string status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE invoice SET Status = @Status WHERE id = @InvoiceID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@InvoiceID", invoiceId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}