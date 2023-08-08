using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text.Json;
using DBDesign;
using System;
using System.Drawing;
using System.Buffers.Text;
using Repair.Models;
using Repair.Server;
using System.Text.Json.Nodes;

namespace WebAPI.Controllers
{
   
    [Route("[controller]")]
    [ApiController]
    public class DeviceEvalController : Controller
    {
        [HttpGet("FrontPage")]
        public JsonObject GetEvalSamples()
        {
            List<Device_Evaluation> device_Eval = DeviceEvalServer.Query();
            JsonObject ret = new JsonObject();
            ret.Add("samples",JsonObject.Parse(JsonSerializer.Serialize(device_Eval)));
            return ret;
        }

        [HttpGet("{id}")]
        public JsonObject GetDeviceEval(string id)
        {
            List<Device_Evaluation> device_Eval = DeviceEvalServer.Query(id);
            JsonObject ret = new JsonObject();
            ret.Add("samples", JsonObject.Parse(JsonSerializer.Serialize(device_Eval)));
            return ret;
        }

        [HttpPost]
        public JsonObject AddEvaluation([FromBody] JsonObject Job)
        {
            int row = DeviceEvalServer.Insert(Job.ToJsonString());
            JsonObject ret  = new JsonObject();

            if(row!= -1)
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
