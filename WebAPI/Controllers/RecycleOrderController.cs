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
        public async Task<JsonObject> CreateOrder(string id)
        {
            JsonObject ret = new JsonObject();
            if (Request != null)
            {
                using (StreamReader stream = new StreamReader(Request.Body))
                {
                    JsonObject Job = (JsonObject)(JsonObject.Parse(await stream.ReadToEndAsync()));
                    Job.Add("UserID", id);
                    Job.Add("OrderID", RecycleOrderServer.Count());
                    Job.Add("Recycle_Time", DateTime.Now);
                    Recycle_Order order = JsonSerializer.Deserialize<Recycle_Order>(Job);
                    Device device = new Device();
                    device.DeviceID = DeviceServer.Count().ToString();
                    device.Device_Cate_ID = DeviceCateServer.QueryByAttribute("categoryname", "\'" + Job["device_cate"].ToString() + "\'").FirstOrDefault();
                    device.Device_Type_ID = DeviceTypeServer.QueryByAttribute("typename", "\'" + Job["device_type"].ToString() + "\'").FirstOrDefault();
                    DeviceServer.Insert(device);
                    order.Device = device;
                    int row = RecycleOrderServer.Insert(order);

                    if (row >= 0)
                    {
                        ret.Add("success", true);
                    }

                    else
                    {
                        ret.Add("success", false);
                    }
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
        public async Task<JsonObject> ModifyOrder(string id , string orderid)
        {
            JsonObject ret = new JsonObject();
            if(Request == null)
            {
                ret.Add("success", false);
            }
            else
            {
                using (StreamReader stream = new StreamReader(Request.Body))
                {
                    JsonObject Job = (JsonObject)(JsonObject.Parse(await stream.ReadToEndAsync()));
                    Job.Add("userid", id);
                    Job.Add("orderid", orderid);
                    Recycle_Order order = JsonSerializer.Deserialize<Recycle_Order>(Job);
                    int row = RecycleOrderServer.Update(order, id);

                    if (row > 0)
                    {
                        ret.Add("success", true);
                    }
                    else
                    {
                        ret.Add("success", false);
                    }
                }
            }
            return ret;
        }
    }
}

