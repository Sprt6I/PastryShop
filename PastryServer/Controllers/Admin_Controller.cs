using Microsoft.AspNetCore.Mvc;
using PastryServer.Models;
using PastryServer.Services;

namespace PastryServer.Controllers
{
    [ApiController]
    [Route("Admin")]
    public class Admin_Controller : ControllerBase
    {
        private readonly Database_Service database;

        public Admin_Controller(Database_Service database)
        {
            this.database = database;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Admin admin)
        {
            bool response = await database.Admin_Login_(admin);
            if (!response) { return BadRequest("Login or Password isn\'t valid"); }

            return Ok();
        }
    }
}