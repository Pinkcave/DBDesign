using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;
using System;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServiceLocController : Controller
    {
        [HttpGet("{uid}")]
        public JsonObject GetLocList(string uid)
        {
            List<Service_Loc> list = ServiceLocServer.QueryByAttribute("customerid", "\'" + uid + "\'");
            JsonObject ret = new JsonObject();
            ret.Add("userid", uid);
            ret.Add("Location", JsonObject.Parse(JsonSerializer.Serialize(list)));
            return ret;
        }

        [HttpPost("{uid}")]
        public JsonObject NewLocation(string uid,[FromBody] JsonObject Job)
        {
            JsonObject ret = new JsonObject();
            if (Job.ContainsKey("Location_Name") && Job.ContainsKey("Loc_Detail"))
            {
                Job.Add("id", ServiceLocServer.Count().ToString());
                Job.Add("customerid", uid);
                Service_Loc loc = JsonSerializer.Deserialize<Service_Loc>(Job);
                int row = ServiceLocServer.Insert(loc);
                if (row > 0)
                {
                    ret.Add("success", true);
                    ret.Add("Loc_Detail", Job);
                }
                else
                {
                    ret.Add("success", false);
                }
            }
            else
            {
                ret.Add("success", false);
                ret.Add("Message", "缺少数据");
            }
            return ret;
        }

        [HttpPost("{uid}/{id}")]
        public JsonObject ModifyLocation(string uid, string id, [FromBody] JsonObject Job)
        {
            Job.Add("id", Guid.NewGuid().ToString().Replace("-", "").ToUpper());
            Job.Add("customerid", uid);
            Service_Loc loc = JsonSerializer.Deserialize<Service_Loc>(Job);
            int row = ServiceLocServer.Update(loc, uid);
            JsonObject ret = new JsonObject();
            if (row > 0)
            {
                ret.Add("success", true);
                ret.Add("Loc_Detail", Job);
            }
            else
            {
                ret.Add("success", false);
            }
            return ret;
        }
    }
}
