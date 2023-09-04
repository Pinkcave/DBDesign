using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsSystemController : Controller
    {
        [HttpGet("{uid}")]
        public JsonObject GetAll(string uid)
        {
            List<News_System> list = NewsSystemServer.QueryByAttribute("userid", "\'" + uid + "\'");
            JsonObject ret = new JsonObject();
            ret.Add("Num", list.Count);
            ret.Add("News", JsonObject.Parse(JsonSerializer.Serialize(list)));
            return ret;
        }

        [HttpGet("{uid}/{id}")]
        public JsonObject GetSingle(string id)
        {
            List<News_System> list = NewsSystemServer.Query(id);
            JsonObject ret = new JsonObject();
            ret.Add("Num", list.Count);
            ret.Add("News", JsonObject.Parse(JsonSerializer.Serialize(list)));
            return ret;
        }

        [HttpPost("{uid}/{id}")]
        public JsonObject ReadNews(string uid,string id)
        {
            JsonObject ret = new JsonObject();
            var news = NewsSystemServer.Query(id).FirstOrDefault();
            if(news == null)
            {
                ret.Add("success", false);
                ret.Add("Message","不存在该消息");
                return ret;
            }

            if(news.UserID!=uid)
            {
                ret.Add("success", false);
                ret.Add("Message", "该用户不存在该消息");
                return ret;
            }

            news.IsRead = 1;
            if(NewsSystemServer.Update(news,news.ID)<=0)
            {
                ret.Add("success", false);
                ret.Add("Message", "连接出错");
                return ret;
            }

            ret.Add("success", true);
            return ret;
        }

        [HttpDelete("{uid}/{id}")]
        public JsonObject DeleteNews(string uid, string id)
        {
            JsonObject ret = new JsonObject();
            var news = NewsSystemServer.Query(id).FirstOrDefault();
            if (news == null)
            {
                ret.Add("success", false);
                ret.Add("Message", "不存在该消息");
                return ret;
            }

            if (news.UserID != uid)
            {
                ret.Add("success", false);
                ret.Add("Message", "该用户不存在该消息");
                return ret;
            }

            if(NewsSystemServer.Delete(id)<=0)
            {
                ret.Add("success", false);
                ret.Add("Message", "连接出错");
                return ret;
            }

            ret.Add("success", true);
            return ret;
        }
    }
}
