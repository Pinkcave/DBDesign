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
            //byte[] ImageBytes = Convert.FromBase64String(base64);
            MemoryStream mem = new MemoryStream(Convert.FromBase64String(base64));
            //mem.Write(ImageBytes, 0, ImageBytes.Length);
            string path = "wwwroot/Image/demo." + format;
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            byte[] MemBytes = mem.ToArray();
            fs.Write(MemBytes, 0, MemBytes.Length);
            //Image image = Image.FromStream(mem);
            //image.Save(path);
            path = "http://110.42.220.245:8081/Image/demo." + format;
            ret.Add("url", path);
            return ret;
        }
        [HttpGet("ImageTest")]
        public JsonObject EntryImage([FromForm]string Json)
        {
           
            string urlset = FileController.ToUrlSet("http://110.42.220.245:8081/Image/Airpods1", "jpg", 3);
            JsonObject ret = new JsonObject();
            ret.Add("ok", urlset);
            return ret;
        }

        [HttpGet]
        public JsonObject Get()
        {
            NewsSystemServer.AddNews("10000", "abc", "efg");


            JsonObject ret = new JsonObject();
            ret.Add("success", false);
            ret.Clear();
            ret.Add("success", true);
            
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