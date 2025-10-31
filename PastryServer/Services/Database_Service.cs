using PastryServer.Models;
using PastryServer.Requests;
using SQLite;

namespace PastryServer.Services
{
    public class Database_Service
    {
        private readonly SQLiteAsyncConnection database;

        public Database_Service()
        {
            database = new SQLiteAsyncConnection("pastryshop.db3");
            Initialize().Wait();
        }

        public async Task Initialize()
        {

            //if (File.Exists("pastryshop.db3")) { File.Delete("pastryshop.db3"); }
            await database.CreateTableAsync<User>();
            await database.CreateTableAsync<Verification_Code>();
            await database.CreateTableAsync<Address>();
            await database.CreateTableAsync<Product>();
            await database.CreateTableAsync<Product_Category>();
            await database.CreateTableAsync<Admin>();
            await database.CreateTableAsync<User_Cart>();
            await database.CreateTableAsync<User_Order>();

            string hashed_admin_password = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword("password", workFactor: 6));

            if (await database.Table<Admin>().CountAsync() == 0) { await database.InsertAsync(new Admin { Login = "admin", Password = hashed_admin_password }); }
            if (await database.Table<Product_Category>().CountAsync() == 0)
            {
                await database.InsertAsync(new Product_Category { Name = "Cakes" });
                await database.InsertAsync(new Product_Category { Name = "Pastries" });
                await database.InsertAsync(new Product_Category { Name = "Breads" });
            }

            if (await database.Table<Product>().CountAsync() == 0)
            {
                await database.InsertAsync(new Product { Name = "donut1", Description = "a", Category = "Breads", Price = 20, In_Stock = 2 });
            }
        }

        public async Task Add_User_(string gmail, string password, DateTime date_time)
        {
            try
            {
                var user = new User { Gmail = gmail, Password = password, Registration_Time_And_Date = date_time };

                await database.InsertAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error adding user - " + ex.Message);
            }
        }

