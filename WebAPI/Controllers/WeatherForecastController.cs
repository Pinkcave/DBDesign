using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Repair.Models;
using Repair.Server;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
         };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        public JsonObject ImageSave(string base64)
        {
            JsonObject ret = new JsonObject();
            //string format = base64.Split(',')[0].Split(';')[0].Split('/')[1];
            //base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");
            string format = "jpg";
            byte[] ImageBytes = Convert.FromBase64String(base64);
            MemoryStream mem = new MemoryStream(ImageBytes, 0, ImageBytes.Length);
            mem.Write(ImageBytes, 0, ImageBytes.Length);
            string path = "wwwroot/Image/demo." + format;
            Image image = Image.FromStream(mem);
            image.Save(path);
            path = "http://110.42.220.245:8081/Image/demo." + format;
            ret.Add("url", path);
            return ret;
        }
        [HttpGet("ImageTest")]
        public JsonObject EntryImage()
        {
            string base64 = Convert.ToBase64String(System.IO.File.ReadAllBytes("wwwroot/Image/BeijingSC.jpg"));
            return ImageSave(base64);
        }

        [HttpGet]
        public JsonObject Get()
        {
            /*return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();*/
            List<WeatherForecast> forecasts = new List<WeatherForecast>();
            for (int index = 1; index <= 5; index++)
            {
                forecasts.Add(new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                });
            }
            
            Repair_Order order = new Repair_Order();
            Repair_Options options = new Repair_Options();
            Repair_Cate cate = new Repair_Cate();
            Coupon coupon = new Coupon();
            coupon.Id = "001";
            coupon.Name = "Discount";
            coupon.Status = true;
            cate.Name = "TBD";
            cate.Image = "url";
            cate.ID = "001";
            cate.Detail = "details";
            options.OptionID = "001";
            options.CateName = cate.Name;
            options.CateImage = cate.Image;
            options.CateId = cate.ID;
            options.RepairRequirement = "***";
            options.RepairPrice = 100;
            options.Brand = "Apple";
            options.CateDetail = "***";
            order.CouObj = coupon;
            order.CouponID = coupon.Id;
            order.OrderID = "001";
            order.OrderStatus = 1;
            order.CreateTime = DateTime.Now;
            order.RepairLocation = "TBD";
            order.EngineerID = "001";
            order.RepairOptionID = options;
            order.UserID = "123456";
            order.UserRate = "5";
            order.OrderPrice = 100;
            order.RepairTime = DateTime.Now;
            List<Repair_Order> orders = new List<Repair_Order>();
            orders.Add(order);
            JsonObject ret  = new JsonObject();
            ret.Add("num", orders.Count);
            ret.Add("orders",JsonObject.Parse(JsonSerializer.Serialize(order)));
            return ret;
        }
    

        [HttpPost]
        public string Verify([FromBody] JsonObject jo)
        {
            WeatherForecast forecast = JsonSerializer.Deserialize<WeatherForecast>(jo);
            return JsonSerializer.Serialize(forecast);
        }
    }
}