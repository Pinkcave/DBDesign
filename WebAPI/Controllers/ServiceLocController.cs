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
                Job.Add("ID", ServiceLocServer.Count().ToString());
                Job.Add("CustomerID", uid);
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
            JsonObject ret = new JsonObject();
            if(Job.ContainsKey("Location_Name") && Job.ContainsKey("Loc_Detail"))
            {
                Job.Add("ID", id);
                Job.Add("CustomerID", uid);
                Service_Loc loc = JsonSerializer.Deserialize<Service_Loc>(Job);
                int row = ServiceLocServer.Update(loc, id);

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

        [HttpDelete("{id}")]
        public JsonObject DeleteServiceLoc(string id)
        {
            JsonObject ret = new JsonObject();
            List<Service_Loc> loc = ServiceLocServer.Query(id);
            if (loc.Equals(null))
            {
                ret.Add("success", false);
                ret.Add("Message", "服务地址不存在");
                return ret;
            }

            if (ServiceLocServer.Delete(id) <= 0)
            {
                ret.Add("success", false);
            }
            else
            {
                ret.Add("success", true);
            }

            return ret;
        }
    }
}
