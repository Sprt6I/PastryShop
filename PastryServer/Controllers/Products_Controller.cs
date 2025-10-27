using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PastryServer.Models;
using PastryServer.Services;

namespace PastryServer.Controllers
{
    [ApiController]
    [Route("Products")]
    public class Products_Controller : ControllerBase
    {
        private readonly Database_Service database;

        public Products_Controller(Database_Service database)
        {
            this.database = database;
        }

        [HttpPost("GetAllProducts")]
        public async Task<ActionResult<List<Product>>> Get_All_Products_()
        {
            var products = await database.Get_All_Products_();
            return Ok(products);
        }

        [HttpPost("UpdateProducts")]
        public async Task<IActionResult> Update_Products_([FromBody] Product product)
        {
            await database.Update_Products_(product);
            return Ok();
        }
    }
}
