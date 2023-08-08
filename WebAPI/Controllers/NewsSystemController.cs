using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsSystemController : Controller
    {
        [HttpGet("{uid}")]
        public JsonObject GetAll(string uid)
        {
            List<News_System> list = NewsSystemServer.QueryByAttribute("userid", "\'" + uid + "\'");
            JsonObject ret = new JsonObject();
            ret.Add("Num", list.Count);
            ret.Add("News", JsonObject.Parse(JsonSerializer.Serialize(list)));
            return ret;
        }

        [HttpGet("{uid}/{id}")]
        public JsonObject GetSingle(string id)
        {
            List<News_System> list = NewsSystemServer.Query(id);
            JsonObject ret = new JsonObject();
            ret.Add("Num", list.Count);
            ret.Add("News", JsonObject.Parse(JsonSerializer.Serialize(list)));
            return ret;
        }
    }
}
