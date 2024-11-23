
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository;
using System.Net;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public SettingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("/setting")]
        public IActionResult Index()
        {
            var connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            return Ok(connectionString);
        }
    }
}