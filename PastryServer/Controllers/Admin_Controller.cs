using Microsoft.AspNetCore.Http;
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
    }
}