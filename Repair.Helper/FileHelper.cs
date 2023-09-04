using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Repair.Helper
{
    public class FileHelper
    {
        public static bool SaveFile(Stream stream,string rootpath,string filename)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            FileStream fs = new FileStream(rootpath + '/' + filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            return true;
        }
        public static bool DeleteFile(string path)
        {
            if(!File.Exists(path))
                return false;
            File.Delete(path);
            return true;
        }
        

        public static string UrlListToSet(List<string> urls)
        {
            string ret = "Urlset(";
            for(int i=0;i<urls.Count; i++)
            {
                if (i != 0)
                    ret += ",";
                ret += "\'" + urls[i] + "\'";
            }
            ret += ")";
            return ret;
        }
    }
}
