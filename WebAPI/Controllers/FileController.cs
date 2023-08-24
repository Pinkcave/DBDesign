using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class FileController : Controller
    {
        public static bool SaveFile(IFormFileCollection files, string path)
        {
            foreach (var file in files)
            {
                Stream stream = file.OpenReadStream();
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            return true;
        }

        public static string ToUrlSet(string root,string format,int count)
        {
            string ret = "Urlset(";
            for(int i = 0; i < count; i++)
            {
                if (i != 0)
                    ret += ",";
                ret += string.Format("\'{0}({1}).{2}\'", root, i, format);
            }
            ret += ")";
            return ret;
        }
    }
}
