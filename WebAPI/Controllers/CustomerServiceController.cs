using System.Text.Json.Nodes;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerServiceController : Controller
    {
        [HttpGet]
        public JsonObject GetCustomerService()
        {
            List<CustomerService> types = CustomerServiceServer.Query();
            JsonObject ret = new JsonObject();
            ret.Add("num", types.Count);
            ret.Add("customer_service", JsonObject.Parse(JsonSerializer.Serialize(types)));
            return ret;
        }
    }
}
