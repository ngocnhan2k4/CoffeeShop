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
using dotenv.net;
using System.Globalization;
namespace CoffeeShop.Service.DataAccess
{

    public class SqlServerDao : IDao
    {
        private readonly string connectionString;

        public SqlServerDao()
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            string server = Environment.GetEnvironmentVariable("SERVER") ?? "127.0.0.1";
            string database = Environment.GetEnvironmentVariable("DATABASE") ?? "coffee-shop";
            string userId = Environment.GetEnvironmentVariable("USERID") ?? "sa";
            string password = Environment.GetEnvironmentVariable("PASSWORD") ?? "SqlServer@123";
            connectionString = $"Server={server};Database={database};User Id={userId};Password={password};TrustServerCertificate=True";
        }

        public List<Discount> GetDiscounts()
        {
            var categories = GetCategories();
            var discounts = new List<Discount>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT name, discount_percent, valid_until, category_id, is_active FROM discount", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var discount = new Discount();
                discount.Name = reader.GetString(0);
                discount.DiscountPercent = reader.GetDouble(1);
                discount.ValidUntil = reader.GetDateTime(2);
                discount.CategoryID = reader.GetInt32(3);
                discount.IsActive = reader.GetBoolean(4);
                discount.CategoryName = categories.FirstOrDefault(c => c.CategoryID == discount.CategoryID)?.CategoryName;
                discounts.Add(discount);
            }
            return discounts;
        }

        public bool AddDiscounts(List<Discount> discounts)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                using var cmd1 = conn.CreateCommand();
                cmd1.Transaction = transaction;
                cmd1.CommandText = "DELETE FROM discount";
                cmd1.ExecuteNonQuery();

                foreach (var discount in discounts)
                {
                    using var cmd = conn.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = "INSERT INTO discount ( name, discount_percent, valid_until, category_id, is_active)" +
                        "VALUES (@name, @discountPercent, @validUntil, @categoryId, @isActive)";
                    cmd.Parameters.AddWithValue("@name", discount.Name);
                    cmd.Parameters.AddWithValue("@discountPercent", discount.DiscountPercent);
                    cmd.Parameters.AddWithValue("@validUntil", discount.ValidUntil);
                    cmd.Parameters.AddWithValue("@categoryId", discount.CategoryID);
                    cmd.Parameters.AddWithValue("@isActive", discount.IsActive);
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
                list.Add(di);
            }
            return list;
        }

        public List<Drink> GetDrinks()
        {
            var discounts = GetDiscounts();
            var discountManager = new DiscountManager(discounts);
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
                    Sizes = new List<Size>()
                };

                drink.Discount = discountManager.GetDiscountForCategory(reader.GetInt32(1));

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
            var discounts = GetDiscounts();
            var discountManager = new DiscountManager(discounts);
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
                    SELECT id,name, category_id, description, image, size, price, stock, (select sum(dd.stock)  from drink dd where d.name = dd.name ) as totalQuantity
                    from drink d
                    WHERE name LIKE @keyword AND (@categoryID = -1 OR category_id = @categoryID)
                    {sortClause}
                    """;
            cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
            cmd.Parameters.AddWithValue("@categoryID", categoryID);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var drink = drinks.FirstOrDefault(d => d.Name == reader.GetString(1));
                if (drink == null)
                {
                    drink = new Drink
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        CategoryID = reader.GetInt32(2),
                        Description = reader.GetString(3),
                        ImageString = reader.GetString(4),
                        Sizes = new List<Size>()
                    };

                    drink.Discount = discountManager.GetDiscountForCategory(reader.GetInt32(2));
                    drinks.Add(drink);
                }
                drink.Sizes.Add(new Size
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(5),
                    Price = reader.GetInt32(6),
                    Stock = reader.GetInt32(7)
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
            cmd.CommandText = "SELECT * FROM invoice Order by id desc";
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
                SELECT COUNT(*) FROM Invoice WHERE YEAR(Created_At) = @Year and status != 'Cancel'
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
                SELECT SUM(total) FROM Invoice WHERE YEAR(Created_At) = @Year and status != 'Cancel'
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
                SELECT MONTH(Created_At), SUM(Total) FROM Invoice WHERE YEAR(Created_At) = @Year and status != 'Cancel' GROUP BY MONTH(Created_At)
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
                WHERE YEAR(i.Created_At) = @Year and i.status != 'Cancel'
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
                SELECT c.name, SUM(di.Quantity * di.price) AS TotalRevenue
                FROM invoice_detail di
                JOIN invoice i ON di.Invoice_ID = i.id
                JOIN drink d ON di.drink_id = d.id
                JOIN Category c ON d.Category_ID = c.id
                WHERE YEAR(i.Created_At) = @Year and i.status != 'Cancel'
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



        public Tuple<List<DetailInvoice>, DeliveryInvoice> GetDetailInvoicesOfId(int invoiceId)
        {
            var detailInvoices = new List<DetailInvoice>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
                SELECT invoice_detail.drink_id, drink.name, invoice_detail.quantity, drink.size, invoice_detail.price, invoice_detail.note
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
                    Price = reader.GetInt32(4),
                    Note = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                };
                detailInvoices.Add(di);
            }
            reader.Close();
            DeliveryInvoice deliveryInvoice = null;
            // Fetch DeliveryInvoice
            using var cmd2 = new SqlCommand("""
            SELECT * FROM delivery_invoice WHERE invoice_id = @invoiceId
            """, conn);
            cmd2.Parameters.AddWithValue("@invoiceId", invoiceId);
            using var reader2 = cmd2.ExecuteReader();
            if (reader2.Read())
            {
                deliveryInvoice = new DeliveryInvoice
                {
                    DeliveryInvoiceID = reader2.GetInt32(0),
                    Address = reader2.GetString(1),
                    PhoneNumber = reader2.GetString(2),
                    ShippingFee = reader2.GetInt32(3)
                };
            }
            reader2.Close();
            // If no DeliveryInvoice is found, create a default one
            if (deliveryInvoice == null)
            {
                deliveryInvoice = new DeliveryInvoice
                {
                    DeliveryInvoiceID = invoiceId,
                    Address = "",
                    PhoneNumber = "",
                    ShippingFee = 0
                };
            }

            return new Tuple<List<DetailInvoice>, DeliveryInvoice>(detailInvoices, deliveryInvoice);
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
                int customerID = getCustomerIDFromInvoice(invoiceId);
                
                if (status == "Paid" && customerID != 0)
                {
                    string customerType = getCustomerType(customerID);
                    int totalAmount = getTotalAmountOfInvoice(invoiceId);
                    List<MemberCard> memberCards = GetMemberCards();
                    int discount = 0;
                    foreach (var memberCard in memberCards)
                    {
                        if (memberCard.CardName == customerType)
                        {
                            discount = memberCard.Discount;
                        }
                    }
                    using (SqlCommand command = new SqlCommand("UPDATE customer SET total_money = total_money + @TotalMoney WHERE id = @CustomerID", connection))
                    {
                        command.Parameters.AddWithValue("@TotalMoney", totalAmount / (1-discount/100));
                        command.Parameters.AddWithValue("@CustomerID", customerID);
                        command.ExecuteNonQuery();
                    }
                    //if (customerType == "Thẻ thành viên")
                    //{
                    //    using (SqlCommand command = new SqlCommand("UPDATE customer SET total_money = total_money + @TotalMoney WHERE id = @CustomerID", connection))
                    //    {
                    //        command.Parameters.AddWithValue("@TotalMoney", totalAmount/0.95);
                    //        command.Parameters.AddWithValue("@CustomerID", customerID);
                    //        command.ExecuteNonQuery();
                    //    }
                    //}
                    //else if (customerType == "Thẻ bạc")
                    //{
                    //    using (SqlCommand command = new SqlCommand("UPDATE customer SET total_money = total_money + @TotalMoney WHERE id = @CustomerID", connection))
                    //    {
                    //        command.Parameters.AddWithValue("@TotalMoney", totalAmount/0.9);
                    //        command.Parameters.AddWithValue("@CustomerID", customerID);
                    //        command.ExecuteNonQuery();
                    //    }
                    //}
                    //else if (customerType == "Thẻ vàng")
                    //{
                    //    using (SqlCommand command = new SqlCommand("UPDATE customer SET total_money = total_money + @TotalMoney WHERE id = @CustomerID", connection))
                    //    {
                    //        command.Parameters.AddWithValue("@TotalMoney", totalAmount / 0.85);
                    //        command.Parameters.AddWithValue("@CustomerID", customerID);
                    //        command.ExecuteNonQuery();
                    //    }
                    //}

                }

            }
        }
        public Invoice AddInvoice(Invoice invoice, List<DetailInvoice> detailInvoices, DeliveryInvoice deliveryInvoice, int customerID)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                // Insert the invoice and get the generated ID
                using var invoiceCmd = new SqlCommand("""
                INSERT INTO invoice (created_at, total, method, status, customer_name, has_delivery, member_card_id)
                VALUES (@created_at, @total, @method, @status, @customer_name, @has_delivery, @member_card_id);
                SELECT SCOPE_IDENTITY();
                """, conn, transaction);
                DateTime dateTime;

                string format = "dd/MM/yyyy hh:mm:ss tt";

                CultureInfo provider = new CultureInfo("vi-VN");

                DateTime.TryParseExact(invoice.CreatedAt, format, provider, DateTimeStyles.None, out dateTime);

                invoiceCmd.Parameters.AddWithValue("@created_at", dateTime);
                invoiceCmd.Parameters.AddWithValue("@total", invoice.TotalAmount);
                invoiceCmd.Parameters.AddWithValue("@method", invoice.PaymentMethod);
                invoiceCmd.Parameters.AddWithValue("@status", invoice.Status);
                invoiceCmd.Parameters.AddWithValue("@customer_name", invoice.CustomerName);
                invoiceCmd.Parameters.AddWithValue("@has_delivery", invoice.HasDelivery);
                invoiceCmd.Parameters.AddWithValue("@member_card_id", customerID);
                int invoiceId = Convert.ToInt32(invoiceCmd.ExecuteScalar());

                // Insert the detail invoices
                foreach (var detail in detailInvoices)
                {
                    using var detailCmd = new SqlCommand("""
                    INSERT INTO invoice_detail (invoice_id, drink_id, quantity, price, note)
                    VALUES (@invoice_id, @drink_id, @quantity, @price, @note);
                    """, conn, transaction);
                    detailCmd.Parameters.AddWithValue("@invoice_id", invoiceId);
                    detailCmd.Parameters.AddWithValue("@drink_id", detail.DrinkId);
                    detailCmd.Parameters.AddWithValue("@quantity", detail.Quantity);
                    detailCmd.Parameters.AddWithValue("@price", detail.Price);
                    detailCmd.Parameters.AddWithValue("@note", detail.Note ?? string.Empty);
                    detailCmd.ExecuteNonQuery();
                }

                // Insert the delivery invoice if it exists
                if (invoice.HasDelivery == "Y")
                {
                    using var deliveryCmd = new SqlCommand("""
                    INSERT INTO delivery_invoice (invoice_id, address, phone, shipping_fee)
                    VALUES (@invoice_id, @address, @phone, @shipping_fee);
                    """, conn, transaction);
                    deliveryCmd.Parameters.AddWithValue("@invoice_id", invoiceId);
                    deliveryCmd.Parameters.AddWithValue("@address", deliveryInvoice.Address);
                    deliveryCmd.Parameters.AddWithValue("@phone", deliveryInvoice.PhoneNumber);
                    deliveryCmd.Parameters.AddWithValue("@shipping_fee", deliveryInvoice.ShippingFee);
                    deliveryCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                invoice.InvoiceID = invoiceId;
                return invoice; // Return the inserted invoice
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
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
        public List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                            SELECT * FROM customer
                            """;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var customer = new Customer
                {
                    customerID = reader.GetInt32(0),
                    customerName = reader.GetString(1),
                    totalMonney = reader.GetDecimal(2),
                    totalPoint = reader.GetDouble(3),
                    type = reader.GetString(4)
                };

                customers.Add(customer);
            }
            reader.Close();
            return customers;
        }
        public Tuple<List<Customer>, int> GetCustomers(int page, int rowsPerPage, string keyword)
        {
            var customers = GetCustomers();
            var query = from c in customers
                        where c.customerName.ToLower().Contains(keyword.ToLower())
                        select c;

            var result = query
                .Skip((page - 1) * rowsPerPage)
                .Take(rowsPerPage);

            return new Tuple<List<Customer>, int>(
                result.ToList(),
                query.Count()
            );
        }

        public bool AddCustomer(Customer customer)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = "INSERT INTO customer (customer_name, total_money, total_point, customer_type) VALUES (@name, @totalMoney, @totalPoint, @customerType)";
                cmd.Parameters.AddWithValue("@name", customer.customerName);
                cmd.Parameters.AddWithValue("@totalMoney", customer.totalMonney);
                cmd.Parameters.AddWithValue("@totalPoint", customer.totalPoint);
                cmd.Parameters.AddWithValue("@customerType", customer.type);
                cmd.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public bool UpdateCustomer(Customer customer)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = "UPDATE customer SET customer_name = @name WHERE id = @id";
                cmd.Parameters.AddWithValue("@name", customer.customerName);
                cmd.Parameters.AddWithValue("@id", customer.customerID);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public bool DeleteCustomer(int id)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = "DELETE FROM customer WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }
        public string getCustomerType(int customerID)
        {
            List<Customer> customers = GetCustomers();
            Customer customer = customers.FirstOrDefault(c => c.customerID == customerID);
            if (customer == null)
            {
                return "";
            }
            return customer.type;
        }
        public int getCustomerIDFromInvoice(int invoiceID)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
                SELECT member_card_id FROM invoice WHERE id = @invoiceID
                """, conn);
            cmd.Parameters.AddWithValue("@invoiceID", invoiceID);
            return (int)cmd.ExecuteScalar();
        }
        public int getTotalAmountOfInvoice(int invoiceID)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("""
                SELECT total FROM invoice WHERE id = @invoiceID
                """, conn);
            cmd.Parameters.AddWithValue("@invoiceID", invoiceID);
            return (int)cmd.ExecuteScalar();
        }
        public List<MemberCard> GetMemberCards()
        {
            List<MemberCard> memberCards = new List<MemberCard>();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT * FROM member_card", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var memberCard = new MemberCard
                {
                    CardName = reader.GetString(0),
                    Discount = reader.GetInt32(1)
                };
                memberCards.Add(memberCard);
            }
            return memberCards;
        }
        public bool UpdateMemberCard(int member, int silver, int gold)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = "UPDATE member_card SET discount = @discount WHERE card_name = @cardName";
                cmd.Parameters.AddWithValue("@discount", member);
                cmd.Parameters.AddWithValue("@cardName", "Thẻ thành viên");
                cmd.ExecuteNonQuery();

                using var cmd2 = conn.CreateCommand();
                cmd2.Transaction = transaction;
                cmd2.CommandText = "UPDATE member_card SET discount = @discount WHERE card_name = @cardName";
                cmd2.Parameters.AddWithValue("@discount", silver);
                cmd2.Parameters.AddWithValue("@cardName", "Thẻ bạc");
                cmd2.ExecuteNonQuery();

                using var cmd3 = conn.CreateCommand();
                cmd3.Transaction = transaction;
                cmd3.CommandText = "UPDATE member_card SET discount = @discount WHERE card_name = @cardName";
                cmd3.Parameters.AddWithValue("@discount", gold);
                cmd3.Parameters.AddWithValue("@cardName", "Thẻ vàng");
                cmd3.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }
    }
}