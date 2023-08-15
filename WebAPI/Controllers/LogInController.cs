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
using System.Text;

namespace WebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class LogInController : Controller
    {
        private List<VerifyCode> verifycodes = new List<VerifyCode>();
        public LogInController()
        {
            verifycodes.Add(new VerifyCode() { CodeToken = "K9bF2", CodeImage = "http://110.42.220.245:8081/Image/verifycode.jpeg" });
            verifycodes.Add(new VerifyCode() { CodeToken = "AEZUT", CodeImage = "http://110.42.220.245:8081/Image/verifycode1.png" });
            verifycodes.Add(new VerifyCode() { CodeToken = "8NYF5", CodeImage = "http://110.42.220.245:8081/Image/verifycode2.png" });
            verifycodes.Add(new VerifyCode() { CodeToken = "QFCPE", CodeImage = "http://110.42.220.245:8081/Image/verifycode3.png" });
            verifycodes.Add(new VerifyCode() { CodeToken = "FGIP", CodeImage = "http://110.42.220.245:8081/Image/verifycode4.png" });
            verifycodes.Add(new VerifyCode() { CodeToken = "VENR", CodeImage = "http://110.42.220.245:8081/Image/verifycode5.png" });
            verifycodes.Add(new VerifyCode() { CodeToken = "6KAY", CodeImage = "http://110.42.220.245:8081/Image/verifycode6.png" });
            verifycodes.Add(new VerifyCode() { CodeToken = "Q8UC", CodeImage = "http://110.42.220.245:8081/Image/verifycode7.png" });
            verifycodes.Add(new VerifyCode() { CodeToken = "QPZ6", CodeImage = "http://110.42.220.245:8081/Image/verifycode8.png" });
            verifycodes.Add(new VerifyCode() { CodeToken = "kBV8", CodeImage = "http://110.42.220.245:8081/Image/verifycode9.png" });
        }
        [HttpGet]
        public string UserVerify()
        {
            VerifyCode vcode = new VerifyCode();
            var rd = new Random();
            vcode = verifycodes[rd.Next(0,10)];
            return JsonSerializer.Serialize(vcode);
        }

        [HttpPost("User")]
        [Consumes("application/json")]
        public async Task<JsonObject> UserLogIn()
        {

            bool isFound = false;

            if (Request != null)
            {
                using (StreamReader stream = new StreamReader(Request.Body))
                {
                    string Json = await stream.ReadToEndAsync();
                    Console.WriteLine(Json);
                    if (Json != null && Json != "")
                    {
                        JsonObject Jobject = (JsonObject)(JsonObject.Parse(Json));
                        List<UserInfo> users = UserServer.Query(Jobject["UserId"].ToString());
                        for (int i = 0; i < users.Count; i++)
                        {
                            if (users[i].Password == Jobject["Password"].ToString())
                            {
                                isFound = true;
                                break;
                            }
                        }
                    }
                }
                /*
                List<UserInfo> users = UserServer.Query(userlog.UserId);
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Password == userlog.Password)
                    {
                        isFound = true;
                        break;
                    }
                }*/
            }
            JsonObject ret = new JsonObject();
            if (isFound)
            {
                ret.Add("success", true);
            }
            else
            {
                ret.Add("success", false);
            }
            return ret;
        }

        [HttpOptions("User")]
        public void Option()
        {
            Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:8080";
        }

        /*
        [HttpPost("Engineer")]
        public string EngineerLogIn()
        {
            bool isFound = false;
            using (StreamReader stream = new StreamReader(Request.Body))
            {
                //读取前端返回数据
                string Json = stream.ReadToEnd();
                var engineer = JsonSerializer.Deserialize<Engineer>(Json);
                //访问数据库
                if (engineer != null)
                {
                    string sql = "select Password from Engineer where ID=" + "\'" + engineer.ID + "\'";
                    var con = DBHelper.GetConnection();
                    var result = DBHelper.ReadInfo(con, sql);
                    for (int i = 0; i < result.Count; i++)
                    {
                        if (result[i]["PASSWORD"] == engineer.Password)
                            isFound = true;
                    }
                    DBHelper.Disconnection(con);
                }
            }
            string? ret = null;
            if (isFound)
            {
                ret = "{\"success\":true,\"message\":\"Log In Successfully\"}";
            }
            else
            {
                ret = "{\"success\":false,\"message\":\"Log In Failed\"}";
            }
            return ret;
        }*/
        /*
        [Route("")]
        public static VerifyCode GenerateVerifyCode(int codeW, int codeH)
        {
            //生成验证码
            Random rd = new Random();
            string code = "";
            string chars = "ABCDEFGHIJKLMNOPQRSTUWVXYZ0123456789abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < 5; i++)
            {
                code += chars[rd.Next(0, chars.Length)];
            }

            //创建画布
            Bitmap bitmap = new Bitmap(codeW, codeH);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            //画画
            for (int i = 0; i < code.Length; i++)
            {
                Font font = new Font("Arial", 16);
                graphics.DrawString(code[i].ToString(), font, new SolidBrush(Color.Black), (float)i * 15 + 2, (float)0);
            }

            //写入内存
            try
            {
                //MemoryStream stream = new MemoryStream();
                string url = "wwwroot/Image/verifycode.jpeg";
                bitmap.Save(url, System.Drawing.Imaging.ImageFormat.Jpeg);
                url = "Image/verifycode.jpeg";
                VerifyCode vcode = new VerifyCode() { CodeToken = code, CodeImage = "https://110.42.220.245:8081/" + url };
                return vcode;
            }

            //释放资源
            finally
            {
                graphics.Dispose();
                bitmap.Dispose();
            }
        }*/
    }
}
