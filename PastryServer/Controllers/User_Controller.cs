using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using PastryServer.Models;
using PastryServer.Services;

namespace PastryServer.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly Gmail_Sender gmail_sender;
        private readonly string gmail_login = "darek26655@gmail.com";
        private readonly string gmail_password = "gdljrbvjzhjruluo"; // gdlj rbvj zhjr uluo

        private readonly Database_Service database;

        public AuthController(Gmail_Sender gmailSender, Database_Service databaseService)
        {
            gmail_sender = gmailSender;
            database = databaseService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register_([FromBody] Register_Request request)
        {
            string gmail = request.gmail;
            if (string.IsNullOrWhiteSpace(gmail)) { Console.WriteLine("[MAIN]: no gmail"); return BadRequest("Email required"); }

            string password = request.password;
            if (string.IsNullOrWhiteSpace(password)) { Console.WriteLine("[MAIN]: no password"); return BadRequest("Password required"); }

            string verification_code = request.verification_code;
            if (string.IsNullOrWhiteSpace(verification_code)) { Console.WriteLine("[MAIN]: no code"); return BadRequest("Verification Code required"); }


            Console.WriteLine($"[MAIN]: register {gmail}");
            if (!await database.Verify_Verification_Code_(gmail, verification_code)) { Console.WriteLine("[MAIN]: wrong code");  return BadRequest("Wrong Verification Code"); }
            if (await database.Check_If_Gmail_Exists_(gmail)) { Console.WriteLine("[MAIN]: user exists"); return BadRequest("User Already Exists");  }

            await database.Add_User_(gmail, password);
            Console.WriteLine("[MAIN]: User registered");

            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login_([FromBody] Login_Request request)
        {
            string gmail = request.gmail;
            if (string.IsNullOrWhiteSpace(gmail)) { Console.WriteLine("[MAIN]: no gmail"); return BadRequest("Email required"); }

            string password = request.password;
            if (string.IsNullOrWhiteSpace(password)) { Console.WriteLine("[MAIN]: no password"); return BadRequest("Password required"); }

            bool password_is_correct = await database.Check_Password_By_Gmail_(gmail, password);
            if (!password_is_correct) { return Unauthorized("User doen\'t exist or password is invalid"); }

            return Ok();
        }
        
        [HttpPost("SentVerificationGmail")]
        public async Task<IActionResult> Sent_Verification_Gmail([FromBody] Gmail_Request request)
        {
            string gmail = request.gmail;
            if (string.IsNullOrWhiteSpace(gmail)) { Console.WriteLine("[MAIN]: no gmail"); return BadRequest("Email required"); }
            if (!Is_Gmail_Valid_(gmail)) { Console.WriteLine("[MAIN]: gmail isnt valid"); return BadRequest("Email isn\'t valid"); }

            string code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            await database.Store_Verification_Code_(gmail, code, TimeSpan.FromMinutes(5));
            await gmail_sender.Send_Email_Async(gmail_password, gmail_login, gmail, "Verification code", code);

            Console.WriteLine("[MAIN]: code sent");

            return Ok();
        }

        private bool Is_Gmail_Valid_(string gmail)
        {
            if (string.IsNullOrWhiteSpace(gmail)) { return false; }

            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            if (!Regex.IsMatch(gmail, pattern, RegexOptions.IgnoreCase)) { return false; }

            return true;
        }
    }
}
