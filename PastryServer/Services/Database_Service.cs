using SQLite;
using BCrypt.Net;
using System.Data;
using PastryServer.Models;

namespace PastryServer.Services
{
    public class Database_Service
    {
        private readonly SQLiteAsyncConnection database;

        public Database_Service()
        {
            if (File.Exists("pastryshop.db3")) { File.Delete("pastryshop.db3"); }

            database = new SQLiteAsyncConnection("pastryshop.db3");
            database.CreateTableAsync<User>().Wait();
            database.CreateTableAsync<Verification_Code>().Wait();
            database.CreateTableAsync<Address>().Wait();
            database.CreateTableAsync<Product>().Wait();
            database.CreateTableAsync<Product_Category>().Wait();
        }

        public async Task Add_User_(string gmail, string password)
        {
            var user = new User { Gmail = gmail, Password = password };

            await database.InsertAsync(user);
        }

        public async Task<bool> Check_Password_By_Gmail_(string gmail, string password)
        {
            User user = await database.Table<User>().Where(user => user.Gmail == gmail).FirstOrDefaultAsync();
            if (user == null) { Console.WriteLine("[DATABASE]: user doenst exist gmail"); return false; }

            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        public async Task<bool> Login_(string gmail, string password)
        {
            User user = await database.Table<User>().Where(user => user.Gmail == gmail).FirstOrDefaultAsync();
            if (user == null) { Console.WriteLine("[DATABASE]: user doenst exist"); return false; }
            if (user.Password != password) {  return false; }

            return true;
        }

        public async Task Store_Verification_Code_(string gmail, string code, TimeSpan time)
        {
            await database.Table<Verification_Code>().Where(c => c.Gmail == gmail).DeleteAsync();

            await database.InsertAsync(new Verification_Code { Gmail = gmail, Code = code, Expires_At = DateTime.UtcNow.Add(time) });
        }

        public async Task<bool> Verify_Verification_Code_(string gmail, string code)
        {
            var requested_code = await database.Table<Verification_Code>().Where(c => c.Gmail == gmail).FirstOrDefaultAsync();
            if (requested_code == null || requested_code.Expires_At < DateTime.UtcNow || code != requested_code.Code) { Console.WriteLine("[DATABASE]: code expired"); return false; }

            await database.Table<Verification_Code>().Where(c => c.Id == requested_code.Id).DeleteAsync();
            return true;
        }

        public async Task<List<Product>> Get_All_Products_()
        {
            var products = await database.Table<Product>().ToListAsync();
            return products;
        }

        public async Task Update_Products_(Product product)
        {
            await database.UpdateAsync(product);
        }
    }
}
