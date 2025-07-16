using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace WebApplication1.Controllers
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