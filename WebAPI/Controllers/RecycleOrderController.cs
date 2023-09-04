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
using System.Security.Cryptography;

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
            bool right = true;
            if (Request == null)
            {
                if (right)
                {
                    ret.Add("success", false);
                    right = false;
                }
                ret.Add("RequestError", "null request");
            }
            if (UserServer.Query(id).Count == 0)
            {
                if (right)
                {
                    ret.Add("success", false);
                    right = false;
                }
                ret.Add("UserError", "null user");
            }
            if (!Job.ContainsKey("Device_Cate"))
            {
                if (right)
                {
                    ret.Add("success", false);
                    right = false;
                }
                ret.Add("CateError", "缺少Device_Cate");
            }
            if (!Job.ContainsKey("Device_Type"))
            {
                if (right)
                {
                    ret.Add("success", false);
                    right = false;
                }
                ret.Add("TypeError", "缺少Device_Type");
            }
            if (!Job.ContainsKey("ExpectedPrice"))
            {
                if (right)
                {
                    ret.Add("success", false);
                    right = false;
                }
                ret.Add("PriceError", "缺少ExpectedPrice");
            }
            if (!Job.ContainsKey("Recycle_Location"))
            {
                if (right)
                {
                    ret.Add("success", false);
                    right = false;
                }
                ret.Add("LocError", "缺少Recycle_Location");
            }
            if (!Job.ContainsKey("Recycle_Time"))
            {
                if (right)
                {
                    ret.Add("success", false);
                    right = false;
                }
                ret.Add("TimeError", "缺少Recycle_Time");
            }
            if (!Job.ContainsKey("CustomerLocation"))
            {
                if (right)
                {
                    ret.Add("success", false);
                    right = false;
                }
                ret.Add("CustomerLocationError", "缺少CustomerLocation");
            }
            if (!Job.ContainsKey("PurchaseChannel"))
            {
                if (right)
                {
                    ret.Add("success", false);
                    right = false;
                }
                ret.Add("PurchaseChannelError", "缺少PurchaseChannel");
            }
            if (!Job.ContainsKey("StorageCapacity"))
            {
                if (right)
                {
                    ret.Add("success", false);
                    right = false;
                }
                ret.Add("StorageCapacityError", "缺少StorageCapacity");

            }
            if (right)
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
                device.PurchaseChannel = Job["PurchaseChannel"].ToString();
                device.StorageCapacity = Job["StorageCapacity"].ToString();
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
                    NewsSystemServer.AddNews(id,"订单创建成功",string.Format("回收订单{0}成功创建，可至订单页面查看",order.OrderID));
                    ret.Add("success", true);
                    ret.Add("orderid", order.OrderID);
                }

                else
                {
                    ret.Add("success", false);
                    ret.Add("Message", "DB未能成功插入");
                }

            }
            else
            {
                // ret.Add("success", false);
                ret.Add("Message", "缺少数据");
            }
            return ret;
        }

        [HttpDelete("{id}/{orderid}")]
        public JsonObject RemoveOrder(string id ,string orderid)
        {
            
            JsonObject ret = new JsonObject();
            Recycle_Order order = RecycleOrderServer.Query(orderid).FirstOrDefault();

            if(order == null)
            {
                ret.Add("success", false);
                ret.Add("Message", "不存在订单");
                return ret;
            }

            if(order.UserID != id) 
            {
                ret.Add("success", false);
                ret.Add("Message", "该用户不存在该订单");
                return ret;
            }

            if (RecycleOrderServer.Delete(orderid) <= 0)
            {
                ret.Add("success", false);
                return ret;
            }

            if (DeviceServer.Delete(order.Device.DeviceID) <= 0)
            {
                ret.Add("success", false);
                return ret;
            }

            //delete files
            if (order.Images != null)
            {
                foreach (var url in order.Images)
                {
                    FileHelper.DeleteFile(url.Replace("http://110.42.220.245:8081", "wwwroot"));
                }
            }
            NewsSystemServer.AddNews(id, "订单删除成功", string.Format("回收订单{0}成功删除", order.OrderID));
            ret.Add("success", true);
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
                && Job.ContainsKey("ExpectedPrice") && Job.ContainsKey("Recycle_Location") && Job.ContainsKey("Recycle_Time") 
                && Job.ContainsKey("CustomerLocation"))
            {
                //Job = (JsonObject)(JsonObject.Parse(await stream.ReadToEndAsync()));
                Job.Add("UserID", id);
                Job.Add("OrderID", orderid);
                Recycle_Order order = RecycleOrderServer.Query(orderid).FirstOrDefault();
                order.ExpectedPrice = (float)Job["ExpectedPrice"];
                order.Recycle_Location = Job["Recycle_Location"].ToString();
                order.Recycle_Time = (DateTime)Job["Recycle_Time"];
                order.CustomerLocation = Job["CustomerLocation"].ToString();
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
                    NewsSystemServer.AddNews(id, "订单修改成功", string.Format("回收订单{0}成功修改，可至回收页面查看", order.OrderID));
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

