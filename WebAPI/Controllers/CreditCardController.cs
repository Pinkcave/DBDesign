using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CreditCardController : Controller
    {
        [HttpGet("{uid}")]
        public JsonObject GetCreditCards(string uid)
        {
            JsonObject ret = new JsonObject();
            if(UserServer.Query(uid).Count<=0)
            {
                ret.Add("Message", "找不到该用户");
                return ret;
            }

            List<CreditCard> cards = CreditCardServer.QueryByAttribute("UserID", "\'" + uid + "\'");
            ret.Add("num",cards.Count);
            ret.Add("cards", JsonObject.Parse(JsonSerializer.Serialize(cards)));
            return ret;
        }

        [HttpPost("{uid}")]
        public JsonObject NewCard(string uid, [FromBody]JsonObject Job)
        {
            JsonObject ret = new JsonObject();
            if (!Job.ContainsKey("CardID")||!Job.ContainsKey("Bank"))
            {
                ret.Add("success", false);
                ret.Add("Message", "缺少数据");
                return ret;
            }

            if(UserServer.Query(uid).Count<=0)
            {
                ret.Add("success", false);
                ret.Add("Message", "用户不存在");
                return ret;
            }

            Job.Add("UserID", uid);
            CreditCard newcard = JsonSerializer.Deserialize<CreditCard>(Job);
            newcard.Balance = 10000;
            if(CreditCardServer.QueryByAttribute("userid","\'"+uid+"\'").Count<=0) 
            {
                newcard.IsDefault = 1;
            }
            else
            {
                newcard.IsDefault=0;
            }
            int row = CreditCardServer.Insert(newcard);
            if(row > 0)
            {
                ret.Add("success", true);
            }
            else 
            { 
                ret.Add("success", false);
                ret.Add("Message", "插入失败");
            }
            return ret;
        }

        [HttpGet("DefaultCard/{uid}")]
        public JsonObject GetDefaultCard(string uid) 
        {
            JsonObject ret = new JsonObject();
            CreditCard card = CreditCardServer.GetDefaultCard(uid);
            ret.Add("userid", uid);
            ret.Add("default",JsonObject.Parse(JsonSerializer.Serialize(card)));
            return ret;
        }

        [HttpPost("DefaultCard/{uid}")]
        public JsonObject DefaultCard(string uid, [FromQuery]string cardid)
        {
            JsonObject ret = new JsonObject();
            if(UserServer.Query(uid).Count<=0)
            {
                ret.Add("success", false);
                ret.Add("Message", "用户不存在");
                return ret;
            }

            if(!CreditCardServer.ChangeDefaultCard(uid, cardid))
            {
                ret.Add("success", false);
                return ret;
            }

            ret.Add("success", true);
            return ret;
        }
    }
}
