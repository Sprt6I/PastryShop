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

        [HttpGet("GetAllProducts")]
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

        [HttpPost("AddProduct")]
        public async Task<IActionResult> Add_Product_([FromBody] Product product)
        {
            (int, string) response = await database.Add_Product_(product);

            if (response.Item1 != 0) { return BadRequest(response.Item2); }

            return Ok();
        }

        [HttpGet("GetAllProductCategories")]
        public async Task<ActionResult<List<Product_Category>>> Get_All_Product_Categories_() {
            var product_categories = await database.Get_All_Product_Categories_();

            return Ok(product_categories);
        }

    }
}
