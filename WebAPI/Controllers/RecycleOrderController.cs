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
using System.Web;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecycleOrderController : Controller
    {
        [HttpGet("{id}")]
        public JsonObject GetOrder(string id)
        {
            List<Recycle_Order> orders = RecycleOrderServer.QueryByAttribute("userid", "\'" + id + "\'");
            JsonObject ret = new JsonObject();
            ret.Add("num", orders.Count);
            ret.Add("recycle_order", JsonObject.Parse(JsonSerializer.Serialize(orders)));
            return ret;
        }

        [HttpGet("{id}/{orderid}")]
        public JsonObject GetSingleOrder(string orderid)
        {
            List<Recycle_Order> orders = RecycleOrderServer.Query(orderid);
            JsonObject ret = new JsonObject();
            ret.Add("recycle_order", JsonObject.Parse(JsonSerializer.Serialize(orders)));
            return ret;
        }

        [HttpPost("{id}")]
        public async Task<JsonObject> CreateOrder(string id, [FromBody]JsonObject Job)
        {
            JsonObject ret = new JsonObject();
            if (Request != null && UserServer.Query(id).Count!=0 
                && Job.ContainsKey("Device_Cate") && Job.ContainsKey("Device_Type") 
                && Job.ContainsKey("ExpectedPrice") && Job.ContainsKey("Recycle_Location") && Job.ContainsKey("Recycle_Time"))
            {
                //JsonObject Job = (JsonObject)(JsonObject.Parse(await stream.ReadToEndAsync()));
                Job.Add("UserID", id);
                Job.Add("OrderID", (RecycleOrderServer.Count()+10000).ToString());
                Recycle_Order order = JsonSerializer.Deserialize<Recycle_Order>(Job);
                Device device = new Device();
                device.DeviceID = DeviceServer.Count().ToString();
                device.Device_Cate_ID = DeviceCateServer.QueryByAttribute("category_name", "\'" + Job["Device_Cate"].ToString() + "\'").FirstOrDefault();
                device.Device_Type_ID = DeviceTypeServer.QueryByAttribute("type_name", "\'" + Job["Device_Type"].ToString() + "\'").FirstOrDefault();
                int row = DeviceServer.Insert(device);
                if (row>=0)
                {
                    order.Device = device;
                    row = RecycleOrderServer.Insert(order);
                }
                
                if (row >= 0)
                {
                    ret.Add("success", true);
                }
                
                else
                {
                    ret.Add("success", false);
                }
                
            }
            else
            {
                ret.Add("success", false);
            }
            return ret;
        }

        [HttpDelete("{id}/{orderid}")]
        public JsonObject RemoveOrder(string orderid)
        {
            int row = RecycleOrderServer.Delete(orderid);
            JsonObject ret = new JsonObject();
            if(row>0)
            {
                ret.Add("success", true);
            }
            else
            {
                ret.Add("success", false);
            }
            return ret;
        }

        [HttpPost("{id}/{orderid}")]
        public JsonObject ModifyOrder(string id , string orderid, [FromBody]JsonObject Job)
        {
            JsonObject ret = new JsonObject();
            if (Request != null && UserServer.Query(id).Count != 0 && RecycleOrderServer.Query(orderid).Count != 0
                && Job.ContainsKey("ExpectedPrice") && Job.ContainsKey("Recycle_Location") && Job.ContainsKey("Recycle_Time"))
            {
                //Job = (JsonObject)(JsonObject.Parse(await stream.ReadToEndAsync()));
                Job.Add("UserID", id);
                Job.Add("OrderID", orderid);
                Recycle_Order order= RecycleOrderServer.Query(orderid).FirstOrDefault();
                order.ExpectedPrice = (float)Job["ExpectedPrice"];
                order.Recycle_Location = Job["Recycle_Location"].ToString();
                order.Recycle_Time = (DateTime)Job["Recycle_Time"];
                int row = RecycleOrderServer.Update(order, orderid);

                if (row > 0)
                {
                    ret.Add("success", true);
                }
                else
                {
                    ret.Add("success", false);
                }
            }
            return ret;
        }
    }
}

