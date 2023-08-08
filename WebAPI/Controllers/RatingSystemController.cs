using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RatingSystemController : Controller
    {
        [HttpGet("{eid}")]
        public JsonObject GetRating(string eid)
        {
            List<Rating_System> rates = RatingSystemServer.Query(null, eid);
            JsonObject ret = new JsonObject();
            ret.Add("Num", rates.Count);
            ret.Add("Rate",JsonObject.Parse(JsonSerializer.Serialize(rates)));
            return ret;
        }

        [HttpGet("{eid}/{uid}")]
        public JsonObject GetRatingDetail(string eid,string uid)
        {
            List<Rating_System> rates = RatingSystemServer.Query(uid, eid);
            JsonObject ret = new JsonObject();
            ret.Add("Num", rates.Count);
            ret.Add("Rate", JsonObject.Parse(JsonSerializer.Serialize(rates)));
            return ret;
        }

        [HttpPost]
        public JsonObject Rating(JsonObject Job)
        {
            Rating_System rate = JsonSerializer.Deserialize<Rating_System>(Job);
            int row = RatingSystemServer.Insert(rate);
            JsonObject ret = new JsonObject();
            if (row > 0)
            {
                ret.Add("success", true);
                ret.Add("rate", Job);
            }
            else
            {
                ret.Add("success", false);
            }
            return ret;
        }
    }
}
