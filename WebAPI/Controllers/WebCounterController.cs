using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebCounterController : Controller
    {
        [HttpGet]
        public WebCounter Get()
        {
            WebCounter web = WebCounterServer.Query();
            return web;
        }
    }
}
