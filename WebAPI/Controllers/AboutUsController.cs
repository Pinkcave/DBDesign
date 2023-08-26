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
        [HttpGet]
        public JsonObject GetAboutUs()
        {
            List<AboutUs> types = AboutUsServer.Query();
            JsonObject ret = new JsonObject();
            ret.Add("num", types.Count);
            ret.Add("about_us", JsonObject.Parse(JsonSerializer.Serialize(types)));
            return ret;
        }
    }
}
