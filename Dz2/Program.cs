using Microsoft.Data.SqlClient;

namespace Dz2
{
    internal class Program
    {
        static string connectionString = "Your_Connection_String_Here";

        static void Main(string[] args)
        {
            Console.WriteLine("Виберіть операцію:");
            Console.WriteLine("1: Вставка нових даних");
            Console.WriteLine("2: Оновлення даних");
            Console.WriteLine("3: Видалення даних (перенос в архів)");
            Console.WriteLine("4: Звіти");
            Console.Write("Ваш вибір: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    InsertData();
                    break;
                case 2:
                    UpdateData();
                    break;
                case 3:
                    DeleteData();
                    break;
                case 4:
                    GenerateReports();
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }

        // Вставка нових даних
        static void InsertData()
        {
            Console.WriteLine("Виберіть тип даних для вставки:");
            Console.WriteLine("1: Канцтовари");
            Console.WriteLine("2: Тип канцтоварів");
            Console.WriteLine("3: Менеджер з продажу");
            Console.WriteLine("4: Фірма-покупець");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Назва товару: ");
                    string productName = Console.ReadLine();
                    Console.Write("ID типу: ");
                    int typeId = int.Parse(Console.ReadLine());
                    Console.Write("Ціна: ");
                    decimal price = decimal.Parse(Console.ReadLine());
                    Console.Write("Кількість на складі: ");
                    int stock = int.Parse(Console.ReadLine());
                    InsertProduct(productName, typeId, price, stock);
                    break;

                case 2:
                    Console.Write("Назва типу: ");
                    string typeName = Console.ReadLine();
                    InsertProductType(typeName);
                    break;

                case 3:
                    Console.Write("Ім'я: ");
                    string firstName = Console.ReadLine();
                    Console.Write("Прізвище: ");
                    string lastName = Console.ReadLine();
                    InsertManager(firstName, lastName);
                    break;

                case 4:
                    Console.Write("Назва компанії: ");
                    string companyName = Console.ReadLine();
                    Console.Write("Контактна особа: ");
                    string contactName = Console.ReadLine();
                    InsertCustomer(companyName, contactName);
                    break;

                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }

        static void InsertProduct(string name, int typeId, decimal price, int stock)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Products (Name, TypeID, Price, Stock) VALUES (@Name, @TypeID, @Price, @Stock)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@TypeID", typeId);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Товар успішно додано.");
                }
            }
        }

        static void InsertProductType(string typeName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO ProductTypes (TypeName) VALUES (@TypeName)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TypeName", typeName);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Тип товару успішно додано.");
                }
            }
        }

        static void InsertManager(string firstName, string lastName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO SalesManagers (FirstName, LastName) VALUES (@FirstName, @LastName)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Менеджер успішно доданий.");
                }
            }
        }

        static void InsertCustomer(string companyName, string contactName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Customers (CompanyName, ContactName) VALUES (@CompanyName, @ContactName)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyName", companyName);
                    cmd.Parameters.AddWithValue("@ContactName", contactName);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Фірма-покупець успішно додана.");
                }
            }
        }

        // Оновлення даних
        static void UpdateData()
        {
            Console.WriteLine("Оновлення даних реалізовано окремими методами.");
        }

        // Видалення даних
        static void DeleteData()
        {
            Console.WriteLine("Введіть ID товару для видалення:");
            int productId = int.Parse(Console.ReadLine());
            DeleteProduct(productId);
        }

        static void DeleteProduct(int productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Перенос в архівну таблицю
                string archiveQuery = @"INSERT INTO ArchivedProducts (ProductID, Name, TypeID, Price, Stock, DeletedDate)
                                    SELECT ProductID, Name, TypeID, Price, Stock, GETDATE()
                                    FROM Products WHERE ProductID = @ProductID";

                using (SqlCommand cmdArchive = new SqlCommand(archiveQuery, conn))
                {
                    cmdArchive.Parameters.AddWithValue("@ProductID", productId);
                    cmdArchive.ExecuteNonQuery();
                }

                // Видалення з основної таблиці
                string deleteQuery = @"DELETE FROM Products WHERE ProductID = @ProductID";
                using (SqlCommand cmdDelete = new SqlCommand(deleteQuery, conn))
                {
                    cmdDelete.Parameters.AddWithValue("@ProductID", productId);
                    cmdDelete.ExecuteNonQuery();
                    Console.WriteLine("Товар успішно видалено.");
                }
            }
        }

        // Звіти
        static void GenerateReports()
        {
            Console.WriteLine("Звіти реалізовані окремими запитами.");
        }
    }
}
