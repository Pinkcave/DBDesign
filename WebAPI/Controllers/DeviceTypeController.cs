using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeviceTypeController : Controller
    {
        [HttpGet]
        public JsonObject GetAll()
        {
            List<Device_Type> types = DeviceTypeServer.Query();
            JsonObject ret = new JsonObject();
            ret.Add("Num", types.Count);
            ret.Add("DeviceType", JsonObject.Parse(JsonSerializer.Serialize(types)));
            return ret;
        }

        [HttpGet("{name}")]
        public JsonObject GetByName(string name)
        {
            List<Device_Type> types = DeviceTypeServer.QueryByAttribute("Type_Name", "\'" + name + "\'");
            JsonObject ret = new JsonObject();
            ret.Add("Name", name);
            ret.Add("DeviceType", JsonObject.Parse(JsonSerializer.Serialize(types)));
            return ret;
        }
    }
}
