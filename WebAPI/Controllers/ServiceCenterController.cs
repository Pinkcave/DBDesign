using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServiceCenterController : Controller
    {
        [HttpGet]
        public JsonObject GetAll()
        {
            List<Service_Center> centers = ServiceCenterServer.Query();
            JsonObject ret = new JsonObject();
            ret.Add("ServiceCenter", JsonObject.Parse(JsonSerializer.Serialize(centers)));
            return ret;
        }

        [HttpGet("{location}")]
        public JsonObject GetByLoc(string location)
        {
            List<Service_Center> centers = ServiceCenterServer.QueryByAttribute("Loc_detail", "\'" + location + "\'");
            JsonObject ret = new JsonObject();
            ret.Add("Location", location);
            ret.Add("DeviceType", JsonObject.Parse(JsonSerializer.Serialize(centers)));
            return ret;
        }
    }
}
