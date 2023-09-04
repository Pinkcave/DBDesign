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
            JsonObject ret = new JsonObject();
            if (Job.ContainsKey("UserName") && Job.ContainsKey("Password") && Job.ContainsKey("Name") && Job.ContainsKey("Identity")
                && Job.ContainsKey("Telephone") && Job.ContainsKey("Email"))
            {
                int id = UserServer.Count() + 10000;
                int row = -1;
                Job.Add("UserId", id.ToString());
                Job.Add("Level", 1);
                UserInfo user = JsonSerializer.Deserialize<UserInfo>(Job);
                user.Balance = 0;
                row = UserServer.Insert(user);
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
            }
            else
            {
                ret.Add("success", false);
                ret.Add("Message", "缺少信息");
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
            if (Job.ContainsKey("UserName") && Job.ContainsKey("Password") && Job.ContainsKey("Name") && Job.ContainsKey("Identity")
                && Job.ContainsKey("Telephone") && Job.ContainsKey("Email"))
            {
                UserInfo user = UserServer.Query(uid).FirstOrDefault();
                user.UserName = Job["UserName"].ToString();
                user.Password = Job["Password"].ToString();
                user.Name = Job["Name"].ToString();
                user.Identity = Job["Identity"].ToString();
                user.Telephone = Job["Telephone"].ToString();
                user.Email = Job["Email"].ToString();
                int row = UserServer.Update(user, uid);
                if (row == 1)
                    return "{\"status\":true}";
                else
                    return "{\"status\":false}";
            }
            else
            {
                return "{\"status\":false,\"Message\":\"缺少信息\"}";
            }
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
