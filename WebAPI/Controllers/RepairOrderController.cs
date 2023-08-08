using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("{uid}")]
        public JsonObject CreateOrder(string uid, [FromBody] JsonObject Job)
        {
            Job.Add("UserID", uid);
            Job.Add("OrderID", RepairOrderServer.Count());
            Job.Add("OrderStatus", false);
            Repair_Order neworder = JsonSerializer.Deserialize<Repair_Order>(Job);
            neworder.CouObj = CouponServer.Query(Job["CouponID"].ToString()).FirstOrDefault();
            neworder.RepairOptionID = RepairOptionServer.Query(Job["OptionID"].ToString()).FirstOrDefault();
            int row = RepairOrderServer.Insert(neworder);
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

        [HttpDelete("{uid}/{id}")]
        public JsonObject DeleteOrder(string uid, string id)
        {
            int row = RepairOrderServer.Delete(id);
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

        [HttpPost("{uid}/{id}")]
        public JsonObject UpdateOrder(string uid, string id, [FromBody] JsonObject Job)
        {
            Job.Add("UserID", uid);
            Job.Add("OrderID", id);
            Repair_Order neworder = JsonSerializer.Deserialize<Repair_Order>(Job);
            neworder.RepairOptionID = RepairOptionServer.Query(Job["OptionID"].ToString()).FirstOrDefault();
            int row = RepairOrderServer.Update(neworder,id);
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