        public async Task<bool> Check_Password_By_Gmail_(string gmail, string password)
        {
            try {
                User user = await database.Table<User>().Where(user => user.Gmail == gmail).FirstOrDefaultAsync();
                if (user == null) { Console.WriteLine("[DATABASE]: user doenst exist gmail"); return false; }

                return await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, user.Password));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error checking password - " + ex.Message);
                return false;
            }
        }

        public async Task<bool> Login_(string gmail, string password)
        {
            try
            {
                User user = await database.Table<User>().Where(user => user.Gmail == gmail).FirstOrDefaultAsync();
                if (user == null) { Console.WriteLine("[DATABASE]: user doenst exist"); return false; }
                if (!await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, user.Password))) { return false; }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error during login - " + ex.Message);
                return false;
            }
        }

        public async Task<int> Get_User_Id_By_Gmail(string gmail)
        {
            try
            {
                User user = await database.Table<User>().Where(user => user.Gmail == gmail).FirstOrDefaultAsync();
                if (user == null) { return -1; }

                return user.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error getting user id - " + ex.Message);
                return -1;
            }
        }

        public async Task<List<User>> Get_All_Users_()
        {
            try
            {
                List<User> users = await database.Table<User>().ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error getting all users - " + ex.Message);
                return new List<User>();
            }
        }

        public async Task<User_Cart> Get_User_Cart_(int user_id)
        {
            if (user_id < 0)
            {
                Console.WriteLine("Invalid user ID", nameof(user_id));
                return null!;
            }
            try
            {
                User_Cart cart = await database.Table<User_Cart>().Where(c => c.User_Id == user_id).FirstOrDefaultAsync();

                if (cart == null)
                {
                    await database.InsertAsync(new User_Cart { User_Id = user_id, Bought_Products = new List<Bought_Product>() });
                    cart = await database.Table<User_Cart>().Where(c => c.User_Id == user_id).FirstOrDefaultAsync();
                }

                return cart;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("[DATABASE]: Error getting user cart - " + ex.Message);
                return null!;
            }
        }

        public async Task<List<User_Order>> Get_User_Orders(User user)
        {
            if (user == null)
            {
                Console.WriteLine("User is null", nameof(user));
                return null!;
            }

            if (user.Id < 0)
            {
                Console.WriteLine("Invalid user ID", nameof(user.Id));
                return null!;
            }

            try
            {
                var orders = await database.Table<User_Order>().Where(order => order.User_Id == user.Id).ToListAsync();
                return orders;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error getting user orders - " + ex.Message);
                return new List<User_Order>();
            }
        }

        public async Task Store_Verification_Code_(string gmail, string code, TimeSpan time)
        {
            try
            {
                await database.Table<Verification_Code>().Where(c => c.Gmail == gmail).DeleteAsync();

                await database.InsertAsync(new Verification_Code { Gmail = gmail, Code = code, Expires_At = DateTime.UtcNow.Add(time) });
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error storing verification code - " + ex.Message);
            }
        }

        public async Task<bool> Verify_Verification_Code_(string gmail, string code)
        {
            try
            {
                var requested_code = await database.Table<Verification_Code>().Where(c => c.Gmail == gmail).FirstOrDefaultAsync();
                if (requested_code == null || requested_code.Expires_At < DateTime.UtcNow || code != requested_code.Code) { Console.WriteLine("[DATABASE]: code expired"); return false; }

                await database.Table<Verification_Code>().Where(c => c.Id == requested_code.Id).DeleteAsync();
                return true;
            }
            catch
            {
                Console.WriteLine("[DATABASE]: Error verifying verification code");
                return false;
            }
        }

        public async Task<List<Product>> Get_All_Products_()
        {
            try
            {
                var products = await database.Table<Product>().ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error getting all products - " + ex.Message);
                return new List<Product>();
            }
        }

        public async Task<List<Product_Group>> Get_All_Products_Grouped_By_Category_()
        {
            try
            {

                var products = await database.Table<Product>().ToListAsync();

                var grouped = products
                    .GroupBy(p => p.Category)
                    .Select(g => new Product_Group
                    {
                        Category = g.Key,
                        Products = g.ToList()
                    })
                    .ToList();

                return grouped;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error getting products grouped by category - " + ex.Message);
                return new List<Product_Group>();
            }
        }

        public async Task Update_Products_(Product product)
        {
            try
            {
                await database.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error updating product - " + ex.Message);
            }
        }

        public async Task<(int, string)> Add_Product_(Product product)
        {
            try
            {
                var response = await database.Table<Product>().Where(p => p.Name == product.Name).FirstOrDefaultAsync();
                if (response != null) { return (1, "Product already exists"); }
                await database.InsertAsync(product);
                return (0, "Product added successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error adding product - " + ex.Message);
                return (2, "Error adding product");
            }
        }

        public async Task<List<Product_Category>> Get_All_Product_Categories_()
        {
            try
            {
                var product_categories = await database.Table<Product_Category>().ToListAsync();
                return product_categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error getting product categories - " + ex.Message);
                return new List<Product_Category>();
            }
        }

        public async Task<bool> Admin_Login_(Admin admin)
        {
            try
            {
                var requested = await database.Table<Admin>().Where(a => a.Login == admin.Login).FirstOrDefaultAsync();
                if (requested == null || ! await Task.Run(() => BCrypt.Net.BCrypt.Verify(admin.Password, requested.Password))) { return false; }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DATABASE]: Error during admin login - " + ex.Message);
                return false;
            }
        }

        public async Task Add_To_Cart_(int user_id, int product_id, int product_quantity)
        {
            User_Cart cart = await Get_User_Cart_(user_id);

            if (cart == null) { await database.InsertAsync(new User_Cart { User_Id = user_id, Bought_Products = new List<Bought_Product>() }); return; }

            cart.Bought_Products.Add(new Bought_Product { Product_Id = product_id, Quantity = product_quantity });
        }
    }
}
