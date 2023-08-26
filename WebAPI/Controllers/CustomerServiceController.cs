using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerServiceController : Controller
    {
        [HttpGet("AboutUs")]
        public IEnumerable<AboutUs> GetUserInfo()
        {
            List<AboutUs> aboutUs = AboutUsServer.Query();
            //Console.WriteLine(uid);
            return aboutUs;
        }
    }
}
