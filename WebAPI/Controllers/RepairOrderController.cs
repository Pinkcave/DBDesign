using Microsoft.AspNetCore.Mvc;
using Repair.Helper;
using Repair.Models;
using Repair.Server;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RepairOrderController : Controller
    {
        [HttpGet("{uid}")]
        public JsonObject GetOrder(string uid)
        {
            List<Repair_Order> orders = RepairOrderServer.QueryByAttribute("userid", "\'" + uid + "\'");
            JsonObject ret = new JsonObject();
            ret.Add("num", orders.Count);
            ret.Add("repair_order", JsonObject.Parse(JsonSerializer.Serialize(orders)));
            return ret;
        }

        [HttpGet("{uid}/{id}")]
        public JsonObject GetSingleOrder(string uid, string id)
        {
            List<Repair_Order> orders = RepairOrderServer.Query(id);
            JsonObject ret = new JsonObject();
            ret.Add("repair_order", JsonObject.Parse(JsonSerializer.Serialize(orders)));
            return ret;
        }

        /*
        [HttpPost("{uid}")]
        public JsonObject CreateOrder(string uid, [FromBody] JsonObject Job)
        {
            JsonObject ret = new JsonObject();
            if (Request.Body == null || !Job.ContainsKey("CouponID") || !Job.ContainsKey("EngineerID") 
                || !Job.ContainsKey("OrderPrice")|| !Job.ContainsKey("ProblemPart")|| !Job.ContainsKey("ProblemDetail")
                || !Job.ContainsKey("Requirement")||!Job.ContainsKey("Brand") 
                || !Job.ContainsKey("RepairLocation") || !Job.ContainsKey("RepairTime"))
            {
                ret.Add("success", false);
                ret.Add("Message", "缺少数据");
                return ret;
            }

            if (UserServer.Query(uid).Count != 0 && EngineerServer.Query(Job["EngineerID"].ToString()).Count != 0)
            {
                //查看用户余额
                using (BalanceController balanceController = new BalanceController())
                {
                    ret = balanceController.Pay(uid, Job["OrderPrice"].Deserialize<float>());
                    if (!ret["success"].Deserialize<bool>())
                        return ret;
                    else
                        ret.Clear();
                }

                //create repair_cate
                Repair_Cate cate = new Repair_Cate();
                cate.ID = RepairCateServer.Count().ToString();
                cate.Name = Job["ProblemPart"].ToString();
                cate.Detail = Job["ProblemDetail"].ToString();
                cate.Image = "null";
                if(RepairCateServer.Insert(cate)<=0)
                {
                    ret.Add("success", false);
                    return ret;
                }

                //create repair_option
                Repair_Options opt = new Repair_Options();
                opt.OptionID = RepairOptionServer.Count().ToString();
                opt.RepairCategory = cate;
                opt.RepairRequirement = Job["Requirement"].ToString();
                opt.Brand = Job["Brand"].ToString();
                if (RepairOptionServer.Insert(opt) <= 0)
                {
                    RepairCateServer.Delete(cate.ID);
                    ret.Add("success", false);
                    return ret;
                }

                //create order
                Job.Add("UserID", uid);
                Job.Add("OrderID", (RepairOrderServer.Count() + 10000).ToString());
                Job.Add("OrderStatus", 0);
                Repair_Order neworder = JsonSerializer.Deserialize<Repair_Order>(Job);
                if (Job["CouponID"] != null)
                {
                    neworder.CouObj = CouponServer.Query(Job["CouponID"].ToString()).FirstOrDefault();
                }
                    
                else
                {
                    neworder.CouObj = null;
                }
                neworder.RepairOptionID = opt;
                neworder.CreateTime = DateTime.Now;
                if(RepairOrderServer.Insert(neworder)<=0)
                {
                    RepairOptionServer.Delete(opt.OptionID);
                    RepairCateServer.Delete(cate.ID);
                    ret.Add("success", false);
                    return ret;
                }
                ret.Add("success", true);
            }
            return ret;
        }*/

        [HttpPost("{uid}")]
        public JsonObject CreateOrder(string uid, [FromForm] string Json)
        {
            JsonObject Job = (JsonObject)JsonObject.Parse(Json);
            JsonObject ret = new JsonObject();
            bool right = true;
            if(Request.Body == null)
            {
                right = false;
                ret.Add("RequestError", "null request body");
            }
            if (!Job.ContainsKey("CouponID"))
            {
                right = false;
                ret.Add("CouponIDError", "缺少CouponID");
            }
            if (!Job.ContainsKey("EngineerID"))
            {
                right = false;
                ret.Add("EngineerIDError", "缺少EngineerID");
            }
            if (!Job.ContainsKey("OrderPrice"))
            {
                right = false;
                ret.Add("OrderPriceError", "缺少OrderPrice");
            }
            if (!Job.ContainsKey("ProblemPart"))
            {
                right = false;
                ret.Add("ProblemPartError", "缺少ProblemPart");
            }
            if (!Job.ContainsKey("ProblemDetail"))
            {
                right = false;
                ret.Add("ProblemDetailError", "缺少ProblemDetail");
            }
            if (!Job.ContainsKey("Requirement"))
            {
                right = false;
                ret.Add("RequirementError", "缺少Requirement");
            }
            if (!Job.ContainsKey("Brand"))
            {
                right = false;
                ret.Add("BrandError", "缺少Brand");
            }
            if (!Job.ContainsKey("isWarranty"))
            {
                right = false;
                ret.Add("isWarrantyError", "缺少isWarranty");
            }
            if (!Job.ContainsKey("RepairLocation"))
            {
                right = false;
                ret.Add("RepairLocationError", "缺少RepairLocation");
            }
            if (!Job.ContainsKey("RepairTime"))
            {
                right = false;
                ret.Add("RepairTimeError", "缺少RepairTime");
            }
            if (!Job.ContainsKey("CustomerLocation"))
            {
                right = false;
                ret.Add("CustomerLocationError", "缺少CustomerLocation");
            }
            if (!Job.ContainsKey("Password"))
            {
                right = false;
                ret.Add("PasswordError", "缺少Password");
            }
            if (/*Request.Body == null || !Job.ContainsKey("CouponID") || !Job.ContainsKey("EngineerID")
                || !Job.ContainsKey("OrderPrice") || !Job.ContainsKey("ProblemPart") || !Job.ContainsKey("ProblemDetail")
                || !Job.ContainsKey("Requirement") || !Job.ContainsKey("Brand")||!Job.ContainsKey("isWarranty")
                || !Job.ContainsKey("RepairLocation") || !Job.ContainsKey("RepairTime") || !Job.ContainsKey("CustomerLocation")*/!right)
            {
                ret.Add("success", false);
                ret.Add("Message", "缺少数据");
                return ret;
            }

            if (UserServer.Query(uid).Count != 0 && EngineerServer.Query(Job["EngineerID"].ToString()).Count != 0)
            {
                //查看用户余额
                using (BalanceController balanceController = new BalanceController())
                {
                    ret = balanceController.Pay(uid, Job["OrderPrice"].Deserialize<float>(),Job);
                    if (!ret["success"].Deserialize<bool>())
                        return ret;
                    else
                        ret.Clear();
                }

                //create repair_cate
                Repair_Cate cate = new Repair_Cate();
                cate.ID = RepairCateServer.Count().ToString();
                cate.Name = Job["ProblemPart"].ToString();
                cate.Detail = Job["ProblemDetail"].ToString();
                cate.Image = "null";
                if (Job.ContainsKey("problemDescription") && Job["problemDescription"]!=null)
                {
                    cate.Description = Job["problemDescription"].ToString();
                }
                else
                {
                    cate.Description = null;
                }
                
                //create repair_option
                Repair_Options opt = new Repair_Options();
                opt.OptionID = RepairOptionServer.Count().ToString();
                opt.RepairCategory = cate;
                opt.RepairRequirement = Job["Requirement"].ToString();
                opt.Brand = Job["Brand"].ToString();
                opt.isWarranty = Job["isWarranty"].ToString();

                //create order
                Job.Add("UserID", uid);
                Job.Add("OrderID", (RepairOrderServer.Count() + 10000).ToString());
                Job.Add("OrderStatus", 0);
                Repair_Order neworder = JsonSerializer.Deserialize<Repair_Order>(Job);
                if (Job["CouponID"] != null)
                {
                    neworder.CouObj = CouponServer.Query(Job["CouponID"].ToString()).FirstOrDefault();
                }

                else
                {
                    neworder.CouObj = null;
                }
                neworder.RepairOptionID = opt;
                neworder.CreateTime = DateTime.Now;

                //load files
                string rootpath = "wwwroot/RepairImage";
                var files = Request.Form.Files;
                for(int i=0;i<files.Count;i++)
                {
                    string filename = string.Format("{0}({1}).{2}", neworder.OrderID, i, files[i].FileName.Split('.')[1]);
                    FileHelper.SaveFile(files[i].OpenReadStream(), rootpath, filename);
                    neworder.Images.Add(rootpath.Replace("wwwroot", "http://110.42.220.245:8081") + '/' + filename);
                }
                
                if (RepairCateServer.Insert(cate) <= 0)
                {
                    ret.Add("success", false);
                    return ret;
                }

                if (RepairOptionServer.Insert(opt) <= 0)
                {
                    RepairCateServer.Delete(cate.ID);
                    ret.Add("success", false);
                    return ret;
                }

                if (RepairOrderServer.Insert(neworder) <= 0)
                {
                    RepairOptionServer.Delete(opt.OptionID);
                    RepairCateServer.Delete(cate.ID);
                    ret.Add("success", false);
                    return ret;
                }
                ret.Add("success", true);
                ret.Add("orderid", neworder.OrderID);
                NewsSystemServer.AddNews(uid, "订单创建成功", string.Format("维修订单{0}成功创建，可至维修页面查看", neworder.OrderID));
            }
            return ret;
        }

        [HttpDelete("{uid}/{id}")]
        public JsonObject DeleteOrder(string uid, string id)
        {
            JsonObject ret = new JsonObject();
            Repair_Order order = RepairOrderServer.Query(id).FirstOrDefault();
            if (order == null)
            {
                ret.Add("success", false);
                ret.Add("Message", "订单不存在");
                return ret;
            }

            if(RepairOrderServer.Delete(id)<=0)
            {
                ret.Add("success", false);
                return ret;
            }
            
            if(RepairOptionServer.Delete(order.RepairOptionID.OptionID)<=0)
            {
                ret.Add("success", false);
                return ret;
            }

            if(RepairCateServer.Delete(order.RepairOptionID.RepairCategory.ID) <= 0)
            {
                ret.Add("success", false);
                return ret;
            }

            if(order.Images != null)
            {
                foreach (var url in order.Images)
                {
                    FileHelper.DeleteFile(url.Replace("http://110.42.220.245:8081", "wwwroot"));
                }
            }

            NewsSystemServer.AddNews(uid, "订单删除成功", string.Format("维修订单{0}成功删除", order.OrderID));
            ret.Add("success", true);
            return ret;
        }
        /*
        [HttpPost("{uid}/{id}")]
        public JsonObject UpdateOrder(string uid, string id, [FromBody] JsonObject Job)
        {
            JsonObject ret = new JsonObject();
            if (Request.Body == null || !Job.ContainsKey("EngineerID")|| !Job.ContainsKey("Requirement") 
                || !Job.ContainsKey("RepairLocation") || !Job.ContainsKey("RepairTime"))
            {
                ret.Add("success", false);
                ret.Add("Message", "缺少数据");
                return ret;
            }

            
            if (UserServer.Query(uid).Count != 0 && EngineerServer.Query(Job["EngineerID"].ToString()).Count != 0)
            {
                Repair_Order order = RepairOrderServer.Query(id).FirstOrDefault();
                if (order == null)
                {
                    ret.Add("success", false);
                    ret.Add("Message", "订单不存在");
                    return ret;
                }
                order.EngineerID = Job["EngineerID"].ToString();
                order.RepairOptionID.RepairRequirement = Job["Requirement"].ToString();
                order.RepairLocation = Job["RepairLocation"].ToString();
                order.RepairTime = Job["RepairTime"].Deserialize<DateTime>();
                if (RepairOptionServer.Update(order.RepairOptionID, order.RepairOptionID.OptionID) > 0)
                {
                    if (RepairOrderServer.Update(order, id) > 0)
                    {
                        ret.Add("success", true);
                        return ret;
                    }
                }
                ret.Add("success", false);
                return ret;
            }
            else
            {
                ret.Add("success", false);
                ret.Add("Message", "用户或工程师不存在");
                return ret;
            }
        }*/

        [HttpPost("{uid}/{id}")]
        public JsonObject UpdateOrder(string uid, string id, [FromForm] string Json)
        {
            JsonObject Job = (JsonObject)JsonObject.Parse(Json);
            JsonObject ret = new JsonObject();
            if (Request.Body == null || !Job.ContainsKey("EngineerID") || !Job.ContainsKey("Requirement")
                || !Job.ContainsKey("RepairLocation") || !Job.ContainsKey("RepairTime") || !Job.ContainsKey("CustomerLocation"))
            {
                ret.Add("success", false);
                ret.Add("Message", "缺少数据");
                return ret;
            }


            if (UserServer.Query(uid).Count != 0 && EngineerServer.Query(Job["EngineerID"].ToString()).Count != 0)
            {
                Repair_Order order = RepairOrderServer.Query(id).FirstOrDefault();
                if (order == null)
                {
                    ret.Add("success", false);
                    ret.Add("Message", "订单不存在");
                    return ret;
                }
                order.EngineerID = Job["EngineerID"].ToString();
                order.RepairOptionID.RepairRequirement = Job["Requirement"].ToString();
                order.RepairLocation = Job["RepairLocation"].ToString();
                order.RepairTime = Job["RepairTime"].Deserialize<DateTime>();
                order.CustomerLocation = Job["CustomerLocation"].ToString();
                
                //Update Images
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
                    string rootpath = "wwwroot/RepairImage";
                    for (int i = 0; i < files.Count; i++)
                    {
                        string filename = string.Format("{0}({1}).{2}", order.OrderID, i, files[i].FileName.Split('.')[1]);
                        FileHelper.SaveFile(files[i].OpenReadStream(), rootpath, filename);
                        order.Images.Add(rootpath.Replace("wwwroot", "http://110.42.220.245:8081") + '/' + filename);
                    }
                }             
                
                if (RepairOptionServer.Update(order.RepairOptionID, order.RepairOptionID.OptionID) > 0)
                {
                    if (RepairOrderServer.Update(order, id) > 0)
                    {
                        NewsSystemServer.AddNews(uid, "订单修改成功", string.Format("维修订单{0}成功修改，可至维修页面查看", order.OrderID));
                        ret.Add("success", true);
                        return ret;
                    }
                }
                ret.Add("success", false);
                return ret;
            }
            else
            {
                ret.Add("success", false);
                ret.Add("Message", "用户或工程师不存在");
                return ret;
            }
        }

        [HttpPost("Rate/{id}/{rate}")]
        public JsonObject RateOrder(string id,string rate)
        {
            Repair_Order order = RepairOrderServer.Query(id).FirstOrDefault();
            int row = 0;
            if (order != null)
            {
                order.UserRate = rate;
                row = RepairOrderServer.Update(order,id);
            }
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

        [HttpPost("Finish/{id}")]
        public JsonObject FinishOrder(string id)
        {
            Repair_Order order = RepairOrderServer.Query(id).FirstOrDefault();
            int row = 0;
            if(order != null)
            {
                order.OrderStatus = 1;
                row = RepairOrderServer.Update(order, id);
            }

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

        [HttpPost("Price/{id}/{price}")]
        public JsonObject PriceOrder(string id, float price)
        {
            Repair_Order order = RepairOrderServer.Query(id).FirstOrDefault();
            int row = 0;
            if (order != null)
            {
                order.OrderPrice = price;
                row = RepairOrderServer.Update(order,id);
            }
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
    }
}
