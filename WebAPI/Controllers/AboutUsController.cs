using System.Text.Json.Nodes;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AboutUsController : Controller
    {
        [HttpGet("CustomerService")]
        public IEnumerable<CustomerService> GetUserInfo()
        {
            List<CustomerService> customerServices = CustomerServiceServer.Query();
            //Console.WriteLine(uid);
            return customerServices;
        }
    }
}
