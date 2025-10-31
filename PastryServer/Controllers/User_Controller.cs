using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using PastryServer.Models;
using PastryServer.Services;
using PastryServer.Helper_Files;
using PastryServer.Requests;

namespace PastryServer.Controllers
{
    [ApiController]
    [RequireHttps]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly Gmail_Sender gmail_sender;
        private readonly string gmail_login;
        private readonly string gmail_password;

        private readonly Database_Service database;

        public AuthController(Gmail_Sender gmailSender, Database_Service databaseService, IConfiguration config)
        {
            gmail_login = config["Gmail:Login"] ?? null!;
            if (gmail_login == null) { throw new Exception("Gmail login is null in AuthController"); }
            
            gmail_password = config["Gmail:Password"] ?? null!;
            if (gmail_password == null) { throw new Exception("Gmail password is null in AuthController"); }


            gmail_sender = gmailSender;
            database = databaseService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register_([FromBody] Register_Request request)
        {
            if (request == null) { Console.WriteLine("[MAIN]: no request"); return BadRequest("Request is null"); }
            if (request.gmail == null || request.password == null || request.verification_code == null) { Console.WriteLine("[MAIN]: some fields are null"); return BadRequest("Some fields are null"); }

            string gmail = request.gmail;
            if (string.IsNullOrWhiteSpace(gmail)) { Console.WriteLine("[MAIN]: no gmail"); return BadRequest("Email required"); }
            if (!Checks.Is_Gmail_Valid_(gmail)) { Console.WriteLine("[MAIN]: gmail isnt valid"); return BadRequest("Email isn\'t valid"); }

            string password = request.password;
            if (string.IsNullOrWhiteSpace(password)) { Console.WriteLine("[MAIN]: no password"); return BadRequest("Password required"); }
            if (!Checks.Is_Password_Valid_(password)) { Console.WriteLine("[MAIN]: password isnt valid"); return BadRequest("Password isn\'t valid"); }

            string verification_code = request.verification_code;
            if (string.IsNullOrWhiteSpace(verification_code)) { Console.WriteLine("[MAIN]: no code"); return BadRequest("Verification Code required"); }

            if (!await database.Verify_Verification_Code_(gmail, verification_code)) { Console.WriteLine("[MAIN]: wrong code");  return BadRequest("Wrong Verification Code"); }
            if (await database.Login_(gmail, password)) { Console.WriteLine("[MAIN]: user exists"); return BadRequest("User Already Exists");  }

            string password_hash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 6));

            await database.Add_User_(gmail, password_hash, DateTime.UtcNow);

            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login_([FromBody] Login_Request request)
        {
            if (request == null) { Console.WriteLine("[MAIN]: no request"); return BadRequest("Request is null"); }
            if (request.gmail == null || request.password == null) { Console.WriteLine("[MAIN]: some fields are null"); return BadRequest("Some fields are null"); }

            string gmail = request.gmail;
            if (string.IsNullOrWhiteSpace(gmail)) { Console.WriteLine("[MAIN]: no gmail"); return BadRequest("Email required"); }
            if (!Checks.Is_Gmail_Valid_(gmail)) { Console.WriteLine("[MAIN]: gmail isnt valid"); return BadRequest("Email isn\'t valid"); }

            string password = request.password;
            if (string.IsNullOrWhiteSpace(password)) { Console.WriteLine("[MAIN]: no password"); return BadRequest("Password required"); }
            if (!Checks.Is_Password_Valid_(password)) { Console.WriteLine("[MAIN]: password isnt valid"); return BadRequest("Password isn\'t valid"); }

            bool password_is_correct = await database.Check_Password_By_Gmail_(gmail, password);
            if (!password_is_correct) { return Unauthorized("User doen\'t exist or password is invalid"); }

            return Ok();
        }

        [HttpPost("GetUserIdByGmail")]
        public async Task<ActionResult<int>> Get_User_Id__By_Gmail_([FromBody] Gmail_Request req)
        {
            if (req == null) { Console.WriteLine("No request in Get_User_Id__By_Gmail_"); return BadRequest("Request is null"); }
            if (req.gmail == null) { Console.WriteLine("No gmail in Get_User_Id__By_Gmail_"); return BadRequest("Gmail is null"); }

            int user_id = await database.Get_User_Id_By_Gmail(req.gmail);
            if (user_id == -1) { Console.WriteLine("Something went really wrong in login, user doens't exist"); return Unauthorized("User doenst exists or soemthing is really wrong"); }

            return Ok(user_id);
        }
        
        [HttpPost("SentVerificationGmail")]
        public async Task<IActionResult> Sent_Verification_Gmail([FromBody] Gmail_Request request)
        {
            if (request == null) { Console.WriteLine("[MAIN]: no request"); return BadRequest("Request is null"); }
            if (request.gmail == null) { Console.WriteLine("[MAIN]: no gmail"); return BadRequest("Email required"); }

            string gmail = request.gmail;
            if (string.IsNullOrWhiteSpace(gmail)) { Console.WriteLine("[MAIN]: no gmail"); return BadRequest("Email required"); }
            if (!Checks.Is_Gmail_Valid_(gmail)) { Console.WriteLine("[MAIN]: gmail isnt valid"); return BadRequest("Email isn\'t valid"); }

            string code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            await database.Store_Verification_Code_(gmail, code, TimeSpan.FromMinutes(5));
            await gmail_sender.Send_Email_Async(gmail_password, gmail_login, gmail, "Verification code", code);

            Console.WriteLine("[MAIN]: code sent");

            return Ok();
        }

        [HttpPost("GetCart")]
        public async Task<ActionResult<User_Cart>> Get_User_Cart([FromBody] User_Id__Request user_id__request)
        {
            if (user_id__request == null) { Console.WriteLine("User id request is null"); return BadRequest("User id request is null");  }
            if (user_id__request.user_Id < 0) { Console.WriteLine("User id is invalid"); return BadRequest("User id is invalid"); }

            User_Cart? user_cart = await database.Get_User_Cart_(user_id__request.user_Id);
            if (user_cart == null) { Console.WriteLine("User cart not found"); return NotFound("User cart not found"); }


            return Ok(user_cart);
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<List<User_Order>>> Get_User_Orders([FromBody] User user)
        {
            if (user == null) { return BadRequest("User is null"); }
            if (user.Id < 0) { return BadRequest("User id is invalid"); }

            List<User_Order>? user_order = await database.Get_User_Orders(user);
            if (user_order == null) { return NotFound("User orders not found"); }
            return Ok(user_order);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> Add_To_Cart([FromBody] Add_To_Cart_Request add_to_cart_request)
        {
            if (add_to_cart_request == null) { return BadRequest("Add to cart request is null"); }
            if (add_to_cart_request.User_Id < 0) { return BadRequest("User id is invalid"); }
            if (add_to_cart_request.Product_Id < 0) { return BadRequest("Product id is invalid"); }
            if (add_to_cart_request.Product_Quantity <= 0) { return BadRequest("Product quantity is invalid"); }

            await database.Add_To_Cart_(add_to_cart_request.User_Id, add_to_cart_request.Product_Id, add_to_cart_request.Product_Quantity);
            return Ok();
        }
    }
}
