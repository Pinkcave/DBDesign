using Microsoft.AspNetCore.Mvc;
using Repair.Server;
using System.Text.Json.Nodes;
using Repair.Models;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeviceController : Controller
    {
        [HttpGet("Search")]
        public JsonObject SearchDevice([FromQuery] string keywords)
        {
            string[] keys = keywords.Split(" ");
            List<DeviceInfo> search = DeviceServer.Search(keys);
            JsonObject ret = new JsonObject();
            ret.Add("num", search.Count);
            ret.Add("devices", JsonObject.Parse(JsonSerializer.Serialize(search)));
            return ret;
        }

        [HttpGet("FrontPage")]
        public JsonObject GetSamples()
        {
            List<DeviceInfo> search = DeviceServer.SearchAll();
            JsonObject ret = new JsonObject();
            ret.Add("num", search.Count);
            ret.Add("devices", JsonObject.Parse(JsonSerializer.Serialize(search)));
            return ret;
        }

        [HttpGet("{id}")]
        public JsonObject GetSingleSample(string id)
        {
            DeviceInfo info = new DeviceInfo();
            info.device =  DeviceServer.Query(id).FirstOrDefault();
            info.eval = DeviceEvalServer.Query(id).FirstOrDefault();
            return (JsonObject)(JsonObject.Parse(JsonSerializer.Serialize(info)));
        }
    }
}
