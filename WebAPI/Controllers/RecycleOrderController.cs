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
using Repair.Helper;

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
        /*
        [HttpPost("{id}")]
        public JsonObject CreateOrder(string id, [FromBody]JsonObject Job)
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
                ret.Add("Message", "缺少数据");
            }
            return ret;
        }*/

        [HttpPost("{id}")]
        public JsonObject CreateOrder(string id, [FromForm] string Json)
        {
            JsonObject Job = (JsonObject)JsonObject.Parse(Json);
            JsonObject ret = new JsonObject();
            if (Request != null && UserServer.Query(id).Count != 0
                && Job.ContainsKey("Device_Cate") && Job.ContainsKey("Device_Type")
                && Job.ContainsKey("ExpectedPrice") && Job.ContainsKey("Recycle_Location") && Job.ContainsKey("Recycle_Time"))
            {
                //JsonObject Job = (JsonObject)(JsonObject.Parse(await stream.ReadToEndAsync()));
                Job.Add("UserID", id);
                Job.Add("OrderID", (RecycleOrderServer.Count() + 10000).ToString());
                Recycle_Order order = JsonSerializer.Deserialize<Recycle_Order>(Job);
                //create device
                Device device = new Device();
                device.DeviceID = DeviceServer.Count().ToString();
                device.Device_Cate_ID = DeviceCateServer.QueryByAttribute("category_name", "\'" + Job["Device_Cate"].ToString() + "\'").FirstOrDefault();
                device.Device_Type_ID = DeviceTypeServer.QueryByAttribute("type_name", "\'" + Job["Device_Type"].ToString() + "\'").FirstOrDefault();
                //loadfile
                string rootpath = "wwwroot/RecycleImage";
                var files = Request.Form.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    string filename = string.Format("{0}({1}).{2}", order.OrderID, i, files[i].FileName.Split('.')[1]);
                    FileHelper.SaveFile(files[i].OpenReadStream(), rootpath, filename);
                    order.Images.Add(rootpath.Replace("wwwroot", "http://110.42.220.245:8081") + '/' + filename);
                }
                //Insert
                int row = DeviceServer.Insert(device);
                if (row >= 0)
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
                ret.Add("Message", "缺少数据");
            }
            return ret;
        }

        [HttpDelete("{id}/{orderid}")]
        public JsonObject RemoveOrder(string orderid)
        {
            
            JsonObject ret = new JsonObject();
            Recycle_Order order = RecycleOrderServer.Query(orderid).FirstOrDefault();

            if (RecycleOrderServer.Delete(orderid) > 0)
            {
                if (DeviceServer.Delete(order.Device.DeviceID) > 0)
                {
                    ret.Add("success", true);
                    return ret;
                }
                    
            }
            //delete files
            foreach (var url in order.Images)
            {
                FileHelper.DeleteFile(url.Replace("http://110.42.220.245:8081", "wwwroot"));
            }

            ret.Add("success", false);
            return ret;
        }
        /*
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
                Recycle_Order order = RecycleOrderServer.Query(orderid).FirstOrDefault();
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
            else
            {
                ret.Add("success", false);
                ret.Add("Message", "缺少数据");
            }
            return ret;
        }*/

        [HttpPost("{id}/{orderid}")]
        public JsonObject ModifyOrder(string id, string orderid, [FromForm] string Json)
        {
            JsonObject Job = (JsonObject)JsonObject.Parse(Json);
            JsonObject ret = new JsonObject();
            if (Request != null && UserServer.Query(id).Count != 0 && RecycleOrderServer.Query(orderid).Count != 0
                && Job.ContainsKey("ExpectedPrice") && Job.ContainsKey("Recycle_Location") && Job.ContainsKey("Recycle_Time"))
            {
                //Job = (JsonObject)(JsonObject.Parse(await stream.ReadToEndAsync()));
                Job.Add("UserID", id);
                Job.Add("OrderID", orderid);
                Recycle_Order order = RecycleOrderServer.Query(orderid).FirstOrDefault();
                order.ExpectedPrice = (float)Job["ExpectedPrice"];
                order.Recycle_Location = Job["Recycle_Location"].ToString();
                order.Recycle_Time = (DateTime)Job["Recycle_Time"];
                //Update Files
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    //delete files
                    if (order.Images != null)
                    {
                        foreach (var url in order.Images)
                        {
                            FileHelper.DeleteFile(url.Replace("http://110.42.220.245:8081", "wwwroot"));
                        }
                        order.Images.Clear();
                    }
                    else
                    {
                        order.Images = new List<string>();
                    }
                    //load files
                    string rootpath = "wwwroot/RecycleImage";
                    for (int i = 0; i < files.Count; i++)
                    {
                        string filename = string.Format("{0}({1}).{2}", order.OrderID, i, files[i].FileName.Split('.')[1]);
                        FileHelper.SaveFile(files[i].OpenReadStream(), rootpath, filename);
                        order.Images.Add(rootpath.Replace("wwwroot", "http://110.42.220.245:8081") + '/' + filename);
                    }
                }
                //Update DB
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
            else
            {
                ret.Add("success", false);
                ret.Add("Message", "缺少数据");
            }
            return ret;
        }
    }
}

