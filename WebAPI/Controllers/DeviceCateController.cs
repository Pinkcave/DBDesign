using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeviceCateController : Controller
    {
        [HttpGet]
        public JsonObject GetAll()
        {
            List<Device_Cate> cates = DeviceCateServer.Query();
            JsonObject ret = new JsonObject();
            ret.Add("DeviceCate", JsonObject.Parse(JsonSerializer.Serialize(cates)));
            return ret;
        }

        [HttpGet("{name}")]
        public JsonObject GetByName(string name)
        {
            List<Device_Cate> cates = DeviceCateServer.QueryByAttribute("CategoryName", "\'" + name + "\'");
            JsonObject ret = new JsonObject();
            ret.Add("Name", name);
            ret.Add("DeviceCate", JsonObject.Parse(JsonSerializer.Serialize(cates)));
            return ret;
        }
    }
}
