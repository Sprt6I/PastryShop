using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using PastryServer.Models;
using PastryServer.Services;
using PastryServer.Helper_Files;
using Org.BouncyCastle.Crypto.Macs;

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
        //private readonly Database_Service_External database_external;

        public AuthController(Gmail_Sender gmailSender, Database_Service databaseService /*Database_Service_External database_external_*/, IConfiguration config)
        {
            gmail_login = config["Gmail:Login"];
            gmail_password = config["Gmail:Password"];
            gmail_sender = gmailSender;
            database = databaseService;
            //database_external = database_external_;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register_([FromBody] Register_Request request)
        {
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

            string password_hash = BCrypt.Net.BCrypt.HashPassword(password);

            await database.Add_User_(gmail, password_hash);

            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login_([FromBody] Login_Request request)
        {
            string gmail = request.gmail;
            if (string.IsNullOrWhiteSpace(gmail)) { Console.WriteLine("[MAIN]: no gmail"); return BadRequest("Email required"); }
            if (!Checks.Is_Gmail_Valid_(gmail)) { Console.WriteLine("[MAIN]: gmail isnt valid"); return BadRequest("Email isn\'t valid"); }

            string password = request.password;
            if (string.IsNullOrWhiteSpace(password)) { Console.WriteLine("[MAIN]: no password"); return BadRequest("Password required"); }
            if (!Checks.Is_Password_Valid_(password)) { Console.WriteLine("[MAIN]: password isnt valid"); return BadRequest("Password isn\'t valid"); }

            //string password_hash = BCrypt.Net.BCrypt.HashPassword(password);

            bool password_is_correct = await database.Check_Password_By_Gmail_(gmail, password);
            if (!password_is_correct) { return Unauthorized("User doen\'t exist or password is invalid"); }

            return Ok();
        }

        [HttpPost("GetUserIdByGmail")]
        public async Task<ActionResult<int>> Get_User_Id__By_Gmail_([FromBody] GmailRequest req)
        {
            int user_id = await database.Get_User_Id_By_Gmail(req.gmail);
            if (user_id == -1) { Console.WriteLine("Something went really wrong in login, user doens't exist"); return Unauthorized("User doenst exists or soemthing is really wrong"); }

            return Ok(user_id);
        }
        
        [HttpPost("SentVerificationGmail")]
        public async Task<IActionResult> Sent_Verification_Gmail([FromBody] Gmail_Request request)
        {
            string gmail = request.gmail;
            if (string.IsNullOrWhiteSpace(gmail)) { Console.WriteLine("[MAIN]: no gmail"); return BadRequest("Email required"); }
            if (!Checks.Is_Gmail_Valid_(gmail)) { Console.WriteLine("[MAIN]: gmail isnt valid"); return BadRequest("Email isn\'t valid"); }

            string code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            await database.Store_Verification_Code_(gmail, code, TimeSpan.FromMinutes(5));
            await gmail_sender.Send_Email_Async(gmail_password, gmail_login, gmail, "Verification code", code);

            Console.WriteLine("[MAIN]: code sent");

            return Ok();
        }

        [HttpGet("GetCart")]
        public async Task<ActionResult<User_Cart>> Get_User_Cart([FromBody] User user)
        {
            return await database.Get_User_Cart_(user);
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<List<User_Order>>> Get_User_Orders([FromBody] User user)
        {
            return await database.Get_User_Orders(user);
        }
    }
}
