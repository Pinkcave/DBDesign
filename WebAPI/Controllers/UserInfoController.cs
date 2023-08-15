using Microsoft.AspNetCore.Mvc;
using Repair.Models;
using Repair.Server;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public  class UserInfoController : Controller
    {
        [HttpPost]
        public JsonObject SignUp([FromBody] JsonObject Job)
        {
            int row = -1;
            /*using (StreamReader stream = new StreamReader(Request.Body))
            {
                //读取前端返回数据
                string Json = await stream.ReadToEndAsync(); 
                JsonObject Job = JsonObject.Parse(Json);
                Job.Add("UserId", UserServer.UserNum.ToString());
                row = UserServer.Insert(Job.ToJsonString());
            }*/
            int id = UserServer.Count() + 10000;
            Job.Add("UserId", id.ToString());
            Job.Add("Level", 1);
            UserInfo user = JsonSerializer.Deserialize<UserInfo>(Job);
            row = UserServer.Insert(user);

            JsonObject ret = new JsonObject();
            if (row == 1)
            {
                ret.Add("success", true);
                ret.Add("id", id.ToString());
            }
            else
            {
                ret.Add("success", false);
                ret.Add("Message", "Failed");
            }

            return ret;
        }

        [HttpGet("{uid}")] 
        public IEnumerable<UserInfo> GetUserInfo(string uid)
        {
            List<UserInfo> user= UserServer.Query(uid);
            //Console.WriteLine(uid);
            return user;
        }
        
        [HttpPost("{uid}")]
        public string ModifyUserInfo(string uid, [FromBody] JsonObject Job)
        {
            UserInfo user = JsonSerializer.Deserialize<UserInfo>(Job);
            user.UserId = uid;
            int row = UserServer.Update(user, uid);
            if (row == 1)
                return "{\"status\":true}";
            else
                return "{\"status\":false}";
        }

        [HttpDelete("{uid}")]
        public string DeleteUserInfo(string uid)
        {
            int row = UserServer.Delete(uid);
            if(row == 1)
                return "{\"status\":true}";
            else
                return "{\"status\":false}";
        }
    }
}
