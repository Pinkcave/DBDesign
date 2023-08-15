using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EngineerController : Controller
    {
        [HttpGet]
        public JsonObject GetAllEngineers()
        {
            JsonObject ret = new JsonObject();
            List<Engineer> engineers = EngineerServer.Query();
            ret.Add("num", engineers.Count);
            ret.Add("engineers", JsonObject.Parse(JsonSerializer.Serialize(engineers)));
            return ret;
        }

        [HttpGet("{id}")]
        public JsonObject GetEngineerInfo(string id)
        {
            List<Engineer> engineers = EngineerServer.Query(id);
            JsonObject ret = new JsonObject();
            ret.Add("engineerinfo", JsonObject.Parse(JsonSerializer.Serialize(engineers)));
            return ret;
        }

        [HttpPost("{id}")]
        public JsonObject UpdateEngineerInfo(string id,JsonObject Job)
        {
            Engineer engineer = JsonSerializer.Deserialize<Engineer>(Job);
            engineer.ID = id;
            int row = EngineerServer.Update(engineer, id);
            JsonObject ret = new JsonObject();
            if (row == 1)
            {
                ret.Add("success", true);
            }
                
            else
            {
                ret.Add("success", false);
            }
            return ret;
        }

        [HttpDelete("{id}")]
        public JsonObject DeleteEngineer(string id)
        {
            int row = EngineerServer.Delete(id);
            JsonObject ret = new JsonObject();
            if (row > 0)
            {
                ret.Add("success", true);
            }

            else
            {
                ret.Add("success", false);
            }
            return ret;
        }
    }
}
