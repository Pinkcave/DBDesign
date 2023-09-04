using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;
using System.Text.Json.Nodes;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BalanceController : Controller
    {
        [HttpPost("Charge/{uid}")]
        public JsonObject Charge(string uid, [FromQuery]float num,[FromBody]JsonObject Job)
        {
            JsonObject ret = new JsonObject();
            UserInfo user = UserServer.Query(uid).FirstOrDefault();
            if(user==null)
            {
                ret.Add("success", false);
                ret.Add("Message", "用户不存在");
                return ret;
            }
            if (!Job.ContainsKey("Password") || user.Password != Job["Password"].ToString())
            {
                ret.Add("success", false);
                ret.Add("Message", "密码不正确");
                return ret;
            }

            CreditCard card = CreditCardServer.GetDefaultCard(uid);
            if(card==null)
            {
                ret.Add("success", false);
                ret.Add("Message", "未设置默认卡");
                return ret;
            }
            if(card.Balance<num)
            {
                ret.Add("success", false);
                ret.Add("Message", "余额不足");
                return ret;
            }

            user.Balance += num;
            card.Balance -= num;

            int row = UserServer.Update(user, user.UserId);
            if(row > 0)
            {
                row = CreditCardServer.Update(card, card.CardID);
                if(row > 0)
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
        [HttpPost("Withdrawal/{uid}")]
        public JsonObject WithDrawal(string uid, [FromQuery]float num, [FromBody]JsonObject Job) 
        {
            JsonObject ret = new JsonObject();
            UserInfo user = UserServer.Query(uid).FirstOrDefault();
            if (user == null)
            {
                ret.Add("success", false);
                ret.Add("Message", "用户不存在");
                return ret;
            }

            if (!Job.ContainsKey("Password") || user.Password != Job["Password"].ToString())
            {
                ret.Add("success", false);
                ret.Add("Message", "密码不正确");
                return ret;
            }

            CreditCard card = CreditCardServer.GetDefaultCard(uid);
            if (card == null)
            {
                ret.Add("success", false);
                ret.Add("Message", "未设置默认卡");
                return ret;
            }
            if (user.Balance < num)
            {
                ret.Add("success", false);
                ret.Add("Message", "余额不足");
                return ret;
            }

            user.Balance -= num;
            card.Balance += num;

            int row = UserServer.Update(user, user.UserId);
            if (row > 0)
            {
                row = CreditCardServer.Update(card, card.CardID);
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
            }
            return ret;
        }

        [HttpPost("Pay/{uid}")]
        public JsonObject Pay(string uid, [FromQuery]float num,[FromBody]JsonObject Job)
        {
            JsonObject ret = new JsonObject();
            UserInfo? user = UserServer.Query(uid).FirstOrDefault();
            if (user == null)
            {
                ret.Add("success", false);
                ret.Add("Message", "用户不存在");
                return ret;
            }

            if (!Job.ContainsKey("Password")||user.Password != Job["Password"].ToString())
            {
                ret.Add("success", false);
                ret.Add("Message", "密码不正确");
                return ret;
            }

            if (user.Balance < num)
            {
                ret.Add("success", false);
                ret.Add("Message", "余额不足");
                return ret;
            }

            user.Balance -= num;
            if(UserServer.Update(user, user.UserId)>0)
            {
                ret.Add("success", true);
            }
            else
            {
                ret.Add("success", false);
            }
            return ret;
        }

        [HttpPost("Income/{uid}")]
        public JsonObject Income(string uid, [FromQuery]float num)
        {
            JsonObject ret = new JsonObject();
            UserInfo? user = UserServer.Query(uid).FirstOrDefault();
            if (user == null)
            {
                ret.Add("success", false);
                ret.Add("Message", "用户不存在");
                return ret;
            }

            user.Balance += num;
            if (UserServer.Update(user, user.UserId) > 0)
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
